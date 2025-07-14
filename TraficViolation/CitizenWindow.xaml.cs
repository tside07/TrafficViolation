using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Data.SqlClient;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for CitizenWindow.xaml
    /// </summary>
    public partial class CitizenWindow : Window
    {
        private string connectionString = ConfigHelper.GetConnectionString();
        private int currentUserId;
        private int currentCitizenId;
        private string currentUserName;

        public CitizenWindow()
        {
            InitializeComponent();
            LoadUserInfo();
            LoadDashboardData();
            SetupEventHandlers();
        }

        // Constructor với tham số user info
        public CitizenWindow(int userId, int citizenId, string userName) : this()
        {
            currentUserId = userId;
            currentCitizenId = citizenId;
            currentUserName = userName;
            LoadUserInfo();
            LoadDashboardData();
        }

        private void SetupEventHandlers()
        {
            // Menu button events
            btnDashboard.Click += BtnDashboard_Click;
            btnNewReport.Click += BtnNewReport_Click;
            btnTrackReports.Click += BtnTrackReports_Click;
            btnMyVehicles.Click += BtnMyVehicles_Click;
            btnNotifications.Click += BtnNotifications_Click;
            btnLogout.Click += BtnLogout_Click;
        }

        private void LoadUserInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT c.name, c.email 
                        FROM citizens c
                        INNER JOIN users u ON c.id = u.citizen_id
                        WHERE u.id = @userId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", currentUserId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentUserName = reader["name"].ToString();
                            // Có thể cập nhật UI hiển thị tên user nếu cần
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user info: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load statistics
                    LoadReportStatistics(conn);

                    // Load recent reports
                    LoadRecentReports(conn);

                    // Load notifications
                    LoadNotifications(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadReportStatistics(SqlConnection conn)
        {
            try
            {
                // Get total reports count
                string totalQuery = @"
                    SELECT COUNT(*) FROM violation_reports 
                    WHERE citizen_id = @citizenId";

                SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                totalCmd.Parameters.AddWithValue("@citizenId", currentCitizenId);
                int totalReports = Convert.ToInt32(totalCmd.ExecuteScalar());

                // Get processing reports count
                string processingQuery = @"
                    SELECT COUNT(*) FROM violation_reports vr
                    INNER JOIN report_statuses rs ON vr.status_id = rs.id
                    WHERE vr.citizen_id = @citizenId AND rs.status = N'Đang xử lý'";

                SqlCommand processingCmd = new SqlCommand(processingQuery, conn);
                processingCmd.Parameters.AddWithValue("@citizenId", currentCitizenId);
                int processingReports = Convert.ToInt32(processingCmd.ExecuteScalar());

                // Get closed reports count
                string closedQuery = @"
                    SELECT COUNT(*) FROM violation_reports vr
                    INNER JOIN report_statuses rs ON vr.status_id = rs.id
                    WHERE vr.citizen_id = @citizenId AND rs.status = N'Đóng'";

                SqlCommand closedCmd = new SqlCommand(closedQuery, conn);
                closedCmd.Parameters.AddWithValue("@citizenId", currentCitizenId);
                int closedReports = Convert.ToInt32(closedCmd.ExecuteScalar());

                // Update UI with statistics (you'll need to add x:Name to TextBlocks in XAML)
                // For now, we'll just store the values
                // You can add named TextBlocks in XAML and update them here

                Console.WriteLine($"Total Reports: {totalReports}, Processing: {processingReports}, Closed: {closedReports}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading statistics: {ex.Message}");
            }
        }

        private void LoadRecentReports(SqlConnection conn)
        {
            try
            {
                string query = @"
                    SELECT TOP 5 
                        vr.id,
                        vr.description,
                        vr.report_date,
                        rs.status,
                        vr.location
                    FROM violation_reports vr
                    INNER JOIN report_statuses rs ON vr.status_id = rs.id
                    WHERE vr.citizen_id = @citizenId
                    ORDER BY vr.report_date DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@citizenId", currentCitizenId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Store recent reports data
                    // You can create a data structure to hold this info
                    // and bind it to a DataGrid or update the UI manually

                    while (reader.Read())
                    {
                        string reportId = reader["id"].ToString();
                        string description = reader["description"].ToString();
                        DateTime reportDate = Convert.ToDateTime(reader["report_date"]);
                        string status = reader["status"].ToString();
                        string location = reader["location"].ToString();

                        Console.WriteLine($"Report {reportId}: {description} - {status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recent reports: {ex.Message}");
            }
        }

        private void LoadNotifications(SqlConnection conn)
        {
            // For now, we'll show static notifications
            // In a real app, you might have a notifications table
            Console.WriteLine("Loading notifications...");
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {

            // Reload dashboard data
            LoadDashboardData();
        }

        private void BtnNewReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open new report window
                var newReportWindow = new NewReportWindow(currentUserId, currentCitizenId);
                newReportWindow.ShowDialog();

                // Refresh dashboard after closing new report window
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening new report window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnTrackReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open track reports window
                var trackReportsWindow = new TrackReportsWindow(currentUserId, currentCitizenId);
                trackReportsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening track reports window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMyVehicles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open my vehicles window
                var myVehiclesWindow = new MyVehiclesWindow(currentUserId, currentCitizenId);
                myVehiclesWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening my vehicles window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open notifications window
                var notificationsWindow = new NotificationsWindow(currentUserId, currentCitizenId);
                notificationsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening notifications window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận đăng xuất",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Clear user session data
                currentUserId = 0;
                currentCitizenId = 0;
                currentUserName = string.Empty;

                // Open login window
                var loginWindow = new MainWindow();
                loginWindow.Show();

                // Close current window
                this.Close();
            }
        }


        private void RefreshDashboard()
        {
            LoadDashboardData();
        }

        // Event handler for the existing Button_Click method
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // This can be used for any generic button click events
            // or removed if not needed
        }

        // Window closing event
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Clean up resources if needed
            base.OnClosing(e);
        }


    }
}