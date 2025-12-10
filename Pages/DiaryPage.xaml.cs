using Kurs_ArendOff.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kurs_ArendOff.Pages
{
    /// <summary>
    /// Логика взаимодействия для DiaryPage.xaml
    /// </summary>
    public partial class DiaryPage : Page
    {
        public DiaryPage()
        {
            InitializeComponent();
        }

        // Метод, который срабатывает, когда страница загрузится
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContractsData();
        }

        /// <summary>
        /// Загружает все записи о договорах из БД и отображает их в DataGrid.
        /// </summary>
        private void LoadContractsData()
        {
            try
            {
                // Используем Entity Framework для подключения к БД
                using (var db = new ApplicationContext())
                {
                    // 1. Запрос: Получаем все записи из таблицы OrganizationDatas
                    var contracts = db.OrganizationDatas.ToList();

                    // 2. Привязка данных к таблице ContractsDataGrid
                    // DataContext - это источник данных для элементов на странице.
                    ContractsDataGrid.ItemsSource = contracts;

                    // Обучение: ItemsSource — это ключевое свойство для WPF-элементов
                    // (например, ListBox, DataGrid). Оно указывает, какой набор данных 
                    // нужно отобразить.
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных договоров: {ex.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}