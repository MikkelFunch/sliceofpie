using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;

        public MainWindow()
        {
            InitializeComponent();
            controller = Controller.GetInstance();
            controller.SetGui(this);
        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            string url = null;

            ImageURLDialog imDialog = new ImageURLDialog();
            imDialog.ShowDialog();

            //get inserted url
            url = imDialog.URLString;

            //check if it is a valid url
            if (url != null && url.StartsWith("http://"))
            {
                BitmapImage bitmap = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                Image image = new Image();
                image.Source = bitmap;
                image.Width = 130;
                image.Height = 130;

                Paragraph p = (Paragraph) richTextBox.Document.Blocks.LastBlock;
                p.Inlines.Add(image);
                richTextBox.Document.Blocks.Add(p);
            }
        }

        private void LoginItem_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog logDialog = new LoginDialog();
            logDialog.ShowDialog();
            if (logDialog.Online)
            {
                BitmapImage bitmap = new BitmapImage(new Uri("Resources\\greendot.png",UriKind.Relative));
                OnlineImage.Source = bitmap;
            }
        }

        private void RegisterItem_Click(object sender, RoutedEventArgs e)
        {
            RegisterUserDialog userdialog = new RegisterUserDialog();
            userdialog.ShowDialog();
        }

        /// <summary>
        /// Method being run when the explorerview has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExplorerTree_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewModel.LoadFilesAndFolders(ExplorerTree.Items);
        }
    }
}