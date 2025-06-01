using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trenirovka_auth.DataSet1TableAdapters;

namespace trenirovka_auth
{
    public partial class Authh : Form
    {
        public Authh()
        {
            InitializeComponent();
            
        }
        int error_conter = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            if (login ==null|| password == null)
            {
                MessageBox.Show("введите логин и пароль", "оповещение");
                return;
            }
            var user =usersTableAdapter.GetData().FirstOrDefault(u=>u.login == login);
            if (user == null)
            {
                MessageBox.Show("пользователь не найден", "оповещение");
                return;
            }
            if (user.block_status == "block")
            {
                MessageBox.Show("ваша учетная запись заблокирована. обратитесь к системному администратору", "оповещение");
                return;    
            }
            if (user.enter_date != null)
            {
                TimeSpan lastSinceEnter= DateTime.Now-user.enter_date;
                if (lastSinceEnter.TotalDays > 30)
                {
                    MessageBox.Show("последний вход был более 30 дней назадю Ваша учетная запись заблокирована. обратитесь к системному администратору", "оповещение");
                    user.block_status="block";
                    return;
                }
                
            }
            if (user.password != password)
            {
                error_conter++;
                MessageBox.Show($"неверный паролью осталось попыток {3-error_conter} ", "оповещение");
                if (error_conter == 3)
                {
                    user.block_status = "block";
                    MessageBox.Show("превышен лимит попыток входа. ваша учетная запись заблокирована. обратитесь к системному администратору", "оповещение");
                    return;
                }
               
                return;
            }

            user.enter_date = DateTime.Now;
            usersTableAdapter.Update(user);
            new Main(user.role_id).Show();
            this.Hide();
        }

        private void Authh_Load_1(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataSet1.users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter.Fill(this.dataSet1.users);
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int guestRole = 4;
            new Main(guestRole).Show();
            this.Hide();
        }
    }
}
