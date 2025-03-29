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

namespace MemoryGame.Views
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            PopulateStatistics();
        }

        private void PopulateStatistics()
        {
            // Example data
            var statistics = new List<UserStatistics>
            {
                new UserStatistics { UserName = "John Doe", GamesPlayed = 10, GamesWon = 5 },
                new UserStatistics { UserName = "Jane Doe", GamesPlayed = 8, GamesWon = 3 },
                // Add more users here...
            };

            lvStatistics.ItemsSource = statistics;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class UserStatistics
    {
        public string UserName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
    }
}
