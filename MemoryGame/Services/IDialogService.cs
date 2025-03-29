using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon);
    }
}
