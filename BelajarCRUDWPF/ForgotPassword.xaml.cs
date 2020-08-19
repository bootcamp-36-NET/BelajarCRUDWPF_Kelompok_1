using BelajarCRUDWPF.Context;
using BelajarCRUDWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        MyContext myContext = new MyContext();

        Regex isEmail = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", RegexOptions.Compiled);

        public ForgotPassword()
        {
            InitializeComponent();
            btnReset.IsEnabled = false;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            var supplier = myContext.Suppliers.Where(Q => Q.Email.Contains(txtEmail.Text)).FirstOrDefault();
            if (supplier == null)
            {
                lblErrorMessage.Content = "Email wasn't registered yet !";
                txtEmail.Focus();
                return;
            }

            Guid guid = Guid.NewGuid();

            SendEmail(supplier.Email,guid);

            supplier.IsReset = 1;
            supplier.Password = BCrypt.Net.BCrypt.HashString(guid.ToString(), 12);
            myContext.SaveChanges();
            MessageBox.Show("Password has been sent to " + supplier.Email);

            Login login = new Login();
            Close();
            login.Show();
        }

        public void SendEmail(string email, Guid guid)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("xyztechnologyid@gmail.com", "WPF CRUD's Admin");
                message.Subject = "WPF CRUD Password Request at " + DateTime.Now.ToString("dddd, MMMM dd yyyy");
                message.Body = "Regarding to password reset request, <br><br>We have reset your password to :<br><br><b>" + guid.ToString() + "</b><br><br> Please proceed to reset password page when you login with the new password given !";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.UseDefaultCredentials = false;
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("xyztechnologyid@gmail.com", "HelloWorld");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                lblErrorMessage.Content = "Please enter email !";
                txtEmail.Focus();
                btnReset.IsEnabled = false;
                return;
            }
            if (!isEmail.IsMatch(txtEmail.Text))
            {
                lblErrorMessage.Content = "Please enter a valid email !";
                txtEmail.Focus();
                btnReset.IsEnabled = false;
                return;
            }
            lblErrorMessage.Content = "";
            btnReset.IsEnabled = true;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }
    }
}
