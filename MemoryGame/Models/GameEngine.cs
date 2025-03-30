using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Models
{
    public class GameEngine : BaseClass
    {
        private static GameEngine _instance;
        public static GameEngine Instance => _instance ??= new GameEngine();
        
        private int _pairsFound;
        private GridCell? _firstCell;
        private GridCell? _secondCell;

        public int PairsFound
        {
            get => _pairsFound;
            set
            {
                _pairsFound = value;
                OnPropertyChanged();
            }
        }

        public GridCell? FirstCell
        {
            get => _firstCell;
            set
            {
                _firstCell = value;
                OnPropertyChanged();
            }
        }

        public GridCell? SecondCell
        {
            get => _secondCell;
            set
            {
                _secondCell = value;
                OnPropertyChanged();
            }
        }

        public GameEngine()
        {
            _pairsFound = 0;
            _firstCell = null;
            _secondCell = null;
        }
    }
}
