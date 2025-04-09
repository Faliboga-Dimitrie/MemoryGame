using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModels
{
    public class GridLoaderViewMode : BaseClass
    {
        private static GridLoaderViewMode _instance;
        public static GridLoaderViewMode Instance => _instance ??= new GridLoaderViewMode();


        private static readonly List<string> _category;
        private static readonly List<int> _ints;
        private static int _categoryIndex;
        private static readonly int _numberOfImages;
        private int _rows = 4;
        private int _columns = 4;


        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged();
                GenerateCells();
            }
        }

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                OnPropertyChanged();
                GenerateCells();
            }
        }

        public List<string> Category
        {
            get => _category;
        }

        public int CategoryIndex
        {
            get => _categoryIndex;
            set
            {
                _categoryIndex = value;
                OnPropertyChanged();
                GenerateCells();
            }
        }

        public int NumberOfImages
        {
            get => _numberOfImages;
        }

        private ObservableCollection<GridCell> _cells = [];
        public ObservableCollection<GridCell> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged();
            }
        }

        public void GenerateCells()
        {
            Cells.Clear();
            int[,] grid = GenerateMemoryGrid();
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Cells.Add(new GridCell(row, col, Category[CategoryIndex] + Convert.ToString(grid[row, col]) + ".jpg"));
                }
            }
        }

        public void SaveToJson(string directoryPath)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(Cells);
            string filePath = System.IO.Path.Combine(directoryPath, "GridData.json");
            System.IO.File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string directoryPath)
        {
            string filePath = System.IO.Path.Combine(directoryPath, "GridData.json");
            string json = System.IO.File.ReadAllText(filePath);
            var loadedCells = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<GridCell>>(json);

            if (loadedCells != null)
            {
                Cells = loadedCells;
            }
        }

        static GridLoaderViewMode()
        {
            _category =
            [
                "Data/Images/GameImages/Animals/Animal",
                "Data/Images/GameImages/Landmarks/landmark",
                "Data/Images/GameImages/Vehicles/Vehicle"
            ];
            _ints = [ 1, 2, 3, 4, 5, 6, 7, 8 ];
            _categoryIndex = 0;
            _numberOfImages = 8;
        }

        public GridLoaderViewMode()
        {
            GenerateCells();
        }

        public void ClearCells()
        {
            Cells.Clear();
        }

        private int[,] GenerateMemoryGrid()
        {
            int totalSlots = Rows * Columns;

            int numImages = _ints.Count;
            List<int> imagePool = [];

            while(true)
            {
                if ( totalSlots / numImages != Convert.ToInt32(totalSlots/ numImages))
                {
                    numImages--;
                }
                else
                {
                    break;
                }
            }

            int basePairCount = totalSlots / numImages;
            if (basePairCount % 2 != 0)
                basePairCount -= 1; 

            foreach (var image in _ints)
            {
                for (int i = 0; i < basePairCount; i++)
                {
                    imagePool.Add(image);
                }
            }

            int remainingSlots = totalSlots - imagePool.Count;
            int index = 0;
            while (remainingSlots > 0)
            {
                imagePool.Add(_ints[index]);
                imagePool.Add(_ints[index]); 
                remainingSlots -= 2;
                index = (index + 1) % numImages;
            }

            Shuffle(imagePool);

            int[,] grid = new int[Rows, Columns];
            int poolIndex = 0;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    grid[i, j] = imagePool[poolIndex++];
                }
            }

            return grid;
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random random = new();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        public static void FlipCell(GridCell girdCell)
        {
            girdCell.IsFlipped = !girdCell.IsFlipped;
            if (girdCell.IsFlipped)
            {
                girdCell.CurrentImagePath = girdCell.BackImagePath;
            }
            else
            {
                girdCell.CurrentImagePath = girdCell.FrontImagePath;
            }
        }
    }
}
