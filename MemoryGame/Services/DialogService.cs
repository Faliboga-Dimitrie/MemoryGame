using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MemoryGame.Models
{
    using System.Diagnostics;
    using System.Windows;
    using Ookii.Dialogs.WinForms;

    public class DialogService
    {
        public void ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            // Fall back to standard MessageBox
            MessageBox.Show(message, title, buttons, icon);
        }

        public void ShowMessageWithLink(string message, string title)
        {
            if (TaskDialog.OSSupportsTaskDialogs)
            {
                var dialog = new TaskDialog
                {
                    WindowTitle = title,
                    MainInstruction = title,
                    Content = message, // You can include <a href="...">link</a> in this string
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
                // If TaskDialog isn't supported, fallback to plain MessageBox
                MessageBox.Show(StripHtml(message), title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string StripHtml(string input)
        {
            // Simple method to remove HTML tags for fallback message
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }
    }

}
