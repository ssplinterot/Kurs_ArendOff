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
                using (var db = new ApplicationContext())
                {

                    var organizationData = db.OrganizationDatas
                        .Select(o => new // Проекция на анонимный тип
                        {
                            // ID
                            ID = o.Id,

                            // Названия организации и договора
                            Название_Организации = o.OrganizationName,
                            Номер_Договора = o.ContractNumber,
                            ИНН = o.INN,

                            // Даты
                            Начало_Аренды = o.ContractStartDate,
                            Окончание_Аренды = o.ContractEndDate,

                            // Финансы и статус
                            Сумма_Аренды = o.RentalAmount,
                            Оплачено = o.IsPaid,

                            // Помещение
                            Идентификатор_Места = o.PlaceIdentifier
                        })
                        .ToList();
                    ContractsDataGrid.ItemsSource = organizationData;
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