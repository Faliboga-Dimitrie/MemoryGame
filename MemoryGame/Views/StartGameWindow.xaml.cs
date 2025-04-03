using MemoryGame.Models;
using MemoryGame.ViewModels;
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
    /// Interaction logic for StartGameWindow.xaml
    /// </summary>
    public partial class StartGameWindow : Window
    {
        public ObservableCollection<User> _users;

        public StartGameWindow()
        {
            InitializeComponent();
        }

        public StartGameWindow(ObservableCollection<User> users, User currentUser)
        {
            InitializeComponent();
            _users = users;
            GameEngineViewModel.Instance.CurrentUser = currentUser;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            GameEngineViewModel.Instance.ClearLastGame();
            mainWindow.Show();
            this.Close();
        }

        private void StatisticsDisplay_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow(_users);
            statisticsWindow.Show();
        }
    }
}
