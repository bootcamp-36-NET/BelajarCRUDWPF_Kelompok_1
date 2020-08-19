using BelajarCRUDWPF.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MyContext myContext = new MyContext();
        MainWindow mainWindow = new MainWindow();

        Regex isEmail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", RegexOptions.Compiled);

        public Login()
        {
            InitializeComponent();
            btnLogin.IsEnabled = false;
            cbRememberMe.IsChecked = false;
            App.Current.Properties[3] = null;
            if (App.Current.Properties[0] == null)
            {
                RememberMe();
            }
            else
            {
                IsRegistration();
            }
        }

        public void IsRegistration()
        {
            txtEmail.Text = App.Current.Properties[0].ToString();
            pbPassword.Password = App.Current.Properties[1].ToString();
        }

        public void RememberMe()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default["Email"].ToString()))
            {
                cbRememberMe.IsChecked = true;
                txtEmail.Text = Properties.Settings.Default["Email"].ToString();
                pbPassword.Password = Properties.Settings.Default["Password"].ToString();
            }
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailaddress);
                return addr.Address == emailaddress;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public void ErrorCheck()
        {
            string email = txtEmail.Text;
            string password = pbPassword.Password;
            if (string.IsNullOrEmpty(email))
            {
                lblErrorMessage.Content = "Please enter email !";
                btnLogin.IsEnabled = false;
                return;
            }
            var isValid = IsValidEmail(email);
            if (string.IsNullOrEmpty(password))
            {
                lblErrorMessage.Content = "Please enter password !";
                btnLogin.IsEnabled = false;
                return;
            }
            lblErrorMessage.Content = "";
            btnLogin.IsEnabled = true;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = pbPassword.Password;
            if (!isEmail.IsMatch(email))
            {
                lblErrorMessage.Content = "Please enter a valid email !";
                btnLogin.IsEnabled = false;
                return;
            }
            var supplier = myContext.Suppliers.Where(Q => Q.Email == email).FirstOrDefault();
            if (supplier == null)
            {
                lblErrorMessage.Content = "email wasn't registered !";
                txtEmail.Focus();
                return;
            }
            var valid = BCrypt.Net.BCrypt.Verify(password, supplier.Password);
            if (!valid)
            {
                lblErrorMessage.Content = "Incorrect password !";
                pbPassword.Focus();
                return;
            }
            if (cbRememberMe.IsChecked == true)
            {
                Properties.Settings.Default["Email"] = email;
                Properties.Settings.Default["Password"] = password;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default["Email"] = "";
                Properties.Settings.Default["Password"] = "";
                Properties.Settings.Default.Save();
            }

            App.Current.Properties[0] = null;
            App.Current.Properties[1] = null;

            if(supplier.IsReset == 1)
            {
                App.Current.Properties[2] = supplier.Email;
                Properties.Settings.Default["Password"] = "";
                Properties.Settings.Default.Save();
                ChangePassword changePassword = new ChangePassword();
                changePassword.Show();
                Close();
                return;
            }

            mainWindow.Show();
            Close();
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ErrorCheck();
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }


        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            Close();
        }

        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            Close();
        }
    }
}
