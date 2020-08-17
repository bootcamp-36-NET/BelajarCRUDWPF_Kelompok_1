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
    /// Interaction logic for ItemManager.xaml
    /// </summary>
    public partial class ItemManager : UserControl
    {
        MyContext myContext = new MyContext();

        Regex alphanumRegex = new Regex("^[a-zA-Z0-9. ]*$");
        //Regex numRegex = new Regex("[^0-9]+");

        public ItemManager()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            dataGridSupplier.ItemsSource = myContext.Items.ToList(); 
            cmbxSupplierId.ItemsSource = myContext.Suppliers.Select(Q => Q.Id).ToList(); ;
        }

        public void Reset()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtSearch.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
            cmbxSupplierId.SelectedItem = null;
            dataGridSupplier.SelectedItem = null;

            lblNameStatus.Content = "";
            dataGridSupplier.ItemsSource = myContext.Items.ToList();
        }

        public void ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblNameStatus.Content = "Name Cannot Empty !";
                btnInsert.IsEnabled = false;
            }
            else if (!alphanumRegex.IsMatch(txtName.Text))
            //else if (!txtName.Text.All(char.IsLetterOrDigit))
            {
                lblNameStatus.Content = "Name Must Contain only number and text and . !";
                btnInsert.IsEnabled = false;
            }
            else if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                lblNameStatus.Content = "Price Cannot Empty !";
                btnInsert.IsEnabled = false;
            }
            else if (!txtPrice.Text.All(char.IsDigit))
            {
                lblNameStatus.Content = "Price Must be Numeric !";
                btnInsert.IsEnabled = false;
            }
            else if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                lblNameStatus.Content = "Stock Cannot Empty !";
                btnInsert.IsEnabled = false;
            }
            else if (!txtStock.Text.All(char.IsDigit))
            {
                lblNameStatus.Content = "Stock Must be Numeric !";
                btnInsert.IsEnabled = false;
            }
            else if (cmbxSupplierId.SelectedItem == null)
            {
                lblNameStatus.Content = "Supplier Id Cannot be empty !";
                btnInsert.IsEnabled = false;
            }
            else
            {
                lblNameStatus.Content = "";
                btnInsert.IsEnabled = true;
            }
        }

        //UPDATE OR INSERT
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                //UPDATE
                var existingSupplier = myContext.Items.Find(Convert.ToInt32(txtId.Text));
                existingSupplier.Name = txtName.Text;
                existingSupplier.Price = Convert.ToInt32(txtPrice.Text);
                existingSupplier.Stock = Convert.ToInt32(txtStock.Text);
                existingSupplier.SupplierId = Convert.ToInt32(cmbxSupplierId.SelectedValue);

                myContext.SaveChanges();
                MessageBox.Show("Data has beed Updated !");
            }
            else
            {
                //INSERT
                var item = new Item()
                {
                    Name = txtName.Text,
                    Price = Convert.ToInt32(txtPrice.Text),
                    Stock = Convert.ToInt32(txtStock.Text),
                    SupplierId = Convert.ToInt32(cmbxSupplierId.SelectedValue)
                };

                myContext.Items.Add(item);
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

        private void DataGridSupplier_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGridSupplier.SelectedItem != null)
            {
                var item = dataGridSupplier.SelectedItem as Item;
                txtId.Text = Convert.ToString(item.Id);
                txtName.Text = item.Name;
                txtPrice.Text = Convert.ToString(item.Price);
                txtStock.Text = Convert.ToString(item.Stock);
                cmbxSupplierId.SelectedValue = item.SupplierId;
            }
            ErrorCheck();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSupplier.SelectedItem == null)
            {
                return;
            }
            if (MessageBox.Show("Do you want to delete this data?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var item = dataGridSupplier.SelectedItem as Item;
                var existingItem = myContext.Items.Find(item.Id);
                if (existingItem != null)
                {
                    myContext.Items.Remove(existingItem);
                    myContext.SaveChanges();
                    MessageBox.Show("Data successfully Deleted !");
                }
                MessageBox.Show("Data successfully Deleted !");
            }
            else
            {
                MessageBox.Show("Canceled !");
            }

            Reset();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var filteredData = myContext.Items.Where(Q => Q.Id.ToString().Contains(txtSearch.Text) || Q.Name.Contains(txtSearch.Text) || Q.Price.ToString().Contains(txtSearch.Text) || Q.Stock.ToString().Contains(txtSearch.Text) || Q.SupplierId.ToString().Contains(txtSearch.Text)).ToList();
            dataGridSupplier.ItemsSource = filteredData;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredData = myContext.Items.Where(Q => Q.Id.ToString().Contains(txtSearch.Text) || Q.Name.Contains(txtSearch.Text) || Q.Price.ToString().Contains(txtSearch.Text) || Q.Stock.ToString().Contains(txtSearch.Text) || Q.SupplierId.ToString().Contains(txtSearch.Text)).ToList();
            dataGridSupplier.ItemsSource = filteredData;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void DtpckrJoinDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void TxtStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void TxtSupplierId_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }

        private void TxtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorCheck();
        }


        private void CmbxSupplierId_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ErrorCheck();
        }
    }
}
