// SelectionPage.xaml.cs

using System.Windows;
using System.Windows.Controls;

namespace Kurs_ArendOff
{
    public partial class SelectionPage : Page
    {
        public SelectionPage()
        {
            InitializeComponent();
        }

        private void ObjectButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            string objectId = button.Tag.ToString();

            // 1. Находим главное окно, чтобы вызвать метод навигации
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                // 2. Вызываем метод MainWindow, который загрузит MapPage (Page1)
                // с нужным ID объекта.
                mainWindow.LoadMapForObject(objectId);
            }
        }
    }
}