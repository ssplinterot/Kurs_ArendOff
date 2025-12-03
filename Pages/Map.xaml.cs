using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurs_ArendOff
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }
        private void Rect_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#96695e"));
            (sender as Rectangle).Opacity = 0.6;

            popUpName.Text = "Золотое Яблоко";
            popUpArea.Text = "Площадь: 60 м²";
            popUpRent.Text = "Аренда: 600 тыс.руб.";
            popUpStatus.Text = "Статус: занято";

            popUpMenu.Visibility = Visibility.Visible;
            Canvas.SetLeft(popUpMenu, 1105 + 20);
            Canvas.SetTop(popUpMenu, 52 + 20);
        }

        private void Rect_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Rectangle).Fill = Brushes.Transparent;
            popUpMenu.Visibility = Visibility.Collapsed;
        }

        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            // Двигаем меню за курсором
            if (popUpMenu.Visibility == Visibility.Visible)
            {
                Point mousePosition = e.GetPosition(this);
                Canvas.SetLeft(popUpMenu, mousePosition.X + 15);
                Canvas.SetTop(popUpMenu, mousePosition.Y + 15);
            }
        }
    }
}