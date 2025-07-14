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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using TraficViolation.Models;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for NewReportWindow.xaml
    /// </summary>
    public partial class NewReportWindow : Window
    {
        private TrafficViolationDbContext _context;
        private int _currentUserId;

        public NewReportWindow()
        {
            InitializeComponent();
        }

        public NewReportWindow(int currentUserId, int currentCitizenId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
            _context = new TrafficViolationDbContext();
            LoadViolationTypes();
            SetDefaultValues();
        }

        private void LoadViolationTypes()
        {
            try
            {
                var violationTypes = _context.ViolationTypes.ToList();
                cbViolationType.ItemsSource = violationTypes;
                cbViolationType.DisplayMemberPath = "Name";
                cbViolationType.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading violation types: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetDefaultValues()
        {
            dpViolationDate.SelectedDate = DateTime.Now;
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                btnSubmit.IsEnabled = false;

                // Lấy user hiện tại
                var currentUser = await _context.Users
                    .Include(u => u.Citizen)
                    .FirstOrDefaultAsync(u => u.Id == _currentUserId);

                if (currentUser?.CitizenId == null)
                {
                    MessageBox.Show("Unable to identify current user. Please login again.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lấy status mặc định (Open)
                var openStatus = await _context.ReportStatuses
                    .FirstOrDefaultAsync(s => s.Status == "Mở");

                if (openStatus == null)
                {
                    MessageBox.Show("Default status not found.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tạo báo cáo
                var description = BuildDescription();

                var violationReport = new ViolationReport
                {
                    CitizenId = currentUser.CitizenId.Value,
                    ViolationTypeId = (int)cbViolationType.SelectedValue,
                    StatusId = openStatus.Id,
                    ReportDate = dpViolationDate.SelectedDate?.Date ?? DateTime.Now,
                    Location = txtLocation.Text.Trim(),
                    Description = description
                };

                _context.ViolationReports.Add(violationReport);
                await _context.SaveChangesAsync();

                MessageBox.Show("Report submitted successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting report: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnSubmit.IsEnabled = true;
            }
        }

        private bool ValidateInput()
        {
            if (cbViolationType.SelectedItem == null)
            {
                MessageBox.Show("Please select a violation type.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPlateNumber.Text))
            {
                MessageBox.Show("Please enter a plate number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpViolationDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a violation date.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dpViolationDate.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Violation date cannot be in the future.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter a location.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private string BuildDescription()
        {
            var description = new StringBuilder();

            // Thêm biển số xe vào description
            description.AppendLine($"License Plate: {txtPlateNumber.Text.Trim()}");

            // Thêm description từ user
            if (!string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                description.AppendLine($"Description: {txtDescription.Text.Trim()}");
            }

            // Thêm image URL nếu có
            if (!string.IsNullOrWhiteSpace(txtImageUrl.Text))
            {
                description.AppendLine($"Image URL: {txtImageUrl.Text.Trim()}");
            }

            return description.ToString().Trim();
        }

        private void ClearForm()
        {
            cbViolationType.SelectedItem = null;
            txtPlateNumber.Clear();
            dpViolationDate.SelectedDate = DateTime.Now;
            txtLocation.Clear();
            txtImageUrl.Clear();
            txtDescription.Clear();
        }
    }
}