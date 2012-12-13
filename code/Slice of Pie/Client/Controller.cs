using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows;

namespace Client
{
    class Controller
    {
        //Singleton instance of controller
        private static Controller instance;
        //refrence to model
        private Model model;
        //refrence to gui
        private MainWindow gui;

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

        /// <summary>
        /// Method used to get the main gui window
        /// </summary>
        /// <param name="gui">Refrence of the main gui window</param>
        public void SetGui(MainWindow gui)
        {
            this.gui = gui;
        }

        /// <summary>
        /// Method to register a new user in the database
        /// Using an email and 2 identical passwords, in order protect against typoes
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="passUnencrypted1">unencrypted first password</param>
        /// <param name="passUnencrypted2">unencrypted second password</param>
        /// <returns>Wheter the creation of the user was successful</returns>
        public Boolean RegisterUser(string email, string passUnencrypted1, string passUnencrypted2)
        {
            //boolean which will be returned
            Boolean successful = false;

            //Check if something have been entered as email - WE DO NOT CHECK THAT IT IS AN EMAIL
            if (email.Length > 0)
            {
                //Check that something has been entered as been entered as passwords and check that the two passwords are identical
                if (passUnencrypted1 != null && passUnencrypted1.Length > 0 && passUnencrypted2 != null && passUnencrypted1 == passUnencrypted2)
                {
                    //Register user
                    successful = model.RegisterUser(email, passUnencrypted1);
                    if (!successful) //if the user already exsist
                    {
                        System.Windows.MessageBox.Show("User aldready exsists", "Creation error");
                    }
                    else //the user has been created
                    {
                        System.Windows.MessageBox.Show("User with email: " + email + " have been successfully created", "Successful");
                    }
                }
                else //the passwords does not match
                {
                    System.Windows.MessageBox.Show("User could not be created. Entered passwords does not match", "Creation error");
                }
            }
            else //no email was entered
            {
                System.Windows.MessageBox.Show("Enter email address", "Creation error");
            }
            return successful; //return the result
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
                    MessageBox.Show("Wrong email or password", "Unable to login");
                }
            }
            else
            {
                MessageBox.Show("Enter email and password", "Login error");
            }
            return successful;
        }


        public void SetOpenDocument(String content, String title, String documentPath)
        {
            //try catch in case of corrupted file
            Model.GetInstance().CurrentDocumentPath = documentPath;
            FlowDocument doc = model.CreateFlowDocumentWithoutMetadata(content);
            gui.richTextBox.Document = doc;
            gui.labelOpenDocument.Content = "Current document: " + title;
            model.CurrentDocumentTitle = title;
        }

        public void CreateDocument(String title)
        {
            FlowDocument emptyDoc = new FlowDocument();
            model.CreateDocument(title, emptyDoc);
            String documentPath = model.RootFolder + "\\" + title + ".txt";
            SetOpenDocument(System.Windows.Markup.XamlWriter.Save(emptyDoc), title, documentPath);
            UpdateExplorerView();
        }

        public void SaveDocument(FlowDocument document)
        {
            if (model.CurrentDocumentPath != null && model.CurrentDocumentPath.Length > 0)
            {
                model.SaveDocumentToFile(document);
                UpdateExplorerView();
            }
            else
            {
                MessageBox.Show("No document open", "Document save error");
            }
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

        public void SyncAllDocuments()
        {
            model.SyncAllDocuments();
            UpdateExplorerView();
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

        public void SyncDocument(FlowDocument document)
        {
            String[][] response = model.SyncDocument(document);

            if (response != null)
            {
                gui.SetupMergeView(response);
            }
        }

        public void Logout()
        {
            model.LogoutUser();
            SetOpenDocument(System.Windows.Markup.XamlWriter.Save(new FlowDocument()), "", "");
            UpdateExplorerView();
        }

        public void SaveMergedDocument(FlowDocument document)
        {
            model.SaveMergedDocument(document);
        }
    }
}