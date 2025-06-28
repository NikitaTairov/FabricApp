using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace FabricApp
{
    public partial class MainWindow : Window
    {
        private string _role;

        public MainWindow(string role)
        {
            InitializeComponent();
            _role = role;
            RestrictAccess();
            LoadAllData();
        }

        private void RestrictAccess()
        {
            if (_role == "Logistician")
            {
                ModelsGrid.IsEnabled = false;
                TypesGrid.IsEnabled = false;
            }
            else if (_role == "Warehouseman")
            {
                // Поступления доступны, модели и виды — ограничены
                ModelsGrid.IsEnabled = false;
                TypesGrid.IsEnabled = false;
            }
            else if (_role == "Administrator")
            {
                // Всё доступно
            }
        }

        private void LoadAllData()
        {
            LoadDeliveries();
            LoadModels();
            LoadTypes();
        }

        private void LoadDeliveries()
        {
            string query = @"
        SELECT d.DeliveryID, d.DeliveryDate, d.Quantity, d.ReceivedBy, m.Name AS ModelName
        FROM Deliveries d
        JOIN Models m ON d.ModelID = m.ModelID
        ORDER BY d.DeliveryDate DESC";

            DeliveriesGrid.ItemsSource = DB.ExecuteQuery(query).DefaultView;

           
        }


        private void LoadModels()
        {
            string query = @"
            SELECT m.ModelID, m.Name, pt.Name AS ProductTypeName, m.Price
            FROM Models m
            JOIN ProductTypes pt ON m.ProductTypeID = pt.ProductTypeID
            ORDER BY m.Name";

            ModelsGrid.ItemsSource = DB.ExecuteQuery(query).DefaultView;

            
        }

        private void LoadTypes()
        {
            string query = @"
        SELECT ProductTypeID, Name
        FROM ProductTypes
        ORDER BY Name";

            
            TypesGrid.ItemsSource = DB.ExecuteQuery(query).DefaultView;
        }

        private void RefreshDeliveries_Click(object sender, RoutedEventArgs e)
        {
            LoadDeliveries();
        }

        private void RefreshModels_Click(object sender, RoutedEventArgs e)
        {
            LoadModels();
        }

        private void RefreshTypes_Click(object sender, RoutedEventArgs e)
        {
            LoadTypes();
        }

        private void AddDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (_role == "Administrator" || _role == "Warehouseman")
            {
                AddDeliveryWindow window = new AddDeliveryWindow();
                window.ShowDialog();
                LoadDeliveries();
            }
            else
            {
                MessageBox.Show("У вас нет прав на добавление поступлений.", "Ограничение доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddModel_Click(object sender, RoutedEventArgs e)
        {
            if (_role == "Administrator")
            {
                AddModelWindow w = new AddModelWindow();
                w.ShowDialog();
                LoadModels();
            }
            else
            {
                MessageBox.Show("Только администратор может добавлять модели.", "Ограничение доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            if (_role == "Administrator")
            {
                AddTypeWindow w = new AddTypeWindow();
                w.ShowDialog();
                LoadTypes();
            }
            else
            {
                MessageBox.Show("Только администратор может добавлять виды товара.", "Ограничение доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteSelectedRecord(DataGrid grid, string table, string idField)
        {
            if (_role != "Administrator")
            {
                MessageBox.Show("Удаление доступно только администратору.", "Отказ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (grid.SelectedItem is DataRowView row)
            {
                var result = MessageBox.Show("Вы действительно хотите удалить выбранную запись?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int id = Convert.ToInt32(row[idField]);
                    string query = $"DELETE FROM {table} WHERE {idField} = @id";
                    DB.ExecuteQuery(query, new SqlParameter("@id", id));
                    LoadAllData();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void DeleteDelivery_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRecord(DeliveriesGrid, "Deliveries", "DeliveryID");
        }

        private void DeleteModel_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRecord(ModelsGrid, "Models", "ModelID");
        }

        private void DeleteType_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRecord(TypesGrid, "ProductTypes", "ProductTypeID");
        }
    }
}