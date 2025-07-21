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
using TraficViolation.Models;
using System.Linq;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TrafficViolationDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new TrafficViolationDbContext();
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
                var user = _context.Users
                    .Where(u => u.Username == username && u.PasswordHash == password)
                    .Select(u => new
                    {
                        Id = u.Id,
                        RoleId = u.RoleId,
                        RoleName = u.Role.RoleName,
                        CitizenId = u.CitizenId
                    })
                    .FirstOrDefault();

                if (user != null)
                {
                    int userId = (int)user.Id;
                    int roleId = (int)user.RoleId;
                    string roleName = user.RoleName;
                    int? citizenId = user.CitizenId;

                    // Điều hướng dựa trên role_id
                    switch (roleId)
                    {
                        case 1:
                            var adminWindow = new AdminWindow();
                            adminWindow.Show();
                            this.Close();
                            break;

                        case 2:
                            var moderatorWindow = new AdminWindow(); 
                            moderatorWindow.Show();
                            this.Close();
                            break;

                        case 3:
                            var citizenWindow = new CitizenWindow();
                            citizenWindow.Show();
                            this.Close();
                            break;

                        default:
                            MessageBox.Show($"Lỗi role không xác định: {roleName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không hợp lệ", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

        //tránh tràn bộ nhớ
        protected override void OnClosed(EventArgs e)
        {
            _context?.Dispose();
            base.OnClosed(e);
        }
    }
}