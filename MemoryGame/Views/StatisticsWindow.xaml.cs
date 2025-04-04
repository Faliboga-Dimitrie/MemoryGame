using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Commands;
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
        public StatisticsWindow(ObservableCollection<User> _users, User currentUser)
        {
            InitializeComponent();
            PopulateStatistics(_users);
            UserStatistics.Users = _users;
            UserStatistics.CurrentUser = currentUser;
        }

        private void PopulateStatistics(ObservableCollection<User> _users)
        {
            lvStatistics.ItemsSource = _users.Select(user => new UserStatistics
            {
                UserName = user.Username,
                GamesPlayed = user.TotalGamesPlayed,
                GamesWon = user.GamesWon
            }).ToList();

            Test.DataContext = new UserStatistics
            {
                CloseCommand = new RelayCommand(UserStatistics.CloseWindow)
            };
        }
    }

    public class UserStatistics
    {
        public static ObservableCollection<User> Users { get; set; }

        public static User CurrentUser { get; set; }
        public string UserName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public ICommand CloseCommand { get; set; }

        public static async void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                await WindowTransitionService.SlideSwitch(window, new StartGameWindow(Users,CurrentUser), Enums.SlideDirection.Left, 500);
            }
        }
    }
}
