using BelajarCRUDWPF.Context;
using BelajarCRUDWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        MyContext myContext = new MyContext();

        Regex isEmail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", RegexOptions.Compiled);

 

        public Register()
        {
            InitializeComponent();
            btnRegister.IsEnabled = false;
            App.Current.Properties[0] = null;
            App.Current.Properties[1] = null;
        }

        public void Errorcheck()
        {
            string email = txtEmail.Text;
            string password = pbPassword.Password;
            string confrimPassword = pbPasswordConfirmation.Password;
            if (string.IsNullOrEmpty(email))
            {
                lblErrorMessage.Content = "Please fill email field !";
                btnRegister.IsEnabled = false;
                return;
            }
            if (!isEmail.IsMatch(email))
            {
                lblErrorMessage.Content = "Please enter a valid email !";
                btnRegister.IsEnabled = false;
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                lblErrorMessage.Content = "Please fill password field !";
                btnRegister.IsEnabled = false;
                return;
            }
            if (string.IsNullOrEmpty(confrimPassword))
            {
                lblErrorMessage.Content = "Please fill confirmation password field !";
                btnRegister.IsEnabled = false;
                return;
            }
            if (confrimPassword != password)
            {
                lblErrorMessage.Content = "Password and confirm password is different !";
                btnRegister.IsEnabled = false;
                return;
            }
            lblErrorMessage.Content = "";
            btnRegister.IsEnabled = true;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var isEmailExist = myContext.Suppliers.Where(Q => Q.Email == txtEmail.Text).Any();
            if (isEmailExist)
            {
                lblErrorMessage.Content = "Email already exist !";
                btnRegister.IsEnabled = false;
                txtEmail.Focus();
                return;
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(pbPassword.Password,12);
            var supplier = new Supplier()
            {
                Name = "",
                JoinDate = DateTime.Now,
                Email = txtEmail.Text,
                Password = hashPassword,
                IsReset = 0
            };
            myContext.Suppliers.Add(supplier);
            myContext.SaveChanges();
            MessageBox.Show("User has been created !");

            App.Current.Properties[0] = txtEmail.Text;
            App.Current.Properties[1] = pbPassword.Password;

            Login login = new Login();
            Close();
            login.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            Errorcheck();
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Errorcheck();
        }

        private void PbPasswordConfirmation_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Errorcheck();
        }
    }
}
