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
    /// <summary>
    /// Логика взаимодействия для Page1.xaml (Карта)
    /// </summary>
    public partial class Page1 : Page
    {
        // Поле для хранения данных помещения, на которое сейчас наведен курсор
        private Place CurrentPlaceData { get; set; }

        public Page1()
        {
            InitializeComponent();
            // Важно: Мы отменили автоматическую раскраску, 
            // поэтому здесь нет подписки на событие this.Loaded += ...
        }

        // -------------------------------------------------------------------
        // 📌 Логика: Курсор входит в область Rectangle
        // -------------------------------------------------------------------
        private void Rect_MouseEnter(object sender, MouseEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect?.Tag == null) return;

            // 1. Устанавливаем цвет и прозрачность (Ваша логика)
            rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#96695e")); // Коричневый цвет
            rect.Opacity = 0.6;

            // 2. Загрузка данных из БД по PlaceIdentifier (из Tag)
            string placeId = rect.Tag.ToString();
            try
            {
                using (var db = new ApplicationContext())
                {
                    // Ищем данные помещения по его ID
                    CurrentPlaceData = db.Places
                                         .FirstOrDefault(p => p.PlaceIdentifier == placeId);

                    if (CurrentPlaceData != null)
                    {
                        // 3. Заполнение всплывающего меню корректными данными
                        popUpName.Text = CurrentPlaceData.Name;
                        popUpArea.Text = $"Площадь: {CurrentPlaceData.Area} м²";
                        popUpRent.Text = $"Аренда: {CurrentPlaceData.TotalRent:N2} BYN"; // Используем вычисляемое свойство
                        popUpStatus.Text = $"Статус: {CurrentPlaceData.FullStatus}";
                        popUpDescription.Text = CurrentPlaceData.Description;

                        popUpMenu.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Если в БД нет данных, показываем ошибку
                        popUpName.Text = $"Ошибка: Данные для ID {placeId} не найдены.";
                        popUpArea.Text = "Площадь: Неизвестно";
                        popUpRent.Text = "Аренда: Неизвестно";
                        popUpStatus.Text = "Статус: Неизвестно";
                        popUpDescription.Text = "Пожалуйста, добавьте данные в таблицу Places.";
                        popUpMenu.Visibility = Visibility.Visible;
                    }

                    // 4. Позиционирование всплывающего окна
                    Point mousePosition = e.GetPosition(this);
                    Canvas.SetLeft(popUpMenu, mousePosition.X + 15);
                    Canvas.SetTop(popUpMenu, mousePosition.Y + 15);
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

        // -------------------------------------------------------------------
        // 📌 Логика: Курсор покидает область Rectangle
        // -------------------------------------------------------------------
        private void Rect_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем исходный прозрачный цвет и скрываем меню (Ваша логика)
            (sender as Rectangle).Fill = Brushes.Transparent;
            popUpMenu.Visibility = Visibility.Collapsed;
        }

        // -------------------------------------------------------------------
        // 📌 Логика: Курсор движется над Rectangle
        // -------------------------------------------------------------------
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

        // -------------------------------------------------------------------
        // 📌 Логика: Клик по помещению
        // -------------------------------------------------------------------
        private void Place_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect?.Tag == null) return;

            string placeId = rect.Tag.ToString();

            // Здесь лучше не делать проверку на существование данных, чтобы дать возможность 
            // добавить договор, даже если данных в таблице Places нет (например, если она пустая).

            // 1. Открываем окно ввода данных, передавая идентификатор помещения
            var inputWindow = new InputWindow(placeId);
            inputWindow.ShowDialog();

            // 2. После закрытия InputWindow переходим на страницу Дневника
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Навигация происходит на Frame MapWindow в MainWindow
                // Важно: Проверьте, что имя Frame в MainWindow.xaml именно MapWindow.
                mainWindow.MapWindow.Navigate(new DiaryPage());
            }
        }
    }
}