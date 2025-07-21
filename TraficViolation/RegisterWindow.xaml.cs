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
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using TraficViolation.Models;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private TrafficViolationDbContext _context;

        public RegisterWindow()
        {
            InitializeComponent();
            _context = new TrafficViolationDbContext();
        }

        private void btnRegisterSuccess_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsEmailExists(email))
                {
                    MessageBox.Show("Email already in use. Please use another email.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Thực hiện đăng ký citizen
                if (RegisterUser(username, password, name, email, phone, address))
                {
                    MessageBox.Show("Registration successful!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Đóng cửa sổ đăng ký và quay về màn hình đăng nhập
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            var regex = new Regex(@"^0\d{9}$");
            return regex.IsMatch(phone);
        }

        private bool IsUsernameExists(string username)
        {
            // Username check không cần thiết cho Citizen vì không có Username field
            // Có thể bỏ qua hoặc check theo cách khác
            return false;
        }

        private bool IsEmailExists(string email)
        {
            try
            {
                return _context.Citizens.Any(c => c.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking email: {ex.Message}");
            }
        }

        private bool RegisterUser(string username, string password, string name, string email, string phone, string address)
        {
            try
            {
                var newCitizen = new Citizen
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Address = address
                };

                _context.Citizens.Add(newCitizen);
                int result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registering citizen: {ex.Message}");
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnRegisterSuccess_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}