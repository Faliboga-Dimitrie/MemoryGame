using MemoryGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.Models
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, buttons, icon);
        }
    }
}
