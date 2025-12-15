using Kurs_ArendOff.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Kurs_ArendOff.Pages
{
    public partial class DiagramPage : Page
    {
        public DiagramPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPinnedContractData();
        }

        private void LoadPinnedContractData()
        {
            try
            {
                using (var db = new ApplicationContext())
                {
                    // Ищем первый закрепленный договор
                    var pinnedContract = db.OrganizationDatas
                                           .FirstOrDefault(o => o.IsPinned == true);
                    if (pinnedContract != null)
                    {
                        TimeSpan remaining = pinnedContract.ContractEndDate - DateTime.Today;
                        int daysRemaining = remaining.Days;

                        // Расчет общей длительности договора в днях
                        TimeSpan totalDurationTime = pinnedContract.ContractEndDate - pinnedContract.ContractStartDate;
                        double totalDays = totalDurationTime.TotalDays;

                        // Расчет процента ПРОШЕДШЕГО времени (completionPercentage: 0.0 в начале, 1.0 в конце)
                        double completionPercentage = 0;
                        if (totalDays > 0)
                        {
                            double currentDaysPassed = Math.Max(0, totalDays - Math.Max(0, remaining.TotalDays));
                            completionPercentage = Math.Min(1.0, currentDaysPassed / totalDays); // Не превышаем 100%
                        }

                        // Обновление UI
                        PlaceholderPanel.Visibility = Visibility.Collapsed;
                        DiagramContentPanel.Visibility = Visibility.Visible;
                        ContractTitle.Text = $"Закреплен: {pinnedContract.OrganizationName}";
                        DaysRemainingText.Text = $"{Math.Max(0, daysRemaining)} Дней";
                        EndDateText.Text = pinnedContract.ContractEndDate.ToShortDateString();
                        TenantNameText.Text = pinnedContract.OrganizationName;
                        AmountText.Text = $"{pinnedContract.RentalAmount:N2} BYN";

                        // Определение цвета
                        string statusColor = GetStatusColor(daysRemaining);
                        System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(statusColor);
                        DaysRemainingText.Foreground = new SolidColorBrush(color);

                        // Запуск анимации прогресса (передаём completionPercentage)
                        AnimateProgress(ProgressPath, completionPercentage, statusColor, daysRemaining);
                    }
                    else
                    {
                        PlaceholderPanel.Visibility = Visibility.Visible;
                        DiagramContentPanel.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных диаграммы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PlaceholderPanel.Visibility = Visibility.Visible;
                DiagramContentPanel.Visibility = Visibility.Collapsed;
            }
        }

        private string GetStatusColor(int daysRemaining)
        {
            if (daysRemaining > 60)
                return "Green";
            else if (daysRemaining > 30)
                return "Orange";
            else if (daysRemaining >= 0)
                return "Red";
            else // Если просрочен
                return "Gray";
        }

        private void AnimateProgress(Path targetPath, double percentage, string statusColor, int daysRemaining)
        {
            // 1. Установка цвета
            System.Windows.Media.Color color = (System.Windows.Media.Color)ColorConverter.ConvertFromString(statusColor);
            targetPath.Stroke = new SolidColorBrush(color);

            // 2. Расчет длины окружности (Radius = 140)
            double radius = 140;
            double circumference = 2 * Math.PI * radius;

            // 3. DashArray: полный dash + gap для анимации
            targetPath.StrokeDashArray = new DoubleCollection { circumference, circumference };

            // 4. Обработка просроченного договора: сразу полный круг без анимации
            if (daysRemaining < 0)
            {
                targetPath.StrokeDashOffset = 0; // Полный заполненный круг
                return;
            }

            // 5. Начальное положение: полностью скрыто (offset = circumference)
            targetPath.StrokeDashOffset = circumference;

            // 6. Анимация: от circumference (пусто) до circumference - (percentage * circumference) (заполнено на percentage)
            DoubleAnimation animation = new DoubleAnimation
            {
                From = circumference,
                To = circumference - (percentage * circumference),
                Duration = TimeSpan.FromSeconds(1.5), // Плавная анимация (можно увеличить до 2-3 сек)
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // 7. Запуск анимации
            targetPath.BeginAnimation(Shape.StrokeDashOffsetProperty, animation);
        }
    }
}