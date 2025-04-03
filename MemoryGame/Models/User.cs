using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Text.Json.Serialization;

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

        private static readonly Dictionary<string, BitmapImage> ImageCache = new();
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
                OnPropertyChanged(nameof(ProfilePicturePath));
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

        public void UpdateJson()
        {
            string filePath = Path.Combine(UserFolder, "userdata.json");

            string json = File.ReadAllText(filePath);

            User? user = JsonSerializer.Deserialize<User>(json);

            user.GamesWon = GamesWon;
            user.TotalGamesPlayed = TotalGamesPlayed;

            string updatedJson = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, updatedJson);
        }


        public User()
        {
            Username = "";
            Password = "";
            ProfilePicturePath = "Data/Images/UserAvatarImages/Avatar1.jpg";
        }
    }
}
