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
using System.Windows.Shapes;

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for RegisterUserDialog.xaml
    /// </summary>
    public partial class RegisterUserDialog : Window
    {
        public RegisterUserDialog()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string email = textBoxEmail.Text;
            string passUnencrypted1 = passwordBox1.Password;
            string passUnencrypted2 = passwordBox2.Password;

            if (Controller.GetInstance().RegisterUser(email, passUnencrypted1, passUnencrypted2) != -1)
            {
                this.Cursor = Cursors.Arrow;
                this.Close();
            }
            this.Cursor = Cursors.Arrow;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxEmail.Focus();
        }
    }
}