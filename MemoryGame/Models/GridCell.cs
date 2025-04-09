using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public class GridCell : BaseClass
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }

        private static readonly string _frontImagePath = "Data/Images/GameImages/Base.jpg";

        private static readonly Dictionary<string, BitmapImage> ImageCache = [];
        public string FrontImagePath
        {
            get => _frontImagePath;
        }

        private string _backImagePath;
        public string BackImagePath
        {
            get => _backImagePath;
            set
            {
                _backImagePath = value;
                OnPropertyChanged();
            }
        }

        private bool _isFlipped;
        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged();
            }
        }

        private string _currentImagePath;
        public string CurrentImagePath
        {
            get => _currentImagePath;
            set
            {
                _currentImagePath = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        [JsonIgnore]
        public ImageSource CurrentImage
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentImagePath))
                    return null;

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CurrentImagePath);

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

        private Visibility _visibility;

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }

        public GridCell() { }

        public GridCell(int row, int col,string backImagePath)
        {
            RowIndex = row;
            ColumnIndex = col;
            BackImagePath = backImagePath;
            IsFlipped = false;
            Visibility = Visibility.Visible;
            CurrentImagePath = FrontImagePath;
        }
    }

}
