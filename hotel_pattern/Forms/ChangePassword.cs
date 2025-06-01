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

namespace hotel_pattern.Forms
{
    public partial class ChangePassword : Form
    {
      
        private string login;

        public ChangePassword(string login)
        {
            InitializeComponent();
  
            this.login = login;
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            var user = usersTableAdapter.GetData().FirstOrDefault(u => u.login==login);
            string currentPassword = textBox1.Text.Trim();
            string newPassword = textBox2.Text.Trim();
            string newPassword2 = textBox3.Text.Trim();
            if (currentPassword == null || newPassword == null || newPassword2==null)
            {
                MessageBox.Show("Заполните все поля!", "ошибка");
                return;
            }
            if (currentPassword != user.password) 
            {
                MessageBox.Show("неверный текущий пароль!", "ошибка");
                return;
            }
            if (newPassword != newPassword2) 
            {
                MessageBox.Show("новый пароль не совпадает с подтверждением!", "ошибка");
                return;
            }
            user.password = newPassword2;
            usersTableAdapter.Update(user);
            MessageBox.Show("пароль успешно изменен!", "оповещение");
            this.Hide();


        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "hotelDataSet.users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter.Fill(this.hotelDataSet.users);

        }
    }
}
