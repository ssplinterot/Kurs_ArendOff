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

namespace Kurs_ArendOff.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadCurrentBuilding();
        }
        private void LoadCurrentBuilding()
        {
            // Загрузка текущего выбранного здания при открытии страницы
            if (Application.Current.Properties["SelectedBuildingId"] is int currentId)
            {
                // Находим соответствующий RadioButton по Tag (ID здания)
                foreach (var child in BuildingSelectionPanel.Children)
                {
                    if (child is RadioButton rb && rb.Tag?.ToString() == currentId.ToString())
                    {
                        rb.IsChecked = true;
                        break;
                    }
                }
            }
            // Если свойство еще не установлено, по умолчанию выбираем первое здание
            else if (BuildingSelectionPanel.Children.Count > 0 && BuildingSelectionPanel.Children[0] is RadioButton defaultRb)
            {
                defaultRb.IsChecked = true;
                Application.Current.Properties["SelectedBuildingId"] = int.Parse(defaultRb.Tag.ToString());
            }
        }

        private void BuildingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRb && checkedRb.IsChecked == true)
            {
                // Сохраняем ID выбранного здания в свойствах приложения
                if (checkedRb.Tag != null && int.TryParse(checkedRb.Tag.ToString(), out int buildingId))
                {
                    Application.Current.Properties["SelectedBuildingId"] = buildingId;

                    // !!! УДАЛЕНИЕ: Убираем всплывающее уведомление о смене здания
                    // MessageBox.Show($"Выбрано здание с ID: {buildingId}", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Если вы хотите видеть, какое здание выбрано, но не в виде MessageBox,
                    // можно использовать вывод в консоль для отладки:
                    System.Diagnostics.Debug.WriteLine($"Выбрано здание с ID: {buildingId}");
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из профиля?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 1. Очистка данных пользователя (можно очистить SelectedBuildingId, если нужно сбросить выбор)
                Application.Current.Properties["CurrentUser"] = null;

                // 2. Находим текущее главное окно и скрываем его
                Window currentMainWindow = Window.GetWindow(this);

                if (currentMainWindow != null)
                {
                    try
                    {
                        // 3. Открываем окно авторизации
                        AuthWindow loginWindow = new AuthWindow();
                        loginWindow.Show();

                        // 4. Закрываем главное окно (после того, как окно входа успешно открыто)
                        currentMainWindow.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии окна авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        // Если ошибка, можно показать главное окно обратно
                        currentMainWindow.Show();
                    }
                }
            }
        }
    }
}

