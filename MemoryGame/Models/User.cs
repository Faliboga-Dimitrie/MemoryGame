using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Models
{
    public class User : BaseClass
    {
        private string _username;
        private string _password;
        private string _profilePicturePath;
        private string _userFolder;
        private int _gamesWon;
        private int _totalGamesPlayed;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string GetSafeUsername() =>
       string.Join("_", Username.Split(Path.GetInvalidFileNameChars()));

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
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

        public int GamesWon
        {
            get => _gamesWon;
            set
            {
                _gamesWon = value;
                OnPropertyChanged();
            }
        }

        public int TotalGamesPlayed
        {
            get => _totalGamesPlayed;
            set
            {
                _totalGamesPlayed = value;
                OnPropertyChanged();
            }
        }

        public string UserFolder
        {
            get => _userFolder;
            set
            {
                _userFolder = value;
            }
        }

        public User()
        {
            Username = "";
            Password = "";
            ProfilePicturePath = "/Data/Images/UserAvatarImages/Avatar1.jpg";
        }
    }
}
