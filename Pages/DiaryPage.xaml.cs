using Kurs_ArendOff.Models;
using System;
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

        private void PinContract_Click(object sender, RoutedEventArgs e)
        {
            if (ContractsDataGrid.SelectedItem == null) return;

            var selectedItem = ContractsDataGrid.SelectedItem;
            int selectedId = (int)selectedItem.GetType().GetProperty("ID").GetValue(selectedItem, null);

            try
            {
                using (var db = new ApplicationContext())
                {
                    var contractToPin = db.OrganizationDatas.FirstOrDefault(o => o.Id == selectedId);
                    if (contractToPin == null) return;

                    // Определяем, должен ли договор быть закреплен после клика
                    bool willBePinned = !contractToPin.IsPinned;

                    // 1. Если новый договор ДОЛЖЕН БЫТЬ закреплен, открепляем все остальные
                    if (willBePinned)
                    {
                        var allPinned = db.OrganizationDatas.Where(o => o.IsPinned).ToList();
                        foreach (var item in allPinned)
                        {
                            item.IsPinned = false;
                        }
                    }

                    // 2. Устанавливаем новый статус для выбранного договора
                    contractToPin.IsPinned = willBePinned;

                    // 3. Сохраняем все изменения (открепление старых и закрепление/открепление нового)
                    db.SaveChanges();

                    // 4. Обновляем DataGrid и выводим сообщение
                    LoadContractsData();

                    string status = contractToPin.IsPinned ? "закреплен" : "откреплен";
                    MessageBox.Show($"Договор ID: {selectedId} {status}.", "Закрепление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка закрепления: {ex.Message}");
            }
        }
        private void EditContract_Click(object sender, RoutedEventArgs e)
        {
            if (ContractsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите договор для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем выбранный элемент (который является анонимным типом)
            var selectedItem = ContractsDataGrid.SelectedItem;

            // Получаем ID, используя отражение (как и в PinContract_Click)
            int selectedId = (int)selectedItem.GetType().GetProperty("ID").GetValue(selectedItem, null);

            // Теперь у нас есть ID для работы
            MessageBox.Show($"Редактирование договора ID: {selectedId}", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void ContractsDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Получаем выбранный элемент
            if (ContractsDataGrid.SelectedItem == null)
            {
                // Скрыть или отключить меню, если ничего не выбрано (опционально)
                PinMenuItem.IsEnabled = false;
                return;
            }

            PinMenuItem.IsEnabled = true;

            // Получаем ID выбранного элемента (используя отражение)
            var selectedItem = ContractsDataGrid.SelectedItem;
            int selectedId = (int)selectedItem.GetType().GetProperty("ID").GetValue(selectedItem, null);

            try
            {
                using (var db = new ApplicationContext())
                {
                    var contract = db.OrganizationDatas.FirstOrDefault(o => o.Id == selectedId);

                    if (contract != null)
                    {
                        // Если договор уже закреплен, показываем "Открепить"
                        if (contract.IsPinned)
                        {
                            PinMenuItem.Header = "Открепить договор";
                        }
                        // Если не закреплен, показываем "Закрепить"
                        else
                        {
                            PinMenuItem.Header = "Закрепить договор";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки БД, если не удалось получить статус
                MessageBox.Show($"Ошибка при проверке статуса договора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PinMenuItem.IsEnabled = false;
            }
        }
    }
}