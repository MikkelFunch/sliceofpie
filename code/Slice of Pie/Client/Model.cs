using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Threading;

namespace Client
{
    class Model
    {
        private static Model instance;

        private Model()
        {
            RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\";
            UserID = -1;
        }

        private String RootFolder
        {
            get;
            set;
        }

        public String CurrentDocument
        {
            get;
            set;
        }

        public static Model GetInstance()
        {
            if (instance == null)
            {
                instance = new Model();
            }
            return instance;
        }

        public static int UserID
        {
            get;
            set;
        }

        public static String Email
        {
            get;
            private set;
        }

        public Boolean RegisterUser(string email, string passUnencrypted)
        {
            Boolean successful = false;
            string pass = Security.EncryptPassword(passUnencrypted);
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                successful = proxy.AddUser(email, pass);
            }
            return successful;
        }

        public int LoginUser(string email, string pass)
        {
            int id = -1;
            using (WcfServiceReference.ServiceClient proxy = new WcfServiceReference.ServiceClient())
            {
                id = proxy.GetUserByEmailAndPass(email, pass);
            }
            if (id != -1)
            {
                //User logged in
                UserID = id;
                Email = email;
                RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\" + Email;
                Directory.CreateDirectory(RootFolder);
            }
            return id;
        }

        public void CreateDocument(String title, FlowDocument document)
        {
            String path = RootFolder + "\\" + title + ".txt";
            File.Create(path).Close();
            CurrentDocument = path;
            SaveDocument(document);
        }

        public void SaveDocument(FlowDocument document)
        {
            /* Find all images in the given document
            foreach (Block block in document.Blocks)
            {
                if (block is Paragraph)
                {
                    Paragraph paragraph = (Paragraph)block;
                    foreach (Inline inline in paragraph.Inlines)
                    {
                        if (inline is InlineUIContainer)
                        {
                            InlineUIContainer uiContainer = (InlineUIContainer)inline;
                            if (uiContainer.Child is Image)
                            {
                                Image image = (Image)uiContainer.Child;
                                BitmapImage localImage = new BitmapImage(new Uri(image.Tag.ToString(), UriKind.RelativeOrAbsolute));
                                image.Source = localImage;
                            }
                        }
                    }
                }
            }*/


            //Create the document and write the content to it.
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(CurrentDocument))) //when no document has been chosen/opened, path is not set
            {
                sw.Write(System.Windows.Markup.XamlWriter.Save(document));
            }
        }

        public void DownloadComplete(BitmapImage image)
        {
            String url = image.UriSource.ToString();
            String picsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sliceofpie\\pics";
            String fileName = Security.EncryptPassword(url) + ".jpg";
            if(File.Exists(picsPath + fileName))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();

                Directory.CreateDirectory(picsPath);
                String photolocation = picsPath + fileName; //file name

                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var filestream = new FileStream(photolocation, FileMode.Create))
                    encoder.Save(filestream);
                Semaphore token = new Semaphore(0, 1, "Token");
                token.Release();
            }
        }
    }
}