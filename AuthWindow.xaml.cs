using Kurs_ArendOff.Models;
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
using System.Windows.Shapes;

namespace Kurs_ArendOff
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = PbPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ApplicationContext())
                {
                    //Проверяем, существует ли пользователь с таким логином
                    if (db.Users.Any(u => u.Username == username))
                    {
                        MessageBox.Show("Пользователь с таким логином уже зарегистрирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Создаем нового пользователя
                    var newUser = new User { Username = username, Password = password };

                    //Добавляем в базу данных и сохраняем
                    db.Users.Add(newUser);
                    db.SaveChanges(); // Тут происходит фактическое сохранение в файл users.db

                    MessageBox.Show("Регистрация прошла успешно!");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = PbPassword.Password;

            try
            {
                using (var db = new ApplicationContext())
                {
                    // Ищем пользователя, у которого совпадают и логин, и пароль
                    var user = db.Users
                                 .FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user != null)
                    {
                        // Открываем основное окно приложения
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        // Закрываем окно авторизации
                        this.Close();
                    }
                    else
                    {
                        // Пользователь не найден или пароль не совпадает
                        MessageBox.Show("Неверный логин или пароль.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }
    }
}
