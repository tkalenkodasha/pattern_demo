using hotel_pattern.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_pattern
{
    public partial class MainForm : Form
    {
        private int _userRole;
        public MainForm(int userRole)
        {
            InitializeComponent();
            _userRole = userRole;
            if (userRole == 1)//администратор
            {
                bookingsToolStripMenuItem.Visible = true;
                guestsToolStripMenuItem.Visible = true;
                roomsToolStripMenuItem.Visible = true;
                cleanSheduleToolStripMenuItem.Visible = true;
                usersToolStripMenuItem.Visible = true;
                servicesToolStripMenuItem.Visible = true;
            }
            else if (userRole == 2) //руководитель
            {
                statisticToolStripMenuItem.Visible = true;
                staffToolStripMenuItem.Visible = true;
                bookingsToolStripMenuItem.Visible = true;
                guestsToolStripMenuItem.Visible = true;
                roomsToolStripMenuItem.Visible = true;
                cleanSheduleToolStripMenuItem.Visible = true;
                usersToolStripMenuItem.Visible = true;
                servicesToolStripMenuItem.Visible = true;
            }
            else if (userRole == 3) //пользователь (уборщик например)
            {

                cleanSheduleToolStripMenuItem.Visible = true;

            }
            else if (userRole == 4) //гость
            {
                servicesToolStripMenuItem.Visible = true;
            }

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            UserControl1 usersTable = new UserControl1();
            usersTable.Dock = DockStyle.Fill;   
            panelContent.Controls.Add(usersTable);
        }

        private void bookingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            BookingsUserControl bookingsTable = new BookingsUserControl();
            bookingsTable.Dock = DockStyle.Fill;
            panelContent.Controls.Add(bookingsTable);
        }

        private void guestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            GuestsUserControl guestsTable = new GuestsUserControl();
            guestsTable.Dock = DockStyle.Fill;
            panelContent.Controls.Add (guestsTable);
        }

        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panelContent.Controls.Clear();
            RoomsUserControl roomsTable = new RoomsUserControl();
            roomsTable.Dock = DockStyle.Fill;
            panelContent.Controls.Add(roomsTable);

        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            RoomsUserControl roomsTable = new RoomsUserControl();
            roomsTable.Dock = DockStyle.Fill;
            panelContent.Controls.Add(roomsTable);
        }
    }
}
