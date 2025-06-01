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
        private readonly HotelDataSet.usersRow _user;
        private readonly usersTableAdapter _usersTableAdapter;

        public ChangePassword(HotelDataSet.usersRow user)
        {
            InitializeComponent();
            _user = user;
            _usersTableAdapter = new usersTableAdapter();

        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            string currentPassword = textBox1.Text.Trim();
            string newPassword = textBox2.Text.Trim();
            string newPassword2 = textBox3.Text.Trim();
            if (currentPassword == null || newPassword == null || newPassword2==null)
            {
                MessageBox.Show("Заполните все поля!", "ошибка");
                return;
            }
            if (currentPassword != _user.password) 
            {
                MessageBox.Show("неверный текущий пароль!", "ошибка");
                return;
            }
            if (newPassword != newPassword2) 
            {
                MessageBox.Show("новый пароль не совпадает с подтверждением!", "ошибка");
                return;
            }
            _user.password = newPassword2;
            _usersTableAdapter.Update(_user);
            MessageBox.Show("пароль успешно изменен!", "оповещение");
            this.Hide();


        }
    }
}
