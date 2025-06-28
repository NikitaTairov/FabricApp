using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace FabricApp
{
    public partial class AddModelWindow : Window
    {
        public AddModelWindow()
        {
            InitializeComponent();
            LoadProductTypes();
        }

        private void LoadProductTypes()
        {
            TypeComboBox.ItemsSource = DB.ExecuteQuery("SELECT ProductTypeID, Name FROM ProductTypes").DefaultView;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = ModelNameBox.Text.Trim();
            string priceText = PriceBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || TypeComboBox.SelectedValue == null || !float.TryParse(priceText, out float price))
            {
                MessageBox.Show("Проверьте корректность всех полей.");
                return;
            }

            int typeId = Convert.ToInt32(TypeComboBox.SelectedValue);

            string query = "INSERT INTO Models (Name, ProductTypeID, Price) VALUES (@name, @type, @price)";
            SqlParameter[] parameters = {
                new SqlParameter("@name", name),
                new SqlParameter("@type", typeId),
                new SqlParameter("@price", price)
            };
            DB.ExecuteQuery(query, parameters);
            this.Close();
        }
    }
}