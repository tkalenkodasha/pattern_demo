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

namespace hotel_pattern.UserControls
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            usersTableAdapter.Fill(hotelDataSet.users);
            rolesTableAdapter.Fill(hotelDataSet.roles);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("вы уверены, что хотите применить изменерия?", "предупреждение", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    usersBindingSource.EndEdit();
                    int rows = usersTableAdapter.Update(hotelDataSet.users);
                    MessageBox.Show($"Сохранено строк: {rows}", "оповещение");

                }
                else if (res == DialogResult.Cancel)
                {
                    usersTableAdapter.Fill(hotelDataSet.users);
                    rolesTableAdapter.Fill(hotelDataSet.roles);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("что-то пошло не так" + ex.Message, "оповещение об ошибке", MessageBoxButtons.OKCancel);
            }
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            usersTableAdapter.Fill(hotelDataSet.users);
            rolesTableAdapter.Fill(hotelDataSet.roles);
        }
    }
}
