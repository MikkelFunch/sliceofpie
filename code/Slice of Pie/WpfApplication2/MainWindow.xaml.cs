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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Model m = new Model();
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
                String path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Random\\Gamer 1337.png";
                BitmapImage bitmap = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
                Image image = new Image();
                image.Source = bitmap;
                image.Width = 130;
                image.Height = 130;

                Paragraph p = (Paragraph) documentView.richTextBox.Document.Blocks.LastBlock;
                p.Inlines.Add(image);
                documentView.richTextBox.Document.Blocks.Add(p);
            }
        }
    }
}
