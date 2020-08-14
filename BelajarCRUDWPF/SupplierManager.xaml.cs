﻿using BelajarCRUDWPF.Context;
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
    public partial class SupplierManager : UserControl
    {
        MyContext myContext = new MyContext();

        Regex r = new Regex("^[a-zA-Z0-9. ]*$");

        public SupplierManager()
        {
            InitializeComponent();
            btnInsert.IsEnabled = false;
            dataGridSupplier.ItemsSource = myContext.Suppliers.ToList();
        }

        public void Reset()
        {
            txtId.Text = "";
            txtName.Text = "";
            lblNameStatus.Content = "";
            txtSearch.Text = "";

            dataGridSupplier.SelectedItem = null;
            dataGridSupplier.ItemsSource = myContext.Suppliers.ToList();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                var existingSupplier = myContext.Suppliers.Find(Convert.ToInt32(txtId.Text));
                existingSupplier.Name = txtName.Text;
                myContext.SaveChanges();
                MessageBox.Show("Data has beed Updated !");
            }
            else
            {
                var supplier = new Supplier()
                {
                    Name = txtName.Text
                };

                myContext.Suppliers.Add(supplier);
                myContext.SaveChanges();
                MessageBox.Show("Data has beed Added !");
            }

            Reset();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblNameStatus.Content = "Name Cannot Empty !";
                btnInsert.IsEnabled = false;
            }
            else if (!r.IsMatch(txtName.Text))
            //else if (!txtName.Text.All(char.IsLetterOrDigit))
            {
                lblNameStatus.Content = "Name Must Contain only number and text and . !";
                btnInsert.IsEnabled = false;
            }
            else
            {
                lblNameStatus.Content = "";
                btnInsert.IsEnabled = true;
            }
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
                var item = dataGridSupplier.SelectedItem as Supplier;
                txtId.Text = Convert.ToString(item.Id);
                txtName.Text = item.Name;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void BtnDelete2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSupplier.SelectedItem == null)
            {
                return;
            }
            if (MessageBox.Show("Do you want to delete this data?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var item = dataGridSupplier.SelectedItem as Supplier;
                var existingSupplier = myContext.Suppliers.Find(item.Id);
                if (existingSupplier != null)
                {
                    myContext.Suppliers.Remove(existingSupplier);
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
            var filteredData = myContext.Suppliers.Where(Q => Q.Id.ToString().Contains(txtSearch.Text) || Q.Name.Contains(txtSearch.Text)).ToList();
            dataGridSupplier.ItemsSource = filteredData;

        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredData = myContext.Suppliers.Where(Q => Q.Id.ToString().Contains(txtSearch.Text) || Q.Name.Contains(txtSearch.Text)).ToList();
            dataGridSupplier.ItemsSource = filteredData;
        }
    }
}
