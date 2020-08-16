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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for TransactionManager.xaml
    /// </summary>
    public partial class TransactionManager : UserControl
    {
        MyContext mycontext = new MyContext();
        Regex r = new Regex("^[a-zA-Z0-9. ]*$");

        public TransactionManager()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            dataGridTransaction.ItemsSource = mycontext.Transactions.ToList();
        }

        public void Reset()
        {
            txtIdTransaction.Text = "";            
            lblNameStatus.Content = "";
            txtSearchTransaction.Text = "";
            dtpckrOrderDate.SelectedDate = null;

            dataGridTransaction.SelectedItem = null;
            dataGridTransaction.ItemsSource = mycontext.Transactions.ToList();
        }

        public void ErrorCheck()
        {

            if (dtpckrOrderDate.SelectedDate == null)
            {
                lblNameStatus.Content = "Join Date Cannot be empty !";
                btnInsert.IsEnabled = false;
            }
            else
            {
                lblNameStatus.Content = "";
                btnInsert.IsEnabled = true;
            }
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIdTransaction.Text))
            {
                var existingTransaction = mycontext.Transactions.Find(Convert.ToInt32(txtIdTransaction.Text));
                existingTransaction.OrderDate = dtpckrOrderDate.SelectedDate.Value;
                mycontext.SaveChanges();
                MessageBox.Show("Data has beed Updated !");
            }
            else
            {
                var transaction = new Transaction()
                {
                    OrderDate = dtpckrOrderDate.SelectedDate.Value
                };

                mycontext.Transactions.Add(transaction);
                mycontext.SaveChanges();
                MessageBox.Show("Data has beed Added !");
            }

            Reset();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void TxtSearchTransaction_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredData = mycontext.Transactions.Where(Q => Q.Id.ToString().Contains(txtSearchTransaction.Text) || Q.OrderDate.ToString().Contains(txtSearchTransaction.Text)).ToList();
            dataGridTransaction.ItemsSource = filteredData;
        }

        private void dataGridTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridTransaction.SelectedItem != null)
            {
                var item = dataGridTransaction.SelectedItem as Transaction;
                txtIdTransaction.Text = Convert.ToString(item.Id); 
                dtpckrOrderDate.SelectedDate = item.OrderDate;
            }
        }


        private void DtpckrOrderDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var filteredData = mycontext.Transactions.Where(Q => Q.Id.ToString().Contains(txtSearchTransaction.Text) || Q.OrderDate.ToString().Contains(txtSearchTransaction.Text)).ToList();
            dataGridTransaction.ItemsSource = filteredData;
        }

        private void BtnDelete2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTransaction.SelectedItem == null)
            {
                return;
            }
            if (MessageBox.Show("Do you want to delete this data?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var item = dataGridTransaction.SelectedItem as Transaction;
                var existingTransaction = mycontext.Transactions.Find(item.Id);
                if (existingTransaction != null)
                {
                    mycontext.Transactions.Remove(existingTransaction);
                    mycontext.SaveChanges();
                    MessageBox.Show("Data successfully Deleted !");
                }
            
            }
            else
            {
                MessageBox.Show("Canceled !");
            }

            Reset();
        }

        private void TxtIdTransaction_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIdTransaction.Text))
            {
                btnInsert.Content = "Update";
            }
            else
            {
                btnInsert.Content = "Insert";
            }
        }

        private void dataGridTransaction_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
