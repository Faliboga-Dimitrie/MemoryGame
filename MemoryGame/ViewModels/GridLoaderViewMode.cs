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


        private static List<string> _category;
        private static List<int> _ints;
        private static int _categoryIndex;
        private static int _numberOfImages;
        private int _rows = 4;
        private int _columns = 3;


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

        private ObservableCollection<GridCell> _cells = new();
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

        static GridLoaderViewMode()
        {
            _category = new List<string>
            {
                "Images/GameImages/Animals/Animal",
                "Images/GameImages/Landmarks/landmark",
                "Images/GameImages/Vehicles/Vehicle"
            };
            _ints = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            _categoryIndex = 0;
            _numberOfImages = 8;
        }

        public GridLoaderViewMode()
        {
            GenerateCells();
        }

        private int[,] GenerateMemoryGrid()
        {
            int totalSlots = Rows * Columns;

            int numImages = _ints.Count;
            List<int> imagePool = new List<int>();

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

            // Step 1: Calculate base pair count per image
            int basePairCount = totalSlots / numImages;
            if (basePairCount % 2 != 0)
                basePairCount -= 1; // Ensure it's even

            // Step 2: Add base pairs to image pool
            foreach (var image in _ints)
            {
                for (int i = 0; i < basePairCount; i++)
                {
                    imagePool.Add(image);
                }
            }

            // Step 3: Handle remaining slots
            int remainingSlots = totalSlots - imagePool.Count;
            int index = 0;
            while (remainingSlots > 0)
            {
                imagePool.Add(_ints[index]);
                imagePool.Add(_ints[index]); // Add a pair
                remainingSlots -= 2;
                index = (index + 1) % numImages; // Circular distribution
            }

            // Step 4: Shuffle the image pool
            Shuffle(imagePool);

            // Step 5: Map shuffled list to grid
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
            Random random = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
