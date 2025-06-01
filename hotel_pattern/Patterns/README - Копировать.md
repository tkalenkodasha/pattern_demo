## Auth ##
```csharp
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

            MessageBox.Show("вы успешно авторизовались!", "оповещение");
            if (user.password == "new_password")
            {
                new ChangePassword(user).Show();
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
```

## ChangePassword ##
```csharp
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
```

## Main ##
```csharp
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
```
## COntrols ##
```csharp
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
            try
            {
                usersTableAdapter.Fill(hotelDataSet.users);
                rolesTableAdapter.Fill(hotelDataSet.roles);
            }
            catch (Exception ex) {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message);
            }
            
           
        }
```