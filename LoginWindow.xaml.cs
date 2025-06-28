using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;

namespace FabricApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            string query = "SELECT * FROM Users WHERE Login = @login AND Password = @password";
            SqlParameter[] parameters =
            {
                new SqlParameter("@login", login),
                new SqlParameter("@password", password)
            };

            DataTable user = DB.ExecuteQuery(query, parameters);

            if (user.Rows.Count == 1)
            {
                string role = user.Rows[0]["Role"].ToString();

                MainWindow mainWindow = new MainWindow(role);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                StatusBlock.Text = "Неверный логин или пароль.";
            }
        }
    }
}