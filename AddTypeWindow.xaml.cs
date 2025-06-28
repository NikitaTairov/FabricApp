using System;
using System.Data.SqlClient;
using System.Windows;

namespace FabricApp
{
    public partial class AddTypeWindow : Window
    {
        public AddTypeWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = TypeNameBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите наименование.");
                return;
            }

            string query = "INSERT INTO ProductTypes (Name) VALUES (@name)";
            SqlParameter[] parameters = {
                new SqlParameter("@name", name)
            };
            DB.ExecuteQuery(query, parameters);
            this.Close();
        }
    }
}