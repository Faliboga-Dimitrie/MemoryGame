using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Ookii.Dialogs.WinForms;

namespace MemoryGame.Services
{
    public static class DialogService
    {
        public static void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, buttons, icon);
        }

        public static void ShowMessageWithLink(string message, string title)
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                var dialog = new TaskDialog
                {
                    WindowTitle = title,
                    MainInstruction = title,
                    Content = message,
                    EnableHyperlinks = true
                };

                dialog.HyperlinkClicked += (sender, e) =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = e.Href,
                        UseShellExecute = true
                    });
                };

                dialog.Buttons.Add(new TaskDialogButton(ButtonType.Ok));
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show(StripHtml(message), title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static string StripHtml(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }
    }

}
