using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace Client
{
    class Controller
    {
        //Singleton instance of controller
        private static Controller instance;
        private Model model;
        private MainWindow gui;

        public static void Main(String[] args)
        {
            Client.App app = new Client.App();
            app.Run();
            Console.WriteLine("test");
        }

        /// <summary>
        /// Private constructor to insure that Controller is not created outside this class.
        /// </summary>
        private Controller()
        {
            model = Model.GetInstance();
        }

        /// <summary>
        /// Accessor method for accessing the single instance of controller.
        /// </summary>
        /// <returns>The only instance of controller</returns>
        public static Controller GetInstance()
        {
            if (instance == null)
            {
                instance = new Controller();
            }
            return instance;
        }

        public void SetGui(MainWindow gui)
        {
            this.gui = gui;
        }

        public Boolean RegisterUser(string email, string passUnencrypted1, string passUnencrypted2)
        {
            Boolean successful = false;

            if (email.Length > 0)
            {
                if (passUnencrypted1 != null && passUnencrypted1.Length > 0 && passUnencrypted1 == passUnencrypted2)
                {
                    successful = model.RegisterUser(email, passUnencrypted1);
                    if (!successful)
                    {
                        System.Windows.MessageBox.Show("User aldready exsists", "Creation error");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("User with email: " + email + " have been successfully created", "Successful");
                    }
                }
                else //the passwords does not match
                {
                    System.Windows.MessageBox.Show("User could not be created. Entered passwords does not match", "Creation error");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Enter email address", "Creation error");
            }
            return successful;
        }

        public bool LoginUser(string email, string unencrytedPass)
        {
            Boolean successful = false;
            if (unencrytedPass.Length > 0 && email.Length > 0)
            {
                String pass = Security.EncryptPassword(unencrytedPass);
                int userID = model.LoginUser(email, pass);
                if (userID != -1)
                {
                    System.Windows.MessageBox.Show("Logged in successfully", "Login");
                    successful = true;
                    UpdateExplorerView();
                }
                else
                {
                    System.Windows.MessageBox.Show("Wrong email or password", "Unable to login");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Enter email and password", "Login error");
            }
            return successful;
        }


        public void SetOpenDocument(String content, String fileName)
        {
            //try catch in case of currupted file
            FlowDocument doc = (FlowDocument)System.Windows.Markup.XamlReader.Parse(content);
            gui.richTextBox.Document = doc;
            gui.labelOpenDocument.Content = "Current document: " + fileName;
        }

        public void CreateDocument(String title)
        {
            model.CreateDocument(title, gui.richTextBox.Document);
            
            UpdateExplorerView();
        }

        public void SaveDocument(FlowDocument document)
        {
            model.SaveDocument(document);
            UpdateExplorerView();
        }

        private void UpdateExplorerView()
        {
            TreeViewModel.GetInstance().LoadFilesAndFolders(gui.ExplorerTree.Items);
        }

        public void DownloadComplete(object sender, EventArgs e)
        {
            System.Windows.Media.Imaging.BitmapImage image = (System.Windows.Media.Imaging.BitmapImage)sender;
            
            model.DownloadComplete(image);
        }
        /*
        public void SaveDocument(String name, int userId, int folderId, int documentId, String content)
        {
            if (documentId != null)
            {
                model.SaveDocument(name, userId, folderId, content);
            }
            else
            {
                model.SaveDocumentRevision(userId, documentId, content);
            }
        }
        */
    }
}