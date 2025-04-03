using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public StatisticsWindow(ObservableCollection<User> _users)
        {
            InitializeComponent();
            PopulateStatistics(_users);
        }

        private void PopulateStatistics(ObservableCollection<User> _users)
        {
            lvStatistics.ItemsSource = _users.Select(user => new UserStatistics
            {
                UserName = user.Username,
                GamesPlayed = user.TotalGamesPlayed,
                GamesWon = user.GamesWon
            }).ToList();
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
