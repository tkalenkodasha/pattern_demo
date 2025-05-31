using hotel_pattern.HotelDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using hotel_pattern.HotelDataSetTableAdapters;

namespace hotel_pattern
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
            string login = loginTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();

            

        }

        private void Auth_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "hotelDataSet.users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter.Fill(this.hotelDataSet.users);

        }
        int error_counter = 0;
        private void loginButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            //проверка на пустые значения
            if (login == null || password == null) {
                MessageBox.Show("введите логин и пароль!", "ошибка входа");
                return;

            }
            var user = usersTableAdapter.GetData().FirstOrDefault(u=>u.login==login);
            //проверка на существование пользователя в бд
            if (user == null)
            {
                MessageBox.Show("пользователь не найден!", "ошибка входа");
                return;
            }
            //проверка на статус блокировки пользователя
            if (user.block_status == "block")
            {
                MessageBox.Show("Ваша учетная запись заблокирована!", "ошибка входа");
                return;
            }
            //проверка на дату последнего входа
            if (user.enter_date!=null)
            {
                TimeSpan timeSinceLastLogin=DateTime.Now-user.enter_date;
                if (timeSinceLastLogin.TotalDays > 30)
                {
                    MessageBox.Show("последний вход был более 30 дней назад, поэтому Ваша учетная запись заблокирована!", "ошибка входа");
                    user.block_status = "block"; // Блокируем учетную запись
                    usersTableAdapter.Update(user); // Сохраняем изменения в базе данных
                    
                    return;
                    
                }
               
            }
            //проверка на правильность пароля
            if (user.password != password)
            {
                error_counter++;
                MessageBox.Show($"Неверный пароль! осталось попыток: {3-error_counter}", "ошибка входа");
                if (error_counter == 3) {
                    MessageBox.Show($"превышен лимит попыток входа! ваш учетная запись заблокирована! обратитесь к администратору системы", "ошибка входа");
                    user.block_status = "block";
                    usersTableAdapter.Update(user);
                    
                    return;
                }
                return;
            }
            //при успешной попытке входа
            user.enter_date = DateTime.Now;
            usersTableAdapter.Update(user);
            new MainForm(user.role_id).Show();
            this.Hide();
        }

        private void guestLabel_Click(object sender, EventArgs e)
        {
            int guest = 4;
            new MainForm(guest).Show();
            this.Hide();
        }
    }
}
