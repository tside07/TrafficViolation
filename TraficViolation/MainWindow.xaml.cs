using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = ConfigHelper.GetConnectionString();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query để lấy thông tin user và role name
                    string query = @"
                        SELECT u.id, u.role_id, ur.role_name, u.citizen_id
                        FROM users u
                        INNER JOIN user_roles ur ON u.role_id = ur.id
                        WHERE u.username = @username AND u.password_hash = @password";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long userIdLong = reader.GetInt64(0);     // user_id là BIGINT
                            long roleIdLong = reader.GetInt64(1);     // role_id là BIGINT
                            string roleName = reader.GetString(2);    // role_name
                            object citizenIdObj = reader.GetValue(3); // citizen_id có thể null

                            int userId = (int)userIdLong;
                            int roleId = (int)roleIdLong;
                            int? citizenId = citizenIdObj == DBNull.Value ? null : (int?)Convert.ToInt64(citizenIdObj);

                            // Điều hướng dựa trên role_id
                            switch (roleId)
                            {
                                case 1: // Quản trị viên (Admin)
                                    var adminWindow = new AdminWindow();
                                    adminWindow.Show();
                                    this.Close();
                                    break;

                                case 2: // Moderator
                                    var moderatorWindow = new AdminWindow(); // Hoặc tạo ModeratorWindow riêng
                                    moderatorWindow.Show();
                                    this.Close();
                                    break;

                                case 3: // Người dùng (Citizen)
                                    var citizenWindow = new CitizenWindow();
                                    citizenWindow.Show();
                                    this.Close();
                                    break;

                                default:
                                    MessageBox.Show($"Unknown role: {roleName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                            }

                            // Log thông tin đăng nhập (tùy chọn)
                            Console.WriteLine($"User {username} logged in with role: {roleName} (ID: {roleId})");
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}