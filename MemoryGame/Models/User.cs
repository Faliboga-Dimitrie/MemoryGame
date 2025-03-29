using System;
using System.Collections.Generic;
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
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
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

        public User()
        {
            Username = "";
            Password = "";
            ProfilePicturePath = "/Data/Images/UserAvatarImages/Avatar1.jpg";
        }
    }
}
