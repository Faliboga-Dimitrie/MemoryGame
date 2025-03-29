using MemoryGame.Commands;
using MemoryGame.Interfaces;
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

namespace MemoryGame.ViewModels
{
    public class UsersViewModel : BaseClass
    {
        private static UsersViewModel _instance;

        public static UsersViewModel Instance => _instance ??= new UsersViewModel();

        private ObservableCollection<User> _users;
        private User _selectedUser;
        private User _newUser;
        private readonly IDialogService _dialogService;
        private string _profilePicturePath = "/Data/Images/UserAvatarImages/Avatar1.jpg";
        private static int _id;

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
                OnPropertyChanged();
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ChangeProfilePicturePathForwardCommand { get; }

        public ICommand ChangeProfilePicturePathBackwardCommand { get; }

        public ICommand StartGameCommand { get;}

        static UsersViewModel()
        {
            _id = 0;
        }

        public UsersViewModel()
        {
            _dialogService = new DialogService();
            LoadUsersJSON();
            _selectedUser = null;
            _newUser = new User();
            AddUserCommand = new RelayCommand(AddUser);
            ChangeProfilePicturePathForwardCommand = new RelayCommand(ChangeProfilePicturePathForward);
            ChangeProfilePicturePathBackwardCommand = new RelayCommand(ChangeProfilePicturePathBackward);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            StartGameCommand = new RelayCommand(StartGame, CanStartGame);
        }

        private void AddUser(object parameter)
        {
            if (NewUser.Username == "" || NewUser.Password == "")
            {
                _dialogService.ShowMessage("Please fill in all the fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Users.Any(u => u.Username == NewUser.Username))
            {
                _dialogService.ShowMessage("Username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewUser.ProfilePicturePath = ProfilePicturePath;
            Users.Add(NewUser);
            NewUser = new User();

            _dialogService.ShowMessage("User added successfully!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            SaveUsersJSON();
        }

        private void DeleteUser(object parameter)
        {
            DeleteUserFromJSON(SelectedUser.Username);
            Users.Remove(SelectedUser);
            _dialogService.ShowMessage("User deleted successfully!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            SelectedUser = null;
        }

        private bool CanDeleteUser(object parameter)
        {
            return SelectedUser != null;
        }

        private void StartGame(object parameter)
        {
            StartGameWindow startGameWindow = new StartGameWindow();
            startGameWindow.Show();
            Application.Current.MainWindow.Close(); // Assuming this is the main window
        }

        private bool CanStartGame(object parameter)
        {
            return SelectedUser != null;
        }

        private void ChangeProfilePicturePathForward(object parameter)
        {
            _id++;
            int i = _id % 6 + 1;
            ProfilePicturePath = "/Data/Images/UserAvatarImages/Avatar" + i + ".jpg";
        }

        private void ChangeProfilePicturePathBackward(object parameter)
        {
            _id = (_id + 5) % 6;
            int i = _id % 6 + 1;
            ProfilePicturePath = "/Data/Images/UserAvatarImages/Avatar" + i + ".jpg";
        }

        private void SaveUsersJSON()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_users, options);

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Data", "UserData", "users.json");

                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("An error occurred while saving users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUsersJSON()
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Data", "UserData", "users.json");

                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    // Deserialize into List<User> and convert to ObservableCollection<User>
                    List<User> usersList = JsonSerializer.Deserialize<List<User>>(jsonString);
                    _users = new ObservableCollection<User>(usersList);
                }
                else
                {
                    _users = new ObservableCollection<User>();
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("An error occurred while loading users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _users = new ObservableCollection<User>();
            }
        }

        private void DeleteUserFromJSON(string username)
        {
            try
            {
                // Find and remove the user from the existing collection
                User userToRemove = _users.FirstOrDefault(u => u.Username == username);
                if (userToRemove != null)
                {
                    _users.Remove(userToRemove);
                }
                else
                {
                    _dialogService.ShowMessage("User not found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Save the updated collection
                SaveUsersJSON();
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage("An error occurred while deleting the user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
