using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;
using MemoryGame.Enums;

namespace MemoryGame.Services
{
    public static class WindowTransitionService
    {
        public static async Task FadeSwitch(Window from, Window to, int duration = 500)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(duration));
            from.BeginAnimation(Window.OpacityProperty, fadeOut);
            await Task.Delay(duration);

            to.Opacity = 0;
            to.Show();

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(duration));
            to.BeginAnimation(Window.OpacityProperty, fadeIn);

            from.Close();
        }

        public static async Task SlideSwitch(Window from, Window to, SlideDirection direction, int duration = 500)
        {
            double fromStartLeft = from.Left;
            double fromStartTop = from.Top;
            double toStartLeft = from.Left;
            double toStartTop = from.Top;

            double toEndLeft = from.Left;
            double toEndTop = from.Top;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            switch (direction)
            {
                case SlideDirection.Left:
                    toStartLeft = screenWidth;
                    fromStartLeft = from.Left;
                    toEndLeft = from.Left;
                    break;

                case SlideDirection.Right:
                    toStartLeft = -from.Width;
                    toEndLeft = from.Left;
                    break;

                case SlideDirection.Up:
                    toStartTop = screenHeight;
                    toEndTop = from.Top;
                    break;

                case SlideDirection.Down:
                    toStartTop = -from.Height;
                    toEndTop = from.Top;
                    break;
            }

            // Set initial position of the new window
            to.Left = toStartLeft;
            to.Top = toStartTop;
            to.Show();

            // Animate old window out (optional)
            DoubleAnimation? fromAnim = null;
            if (direction == SlideDirection.Left || direction == SlideDirection.Right)
                fromAnim = new DoubleAnimation(from.Left, (direction == SlideDirection.Left ? -from.Width : screenWidth), TimeSpan.FromMilliseconds(duration));
            else if (direction == SlideDirection.Up || direction == SlideDirection.Down)
                fromAnim = new DoubleAnimation(from.Top, (direction == SlideDirection.Down ? screenHeight : -from.Height), TimeSpan.FromMilliseconds(duration));

            // Animate new window in
            var toAnimX = new DoubleAnimation(toStartLeft, toEndLeft, TimeSpan.FromMilliseconds(duration));
            var toAnimY = new DoubleAnimation(toStartTop, toEndTop, TimeSpan.FromMilliseconds(duration));

            if (direction == SlideDirection.Left || direction == SlideDirection.Right)
            {
                from.BeginAnimation(Window.LeftProperty, fromAnim);
                to.BeginAnimation(Window.LeftProperty, toAnimX);
            }
            else
            {
                from.BeginAnimation(Window.TopProperty, fromAnim);
                to.BeginAnimation(Window.TopProperty, toAnimY);
            }

            await Task.Delay(duration);
            from.Close();
        }
    }
}
