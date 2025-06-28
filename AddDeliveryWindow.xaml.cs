using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace FabricApp
{
    public partial class AddDeliveryWindow : Window
    {
        public AddDeliveryWindow()
        {
            InitializeComponent();
            LoadModels();
        }

        private void LoadModels()
        {
            ModelComboBox.ItemsSource = DB.ExecuteQuery("SELECT ModelID, Name FROM Models").DefaultView;
            ModelComboBox.DisplayMemberPath = "Name";
            ModelComboBox.SelectedValuePath = "ModelID";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ModelComboBox.SelectedValue == null || !int.TryParse(QuantityBox.Text, out int quantity) || string.IsNullOrWhiteSpace(ReceivedByBox.Text))
            {
                MessageBox.Show("Проверьте правильность всех данных.");
                return;
            }

            int modelId = Convert.ToInt32(ModelComboBox.SelectedValue);
            DateTime date = DeliveryDatePicker.SelectedDate ?? DateTime.Today;
            string receivedBy = ReceivedByBox.Text.Trim();

            string query = "INSERT INTO Deliveries (ModelID, DeliveryDate, Quantity, ReceivedBy) VALUES (@m, @d, @q, @r)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@m", modelId),
                new SqlParameter("@d", date),
                new SqlParameter("@q", quantity),
                new SqlParameter("@r", receivedBy)
            };
            DB.ExecuteQuery(query, parameters);
            this.Close();
        }
    }
}