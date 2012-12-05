﻿using System;
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
    /// Interaction logic for ButtonMenuView.xaml
    /// </summary>
    public partial class ButtonMenuView : UserControl
    {
        public ButtonMenuView()
        {
            InitializeComponent();
        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            string url = null;

            ImageURLDialog imDialog = new ImageURLDialog();
            if (imDialog.ShowDialog() == true)
            {
                url = imDialog.URLString;
            }

            if(url != null && url.StartsWith("http:\\"))
            {
                //insert into document
            }
        }
    }
}