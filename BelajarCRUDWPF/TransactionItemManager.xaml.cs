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
    /// Interaction logic for TransactionItemManager.xaml
    /// </summary>
    public partial class TransactionItemManager : UserControl
    {
        MyContext myContext = new MyContext();
        Regex alphanumRegex = new Regex("^[a-zA-Z0-9. ]*$");

        public TransactionItemManager()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            dataGridTransactionItem.ItemsSource = myContext.TransactionItems.ToList();
            cmbxTransactionId.ItemsSource = myContext.Transactions.Select(Q => Q.Id).ToList();
            cmbxItemId.ItemsSource = myContext.Items.Select(Q => Q.Id).ToList();
        }

        public void Reset()
        {
            txtId.Text = "";
            txtQuantity.Text = "";
            txtSearch.Text = "";
            cmbxTransactionId.SelectedItem = null;
            cmbxItemId.SelectedItem = null;

            dataGridTransactionItem.SelectedItem = null;
            lblNameStatus.Content = "";
            dataGridTransactionItem.ItemsSource = myContext.TransactionItems.ToList();
        }

        public void ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                lblNameStatus.Content = "Quantity Cannot Empty !";
                btnInsert.IsEnabled = false;
            }
            else if (!txtQuantity.Text.All(char.IsDigit))
            {
                lblNameStatus.Content = "Stock Must be Numeric !";
                btnInsert.IsEnabled = false;
            }

            else if (cmbxTransactionId.SelectedItem == null)
            {
                lblNameStatus.Content = "Transaction Id do not empty";
                btnInsert.IsEnabled = true;
            }

            else if (cmbxItemId.SelectedItem == null)
            {
                lblNameStatus.Content = "Item Id do not empty";
                btnInsert.IsEnabled = true;
            }

            else
            {
                lblNameStatus.Content = "";
                btnInsert.IsEnabled = true;
            }
        }

       

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                //UPDATE
                var existingTransactionItem = myContext.TransactionItems.Find(Convert.ToInt32(txtId.Text));
                existingTransactionItem.Quantity = Convert.ToInt32(txtQuantity.Text);
                existingTransactionItem.TransactionId = Convert.ToInt32(cmbxTransactionId.SelectedValue);
                existingTransactionItem.ItemId = Convert.ToInt32(cmbxItemId.SelectedValue);

                myContext.SaveChanges();
                MessageBox.Show("Data has beed Updated !");
            }
            else
            {
                //INSERT
                var transactionitem = new TransactionItem()
                {
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    TransactionId = Convert.ToInt32(cmbxTransactionId.SelectedValue),
                    ItemId = Convert.ToInt32(cmbxItemId.SelectedValue)
                };

                myContext.TransactionItems.Add(transactionitem);
                myContext.SaveChanges();
                MessageBox.Show("Data has beed Added !");
            }

            Reset();
        }

        private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                btnInsert.Content = "Update";
            }
            else
            {
                btnInsert.Content = "Insert";
            }
        }

        private void dataGridTransactionItem_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridTransactionItem.SelectedItem != null)
            {
                var transactionitem = dataGridTransactionItem.SelectedItem as TransactionItem;
                txtId.Text = Convert.ToString(transactionitem.Id);
                txtQuantity.Text = Convert.ToString(transactionitem.Quantity);
                cmbxTransactionId.SelectedValue = transactionitem.TransactionId;
                cmbxItemId.SelectedValue = transactionitem.ItemId;

            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTransactionItem.SelectedItem == null)
            {
                return;
            }
            if (MessageBox.Show("Do you want to delete this data?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var transactionitem = dataGridTransactionItem.SelectedItem as TransactionItem;
                var existingTransactionItem = myContext.TransactionItems.Find(transactionitem.Id);
                if (existingTransactionItem != null)
                {
                    myContext.TransactionItems.Remove(existingTransactionItem);
                    myContext.SaveChanges();
                    MessageBox.Show("Data successfully Deleted !");
                }
            }
            else
            {
                MessageBox.Show("Canceled !");
            }

            Reset();
        }

        private void cmbxTransactionId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorCheck();
        }

       

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredData = myContext.TransactionItems.Where(Q => Q.Id.ToString().Contains(txtSearch.Text) || Q.Quantity.ToString().Contains(txtSearch.Text) || Q.TransactionId.ToString().Contains(txtSearch.Text) || Q.ItemId.ToString().Contains(txtSearch.Text) || Q.TransactionId.ToString().Contains(txtSearch.Text)).ToList();
            dataGridTransactionItem.ItemsSource = filteredData;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void cmbxTransactionId_SelectionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void cmbxItemId_SelectionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }

    }
}
