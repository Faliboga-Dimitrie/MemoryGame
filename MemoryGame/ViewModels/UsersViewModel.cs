﻿using MemoryGame.Commands;
using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using MemoryGame.Views;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Text.Json.Serialization;
using System.Windows.Media.Animation;
using MemoryGame.Services;
using MemoryGame.Enums;

namespace MemoryGame.ViewModels
{
    public class UsersViewModel : BaseClass
    {
        private static UsersViewModel _instance;

        public static UsersViewModel Instance => _instance ??= new UsersViewModel();

        private ObservableCollection<User> _users;
        private User _selectedUser;
        private User _newUser;
        private string _profilePicturePath = "Data/Images/UserAvatarImages/Avatar1.jpg";
        private static int _id;
        private static readonly Dictionary<string, BitmapImage> ImageCache = [];
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set
            {
                _profilePicturePath = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        [JsonIgnore]
        public ImageSource ProfilePicture
        {
            get
            {
                if (string.IsNullOrEmpty(ProfilePicturePath))
                    return null;

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProfilePicturePath);

                if (!File.Exists(fullPath))
                    return null;

                if (ImageCache.TryGetValue(fullPath, out var cachedImage))
                {
                    return cachedImage;
                }

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                ImageCache[fullPath] = bitmapImage;

                return bitmapImage;
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ChangeProfilePicturePathForwardCommand { get; }

        public ICommand ChangeProfilePicturePathBackwardCommand { get; }

        public ICommand StartGameCommand { get;}

        public ICommand UserRegisterCommand { get; }

        public ICommand ReturnMainWindowCommand { get; }

        static UsersViewModel()
        {
            _id = 0;
        }

        public UsersViewModel()
        {
            _users = [];
            LoadUsersJSON();
            _selectedUser = null;
            _newUser = new User();
            AddUserCommand = new RelayCommand(AddUser);
            ChangeProfilePicturePathForwardCommand = new RelayCommand(ChangeProfilePicturePathForward);
            ChangeProfilePicturePathBackwardCommand = new RelayCommand(ChangeProfilePicturePathBackward);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            StartGameCommand = new RelayCommand(StartGame, CanStartGame);
            UserRegisterCommand = new RelayCommand(UserRegister);
            ReturnMainWindowCommand = new RelayCommand(ReturnMainWindow);
        }

        private void AddUser(object parameter)
        {
            if (NewUser.Username == "" || NewUser.Password == "")
            {
                DialogService.ShowMessage("Please fill in all the fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Users.Any(u => u.Username == NewUser.Username))
            {
                DialogService.ShowMessage("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewUser.ProfilePicturePath = ProfilePicturePath;
            Users.Add(NewUser);
            NewUser = new User();

            DialogService.ShowMessage("User added successfully!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            SaveUsersJSON();
        }

        private void DeleteUser(object parameter)
        {
            DeleteUserFromJSON(SelectedUser.Username);
            Users.Remove(SelectedUser);
            DialogService.ShowMessage("User deleted successfully!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            SelectedUser = null;
        }

        private bool CanDeleteUser(object parameter)
        {
            return SelectedUser != null;
        }

        private async void StartGame(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                await WindowTransitionService.FadeSwitch(currentWindow, new StartGameWindow(this.Users, this.SelectedUser), 500);
            }
        }

        private async void UserRegister(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                await WindowTransitionService.SlideSwitch(currentWindow, new RegisterWindow(),SlideDirection.Left);
            }
        }

        private async void ReturnMainWindow(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                await WindowTransitionService.SlideSwitch(currentWindow, new MainWindow(), SlideDirection.Right);
            }
        }


        private bool CanStartGame(object parameter)
        {
            return SelectedUser != null;
        }

        private void ChangeProfilePicturePathForward(object parameter)
        {
            _id++;
            int i = _id % 6 + 1;
            ProfilePicturePath = "Data/Images/UserAvatarImages/Avatar" + i + ".jpg";
        }

        private void ChangeProfilePicturePathBackward(object parameter)
        {
            _id = (_id + 5) % 6;
            int i = _id % 6 + 1;
            ProfilePicturePath = "Data/Images/UserAvatarImages/Avatar" + i + ".jpg";
        }
        private void SaveUsersJSON()
        {
            try
            {
                string basePath = Path.Combine(
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                        .Parent.Parent.FullName,
                    "Data",
                    "UserData"
                );

                // Create root directory if missing
                Directory.CreateDirectory(basePath);

                foreach (var user in _users)
                {
                    var userFolder = Path.Combine(basePath, user.GetSafeUsername());
                    user.UserFolder = userFolder;
                    var filePath = Path.Combine(userFolder, "userdata.json");

                    Directory.CreateDirectory(userFolder);

                    string json = JsonSerializer.Serialize(user, _jsonOptions);
                    File.WriteAllText(filePath, json);
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage($"Save failed: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void LoadUsersJSON()
        {
            try
            {
                string basePath = Path.Combine(
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                        .Parent.Parent.FullName,
                    "Data",
                    "UserData"
                );



                if (Directory.Exists(basePath))
                {
                    foreach (var userFolder in Directory.EnumerateDirectories(basePath))
                    {
                        var filePath = Path.Combine(userFolder, "userdata.json");
                        if (File.Exists(filePath))
                        {
                            string json = File.ReadAllText(filePath);
                            var user = JsonSerializer.Deserialize<User>(json);
                            _users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage($"Load failed: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUserFromJSON(string username)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Username == username);
                if (user == null) return;

                string basePath = Path.Combine(
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                        .Parent.Parent.FullName,
                    "Data",
                    "UserData"
                );

                var userFolder = Path.Combine(basePath, user.GetSafeUsername());

                if (Directory.Exists(userFolder))
                {
                    Directory.Delete(userFolder, true);
                    _users.Remove(user);
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowMessage($"Delete failed: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void UpdateUserJson(string UserFolder, int GamesWon, int TotalGamesPlayed)
        {
            string filePath = Path.Combine(UserFolder, "userdata.json");

            string json = File.ReadAllText(filePath);

            User? user = JsonSerializer.Deserialize<User>(json);

            user.GamesWon = GamesWon;
            user.TotalGamesPlayed = TotalGamesPlayed;

            string updatedJson = JsonSerializer.Serialize(user, _jsonOptions);

            File.WriteAllText(filePath, updatedJson);
        }
    }
}
