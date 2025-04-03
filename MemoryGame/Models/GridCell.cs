using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public class GridCell : BaseClass
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }

        private string _frontImagePath = "/Data/Images/GameImages/Base.jpg";
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

        private string _currentImage;
        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged();
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
            UpdateCurrentImage();
        }

        private void UpdateCurrentImage()
        {
            if (IsFlipped)
            {
                CurrentImage = BackImagePath;
            }
            else
            {
                CurrentImage = FrontImagePath;
            }
        }

        public void Flip()
        {
            IsFlipped = !IsFlipped;
            UpdateCurrentImage();
        }
    }

}
