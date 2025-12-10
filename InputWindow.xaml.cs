using System;
using System.Windows;
using Kurs_ArendOff.Models; 
using System.Windows.Controls;
using System.Linq; 

namespace Kurs_ArendOff
{
    public partial class InputWindow : Window
    {
        
        private readonly string _placeIdentifier;// Поле для хранения ID помещения 
        public InputWindow() : this("Не указано")
        {
            // Здесь просто вызывается конструктор ниже, передавая "Не указано"
        }

        /// <param name="placeIdentifier">Уникальный идентификатор помещения.</param>
        public InputWindow(string placeIdentifier)
        {
            InitializeComponent();
            _placeIdentifier = placeIdentifier;//Сохраняем переданный ID в приватное поле класса

            // Устанавливаем заголовок
            Title = $"Ввод данных договора для места: {_placeIdentifier}";
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        { 
            if (!ValidateInput()) return;//Сбор и проверка данных
  
            var newOrgData = new OrganizationData
            {
                OrganizationName = TxtOrgName.Text,
                INN = TxtINN.Text,
                ContractNumber = TxtContractNumber.Text,
                ContractStartDate = DpStartDate.SelectedDate.Value,
                ContractEndDate = DpEndDate.SelectedDate.Value,
                RentalAmount = decimal.Parse(TxtRentalAmount.Text),

                IsPaid = ChkIsPaid.IsChecked ?? false,

               
                PlaceIdentifier = _placeIdentifier //Используем сохраненный ID из приватного поля
            };

            //Сохранение в базу данных
            try
            {
                using (var db = new ApplicationContext())
                {
                    db.OrganizationDatas.Add(newOrgData);
                    db.SaveChanges();

                    MessageBox.Show($"Договор для места {_placeIdentifier} успешно сохранен!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
           
            if (string.IsNullOrWhiteSpace(TxtOrgName.Text) ||
                string.IsNullOrWhiteSpace(TxtINN.Text) ||
                string.IsNullOrWhiteSpace(TxtContractNumber.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все текстовые поля.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!DpStartDate.SelectedDate.HasValue || !DpEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите даты начала и конца аренды.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (DpStartDate.SelectedDate.Value >= DpEndDate.SelectedDate.Value)
            {
                MessageBox.Show("Дата начала аренды должна быть раньше даты окончания.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(TxtRentalAmount.Text, out _))
            {
                MessageBox.Show("Сумма аренды должна быть числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}