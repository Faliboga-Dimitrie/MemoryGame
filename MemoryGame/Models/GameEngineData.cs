using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Models
{
    public class GameEngineData
    {
        public GameEngine GameEngine { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int SecondsLeft { get; set; }
        public int MinutesLeft { get; set; }
        public string SelectedCategory { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public GameEngineData(GameEngine gameEngine,string selectedCategory, 
            int rows, int columns, int minutes, int seconds, int minutesLeft, int secondsLeft)
        {
            GameEngine = gameEngine;
            SelectedCategory = selectedCategory;
            Rows = rows;
            Columns = columns;
            Minutes = minutes;
            Seconds = seconds;
            MinutesLeft = minutesLeft;
            SecondsLeft = secondsLeft;
        }
    }
}
