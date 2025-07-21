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
using Microsoft.EntityFrameworkCore;

namespace TraficViolation
{
    /// <summary>
    /// Interaction logic for CitizenWindow.xaml
    /// </summary>
    public partial class CitizenWindow : Window
    {

        public CitizenWindow()
        {
            InitializeComponent();
        }

        public CitizenWindow(int userId, int citizenId, string userName)
        {
            InitializeComponent();
            
        }

    }
}