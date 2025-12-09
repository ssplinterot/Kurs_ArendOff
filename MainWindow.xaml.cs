using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using Kurs_ArendOff;


namespace Kurs_ArendOff
{
    public partial class MainWindow : Window
    {
        private bool isExpanded = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Расширение меню
        private void SideMenu_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!isExpanded)
            {
                AnimateMenu(200, 60, true);
                isExpanded = true;
            }
        }

        // Сжатие меню
        private void SideMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isExpanded)
            {
                AnimateMenu(60, 200, false);
                isExpanded = false;
            }
        }

        // Анимация ширины и текста
        private void AnimateMenu(double newWidth, double marginLeft, bool showText)
        {
            // Анимация ширины панели
            var widthAnimation = new DoubleAnimation
            {
                To = newWidth,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            SideMenu.BeginAnimation(WidthProperty, widthAnimation);

            // Анимация отступа основного содержимого
            var marginAnimation = new ThicknessAnimation
            {
                To = new Thickness(newWidth, 0, 0, 0),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            MainContent.BeginAnimation(MarginProperty, marginAnimation);

            // Плавное появление текста
            AnimateTextOpacity(txtDiagram, showText);
            AnimateTextOpacity(txtPlaces, showText);
            AnimateTextOpacity(txtDiary, showText);
            AnimateTextOpacity(txtSettings, showText);
        }

        // Анимация прозрачности текста
        private void AnimateTextOpacity(TextBlock textBlock, bool fadeIn)
        {
            var animation = new DoubleAnimation
            {
                To = fadeIn ? 1 : 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            textBlock.BeginAnimation(OpacityProperty, animation);
        }

        private void PlaceButton_Click(object sender, RoutedEventArgs e)
        {
            MapWindow.Navigate(new Kurs_ArendOff.Page1());
        }

        private void DiaryButton_Click(object sender, RoutedEventArgs e)
        {

            MapWindow.Navigate(new Kurs_ArendOff.Pages.DiaryPage());
        }
    }
}