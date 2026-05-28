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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Midterms_Cay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Products = new List<Product> { };
            AddedCart = new List<CartItem> { };
            this.DataContext = this;
        }
        public List<Product> Products { get; }
        public List<CartItem> AddedCart { get; }

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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NameTextBox.Text != "" && TitleTextBox.Text != "" && SalaryTextBox.Text != "")
                {
                    Products.Add(new Product
                    {
                        Id = int.Parse(IdTextBox.Text),
                        Name = NameTextBox.Text,
                        Description = TitleTextBox.Text,
                        Price = decimal.Parse(SalaryTextBox.Text)
                    });
                    myDataGrid.Items.Refresh();
                    IdTextBox.Text = "";
                    NameTextBox.Text = "";
                    TitleTextBox.Text = "";
                    SalaryTextBox.Text = "";

                }
                else
                {
                    MessageBox.Show("Please fill in all fields!");
                }
            }
            catch
            {
                MessageBox.Show("Please fill in all fields correctly!");
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = myDataGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                Products.Remove(selectedProduct);
                myDataGrid.Items.Refresh();
                MessageBox.Show("Product removed successfully!");
            }
            else
            {
                MessageBox.Show("Please select an product to remove!");
            }
        }
        private void RemovefromCartBtn_Click(object sender, RoutedEventArgs e)
        {
            CartItem selectedItem = myDataGrid2.SelectedItem as CartItem;
            if (selectedItem != null)
            {
                AddedCart.Remove(selectedItem);
                myDataGrid2.Items.Refresh();
                MessageBox.Show("Item removed from cart successfully!");
            }
            else
            {
                MessageBox.Show("Please select an item to remove from the cart!");
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
                    myDataGrid.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show("Please fill in all fields correctly!");
                }
            }
            else
            {
                MessageBox.Show("Please select a product to update!");
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            TitleTextBox.Text = "";
            SalaryTextBox.Text = "";
        }

        private void AddtoCartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Product selectedProduct)
            {
                CartItem newItem = new CartItem
                {
                    Id = selectedProduct.Id,
                    Name = selectedProduct.Name,
                    Description = selectedProduct.Description,
                    Price = selectedProduct.Price
                };

                AddedCart.Add(newItem);
                myDataGrid.Items.Refresh();
                myDataGrid2.Items.Refresh();

                MessageBox.Show("Item added to the Cart successfully!");
            }
            else
            {
                MessageBox.Show("Please select a product to add in the cart!");
            }
        }
    }
}
