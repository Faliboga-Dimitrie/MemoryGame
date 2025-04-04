using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using MemoryGame.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MemoryGame.Views;

namespace MemoryGame.ViewModels
{
    public class GameEngineViewModel : BaseClass
    {

        private static GameEngineViewModel _instance;
        public static GameEngineViewModel Instance => _instance ??= new GameEngineViewModel();

        private GridLoaderViewMode _gridLoader;
        private GameEngine _gameEngine;
        private DialogService _dialogService;
        private TimeTracker _timeTracker;
        private User _currentUser;
        private static ObservableCollection<User> _users;

        private static List<string> _buttonContents;

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public TimeTracker TimeTracker
        {
            get => _timeTracker;
            set
            {
                _timeTracker = value;
                OnPropertyChanged();
            }
        }

        public GridLoaderViewMode GridLoader
        {
            get => _gridLoader;
            set
            {
                _gridLoader = value;
                OnPropertyChanged();
            }
        }

        public GameEngine GameEngine
        {
            get => _gameEngine;
            set
            {
                _gameEngine = value;
                OnPropertyChanged();
            }
        }

        private Visibility _gameStarted = Visibility.Collapsed;

        public Visibility GameStarted
        {
            get => _gameStarted;
            set
            {
                _gameStarted = value;
                OnPropertyChanged();
            }
        }

        private Visibility _gameSpecifier = Visibility.Visible;

        public Visibility GameSpecifier
        {
            get => _gameSpecifier;
            set
            {
                _gameSpecifier = value;
                OnPropertyChanged();
            }
        }

        private Visibility _isCustomGame = Visibility.Hidden;

        public Visibility IsCustomGame
        {
            get => _isCustomGame;
            set
            {
                _isCustomGame = value;
                OnPropertyChanged();
            }
        }

        string _buttonContent;
        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                _buttonContent = value;
                OnPropertyChanged();
            }
        }

        static GameEngineViewModel()
        {
            _buttonContents = new List<string> { "Select Standard", "Select Custom" };
        }

        public GameEngineViewModel()
        {
            _gridLoader = new GridLoaderViewMode();
            _gameEngine = new GameEngine();
            _dialogService = new DialogService();
            _timeTracker = new TimeTracker();
            _timeTracker.Timer.Tick += Timer_Tick;

            _buttonContent = _buttonContents[1];

            FlipCommand = new RelayCommand(FlipCell);
            CategoryCommand = new RelayCommand(SelectCategoryCommand);
            AboutCommand = new RelayCommand(AboutDisplay);
            NewGameCommand = new RelayCommand(StartGame, CanStartGame);
            SelectGameTypeCommand = new RelayCommand(SelectGameType);
            SaveCommand = new RelayCommand(SaveToJson);
            LoadCommand = new RelayCommand(LoadFromJson);
            DisplayStatisticsCommand = new RelayCommand(DisplayStatistics);
            ReturnToMainWindowCommand = new RelayCommand(ReturnToMainWindow);
        }

        public ICommand FlipCommand { get; }

        public ICommand CategoryCommand { get; }

        public ICommand AboutCommand { get; }

        public ICommand NewGameCommand { get; }

        public ICommand SelectGameTypeCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand DisplayStatisticsCommand { get; }

        public ICommand ReturnToMainWindowCommand { get; }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeTracker.RemainingTime = TimeTracker.RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            if (TimeTracker.RemainingTime <= TimeSpan.Zero)
                StopTimer();
        }

        private void StartTimer(object parameter)
        {
            TimeTracker.RemainingTime = TimeSpan.FromMinutes(TimeTracker.Minutes);
            TimeTracker.RemainingTime = TimeTracker.RemainingTime.Add(TimeSpan.FromSeconds(TimeTracker.Seconds));
            TimeTracker.Timer.Start();
            CommandManager.InvalidateRequerySuggested();
        }

        private void StopTimer()
        {
            TimeTracker.Timer.Stop();
            CommandManager.InvalidateRequerySuggested();
            _dialogService.ShowMessage("Time is up!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            GameSpecifier = Visibility.Visible;
            GameStarted = Visibility.Hidden;
            TimeTracker.Minutes = 0;
            TimeTracker.Seconds = 0;
            GridLoader.GenerateCells();
        }

        private async void FlipCell(object parameter)
        {
            if (parameter is GridCell cell)
            {
                if (GameEngine.FirstCell == null)
                {
                    GameEngine.FirstCell = cell;
                    cell.Flip();
                }
                else if (GameEngine.SecondCell == null)
                {
                    if (GameEngine.FirstCell == cell)
                    {
                        return;
                    }

                    cell.Flip();
                    GameEngine.SecondCell = cell;
                    if (GameEngine.FirstCell.BackImagePath == GameEngine.SecondCell.BackImagePath)
                    {
                        await Task.Delay(1000);
                        GameEngine.PairsFound++;
                        if(GameEngine.PairsFound == GridLoader.Rows * GridLoader.Columns / 2)
                        {
                            _dialogService.ShowMessage("Congratulations You Won!","Good boy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            ClearLastGame();
                            CurrentUser.GamesWon++;
                            CurrentUser.UpdateJson();
                        }
                        else
                        {
                            GameEngine.FirstCell.Visibility = Visibility.Hidden;
                            GameEngine.SecondCell.Visibility = Visibility.Hidden;
                        }
                            
                        GameEngine.FirstCell = null;
                        GameEngine.SecondCell = null;
                    }
                    else
                    {
                        await Task.Delay(1000);
                        GameEngine.FirstCell.Flip();
                        GameEngine.SecondCell.Flip();
                        GameEngine.FirstCell = null;
                        GameEngine.SecondCell = null;
                    }
                }
            }
        }

        private void SelectCategoryCommand(object parameter)
        {
            if (SelectedCategory != null)
            {
                if(SelectedCategory.Contains("Animals"))
                {
                    GridLoader.CategoryIndex = 0;
                }
                else if (SelectedCategory.Contains("Landmarks"))
                {
                    GridLoader.CategoryIndex = 1;
                }
                else if (SelectedCategory.Contains("Vehicles"))
                {
                    GridLoader.CategoryIndex = 2;
                }

                GridLoader.GenerateCells();
            }
        }

        private void AboutDisplay(object parameter)
        {
            _dialogService.ShowMessageWithLink("Faliboga Dimitrie 10LF331 <a href=\"https://www.unitbv.ro/\">University of Brasov</a>", "Developer Information");
        }

        private void StartGame(object parameter)
        {
            GameSpecifier = Visibility.Hidden;
            GameStarted = Visibility.Visible;
            GridLoader.GenerateCells();
            StartTimer(parameter);
            CurrentUser.TotalGamesPlayed++;
            CurrentUser.UpdateJson();
        }

        public void ClearLastGame()
        {
            GameSpecifier = Visibility.Visible;
            GameStarted = Visibility.Hidden;
            GridLoader.Rows = 4;
            GridLoader.Columns = 4;
            GridLoader.Cells.Clear();
            TimeTracker.Timer.Stop();
            TimeTracker.Minutes = 0;
            TimeTracker.Seconds = 0;
            TimeTracker.RemainingTime = TimeSpan.FromMinutes(0);
            TimeTracker.RemainingTime = TimeTracker.RemainingTime.Add(TimeSpan.FromSeconds(0));
        }

        private bool CanStartGame(object parameter)
        {
            return (TimeTracker.Minutes != 0 || TimeTracker.Seconds != 0) 
                && GridLoader.Rows * GridLoader.Columns % 2 == 0;
        }

        private void SelectGameType(object parameter)
        {
            if(ButtonContent == _buttonContents[0])
            {
                ButtonContent = _buttonContents[1];
                IsCustomGame = Visibility.Hidden;
            }
            else
            {
                ButtonContent = _buttonContents[0];
                IsCustomGame = Visibility.Visible;
            }
        }

        public async void DisplayStatistics(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                StatisticsWindow statisticsWindow = new StatisticsWindow(Users, CurrentUser);
                await WindowTransitionService.SlideSwitch(currentWindow, statisticsWindow, SlideDirection.Right, 500);
            }
        }

        public async void ReturnToMainWindow(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                ClearLastGame();
                await WindowTransitionService.FadeSwitch(currentWindow, new MainWindow(), 500);
            }
        }

        public void SaveToJson(object parameter)
        {
            string directoryPath = CurrentUser.UserFolder;

            GridLoader.SaveToJson(directoryPath);

            GameEngineData gameEngineData = new GameEngineData(GameEngine, SelectedCategory, GridLoader.Rows,
                GridLoader.Columns, TimeTracker.Minutes, TimeTracker.Seconds, TimeTracker.RemainingTime.Minutes, TimeTracker.RemainingTime.Seconds);

            string json = JsonSerializer.Serialize(gameEngineData, new JsonSerializerOptions { WriteIndented = true });

            string filePath = System.IO.Path.Combine(directoryPath, "GameEngineData.json");

            System.IO.File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(object parameter)
        {
            string directoryPath = CurrentUser.UserFolder;

            ObservableCollection<GridCell> cells = GridLoader.Cells;

            int index = 0;

            string filePath = System.IO.Path.Combine(directoryPath, "GameEngineData.json");

            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                GameEngineData gameEngineData = JsonSerializer.Deserialize<GameEngineData>(json);

                GameEngine = gameEngineData.GameEngine;
                TimeTracker.Minutes = gameEngineData.Minutes;
                TimeTracker.Seconds = gameEngineData.Seconds;
                TimeTracker.RemainingTime = TimeSpan.FromMinutes(gameEngineData.MinutesLeft);
                TimeTracker.RemainingTime = TimeTracker.RemainingTime.Add(TimeSpan.FromSeconds(gameEngineData.SecondsLeft));
                SelectedCategory = gameEngineData.SelectedCategory;
                GridLoader.Rows = gameEngineData.Rows;
                GridLoader.Columns = gameEngineData.Columns;
                GameSpecifier = Visibility.Hidden;
                GameStarted = Visibility.Visible;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    GridLoader.LoadFromJson(directoryPath);

                    TimeTracker.Timer.Start();

                });
            }
            else
            {
                _dialogService.ShowMessage("No saved game found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
