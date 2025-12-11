using Kurs_ArendOff.Models;
using Kurs_ArendOff.Pages;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kurs_ArendOff
{

    public partial class Page1 : Page
    {
        // Поле для хранения данных помещения, на которое сейчас наведен курсор
        private Place CurrentPlaceData { get; set; }

        public Page1()
        {
            InitializeComponent();

        }
        private void PositionPopUpMenu(MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            //Canvas.SetLeft(popUpMenu, mousePosition.X-50);
            //Canvas.SetTop(popUpMenu, mousePosition.Y);
        }
        private void Rect_MouseEnter(object sender, MouseEventArgs e)
        {
            var shape = sender as System.Windows.Shapes.Shape;
            if (shape?.Tag == null) return;

            var parentCanvas = shape.Parent as Canvas;
            if (parentCanvas == null) return;

            shape.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#96695e")); // Коричневый цвет
            shape.Opacity = 0.6;

            //Загрузка данных из БД из Tag
            string placeId = shape.Tag.ToString();
            try
            {
                using (var db = new ApplicationContext())
                {
                    // Ищем данные помещения по его ID
                    CurrentPlaceData = db.Places
                                         .FirstOrDefault(p => p.PlaceIdentifier == placeId);

                    if (CurrentPlaceData != null)
                    {
                        //Заполнение всплывающего меню корректными данными
                        popUpName.Text = CurrentPlaceData.Name;
                        popUpArea.Text = $"Площадь: {CurrentPlaceData.Area} м²";
                        popUpRent.Text = $"Аренда: {CurrentPlaceData.TotalRent:N2} BYN"; // Используем вычисляемое свойство
                        //popUpStatus.Text = $"Статус: {CurrentPlaceData.FullStatus}";
                        popUpDescription.Text = CurrentPlaceData.Description;

                        popUpMenu.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Если в БД нет данных, показываем ошибку
                        popUpName.Text = $"Ошибка: Данные для ID не найдены.";
                        popUpArea.Text = "Площадь: Неизвестно";
                        popUpRent.Text = "Аренда: Неизвестно";
                        popUpStatus.Text = "Статус: Неизвестно";
                        //popUpDescription.Text = "Пожалуйста, добавьте данные в таблицу Places.";
                        popUpMenu.Visibility = Visibility.Visible;
                    }

                    //Позиционирование всплывающего окна
                    Point mousePosition = e.GetPosition(parentCanvas);
                    PositionPopUpMenu(e);
                    Canvas.SetLeft(popUpMenu, mousePosition.X + 15);
                    Canvas.SetTop(popUpMenu, mousePosition.Y + 15);
                    popUpMenu.Visibility = Visibility.Visible;
                }
            }
            catch (System.Exception ex)
            {
                // Обработка ошибок БД
                popUpName.Text = "Ошибка БД";
                popUpDescription.Text = $"Не удалось загрузить данные: {ex.Message}";
                popUpMenu.Visibility = Visibility.Visible;
            }

        }
        private void Rect_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем исходный прозрачный цвет и скрываем меню
            (sender as Shape).Fill = Brushes.Transparent;
            popUpMenu.Visibility = Visibility.Collapsed;
        }
        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            // Двигаем меню за курсором
            if (popUpMenu.Visibility == Visibility.Visible)
            {
                Point mousePosition = e.GetPosition(this);
                PositionPopUpMenu(e);
                //Canvas.SetLeft(popUpMenu, mousePosition.X -150);
                //Canvas.SetTop(popUpMenu, mousePosition.Y -50);
            }
        }
        private void Place_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var shape = sender as Shape;
            if (shape?.Tag == null) return;

            string placeId = shape.Tag.ToString();
            //Открываем окно ввода данных, передавая идентификатор помещения
            var inputWindow = new InputWindow(placeId);
            inputWindow.ShowDialog();

            //после закрытия InputWindow переходим на страницу Дневника
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MapWindow.Navigate(new DiaryPage());
            }
        }

        
    }
}