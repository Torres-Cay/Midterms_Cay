using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Midterms_Cay
{
    public partial class MainWindow : Window
    {
        // Master storage collections
        public List<Product> Products { get; set; }
        public List<CartItem> AddedCart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Products = new List<Product>();
            AddedCart = new List<CartItem>();
            this.DataContext = this;

            // Initialize display grid
            RefreshProductGrid();
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        public class CartItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }

        /// <summary>
        /// Central engine handles filtering and sorting concurrently without corrupting master data.
        /// </summary>
        private void RefreshProductGrid()
        {
            // Protective Guard: Prevent execution before UI elements are fully parsed & loaded
            if (myDataGrid == null || SearchTextBox == null || Sort == null || Products == null)
                return;

            IEnumerable<Product> displayedList = Products;

            // 1. EVALUATE SEARCH BAR FILTER
            string query = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(query))
            {
                displayedList = displayedList.Where(p =>
                    p.Id.ToString().Contains(query) ||
                    (p.Name != null && p.Name.ToLower().Contains(query)) ||
                    (p.Description != null && p.Description.ToLower().Contains(query))
                );
            }

            // 2. EVALUATE SORTING RULES
            if (Sort.SelectedItem is ComboBoxItem selectedSort)
            {
                string engineRule = selectedSort.Content.ToString();
                switch (engineRule)
                {
                    case "A-Z":
                        displayedList = displayedList.OrderBy(p => p.Name);
                        break;
                    case "Z-A":
                        displayedList = displayedList.OrderByDescending(p => p.Name);
                        break;
                    case "Price(Low - High)":
                        displayedList = displayedList.OrderBy(p => p.Price);
                        break;
                    case "Price(High - Low)":
                        displayedList = displayedList.OrderByDescending(p => p.Price);
                        break;
                }
            }

            // 3. APPLY TO DATAGRID VIEW
            myDataGrid.ItemsSource = displayedList.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshProductGrid();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshProductGrid();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NameTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(TitleTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(SalaryTextBox.Text))
                {
                    Products.Add(new Product
                    {
                        Id = int.Parse(IdTextBox.Text),
                        Name = NameTextBox.Text,
                        Description = TitleTextBox.Text,
                        Price = decimal.Parse(SalaryTextBox.Text)
                    });

                    RefreshProductGrid();
                    ClearBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Please fill in all fields!");
                }
            }
            catch
            {
                MessageBox.Show("Please check that data entries are formatted correctly!");
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Product selectedProduct)
            {
                Products.Remove(selectedProduct);
                RefreshProductGrid();
                MessageBox.Show("Product removed successfully!");
            }
            else
            {
                MessageBox.Show("Please select a product from the list to remove!");
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Product selectedProduct)
            {
                try
                {
                    selectedProduct.Id = int.Parse(IdTextBox.Text);
                    selectedProduct.Name = NameTextBox.Text;
                    selectedProduct.Description = TitleTextBox.Text;
                    selectedProduct.Price = decimal.Parse(SalaryTextBox.Text);

                    RefreshProductGrid();
                    MessageBox.Show("Product updated successfully!");
                }
                catch
                {
                    MessageBox.Show("Please ensure properties are updated with valid inputs!");
                }
            }
            else
            {
                MessageBox.Show("Select a row in the Available Products table to update.");
            }
        }

        private void AddtoCartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Product selectedProduct)
            {
                AddedCart.Add(new CartItem
                {
                    Id = selectedProduct.Id,
                    Name = selectedProduct.Name,
                    Description = selectedProduct.Description,
                    Price = selectedProduct.Price
                });

                myDataGrid2.Items.Refresh();
                MessageBox.Show("Item successfully stacked into cart!");
            }
            else
            {
                MessageBox.Show("Please select a product from the collection list first.");
            }
        }

        private void RemovefromCartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid2.SelectedItem is CartItem selectedItem)
            {
                AddedCart.Remove(selectedItem);
                myDataGrid2.Items.Refresh();
                MessageBox.Show("Item removed from cart!");
            }
            else
            {
                MessageBox.Show("Please select an item inside the Shopping Cart to eliminate.");
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            TitleTextBox.Text = "";
            SalaryTextBox.Text = "";
        }
    }
}