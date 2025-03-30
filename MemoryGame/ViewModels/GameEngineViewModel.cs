using MemoryGame.Commands;
using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.ViewModels
{
    public class GameEngineViewModel : BaseClass
    {

        private static GameEngineViewModel _instance;
        public static GameEngineViewModel Instance => _instance ??= new GameEngineViewModel();

        private GridLoaderViewMode _gridLoader;
        private GameEngine _gameEngine;

        public int Rows
        {
            get => GridLoader.Rows;
            set
            {
                GridLoader.Rows = value;
                OnPropertyChanged();
            }
        }

        public int Columns
        {
            get => GridLoader.Columns;
            set
            {
                GridLoader.Columns = value;
                OnPropertyChanged();
            }
        }

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

        public GameEngineViewModel()
        {
            _gridLoader = new GridLoaderViewMode();
            _gameEngine = new GameEngine();
            Rows = GridLoader.Rows;
            Columns = GridLoader.Columns;
            FlipCommand = new RelayCommand(FlipCell);
            CategoryCommand = new RelayCommand(SelectCategoryCommand);
        }

        public RelayCommand FlipCommand { get; }

        public RelayCommand CategoryCommand { get; }

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
                        GameEngine.FirstCell.Visibility = Visibility.Hidden;
                        GameEngine.SecondCell.Visibility = Visibility.Hidden;
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

    }

}
