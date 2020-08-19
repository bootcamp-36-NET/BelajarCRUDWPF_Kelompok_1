using BelajarCRUDWPF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        MyContext myContext = new MyContext();

        public ChangePassword()
        {
            InitializeComponent();
            btnProceed.IsEnabled = false;
        }

        public void ErrorCheck()
        {
            string password = pbPassword.Password;
            string confrimPassword = pbPasswordConfirmation.Password;
            if (string.IsNullOrEmpty(password))
            {
                lblErrorMessage.Content = "Please fill password field !";
                btnProceed.IsEnabled = false;
                return;
            }
            if (string.IsNullOrEmpty(confrimPassword))
            {
                lblErrorMessage.Content = "Please fill confirmation password field !";
                btnProceed.IsEnabled = false;
                return;
            }
            if (confrimPassword != password)
            {
                lblErrorMessage.Content = "Password and confirm password is different !";
                btnProceed.IsEnabled = false;
                return;
            }
            lblErrorMessage.Content = "";
            btnProceed.IsEnabled = true;
        }
        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ErrorCheck();
        }

        private void PbPasswordConfirmation_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ErrorCheck();
        }

        private void BtnProceed_Click(object sender, RoutedEventArgs e)
        {
            var userEmail = App.Current.Properties[2].ToString();
            var supplier = myContext.Suppliers.Where(Q => Q.Email.Equals(userEmail)).FirstOrDefault();
            var hashPassword = BCrypt.Net.BCrypt.HashString(pbPassword.Password, 12);
            supplier.IsReset = 0;
            supplier.Password = hashPassword;
            myContext.SaveChanges();
            MessageBox.Show("Password has been changed !");

            App.Current.Properties[2] = null;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
