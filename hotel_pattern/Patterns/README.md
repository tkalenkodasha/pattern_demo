# Шаблон приложения

### Для того, чтобы разработать приложение, нужно:
1. связать бд с visual studio
2. добавить датасет
3. сделать форму для входа
4. сделать юзерконтролы
5. создать формы для отображения юзерконтролов и связать их
6. написать руководство пользователя

## Оглавление:
1. [Форма Auth](#title1)
2. [Форма ChangePassword](#title2)
3. [Форма пользователя (н-р Admin)](#title3)
4. [UserControl](#title4)
5. [Руководство пользователя](#title5)

## <a id="title1">Форма Auth</a>

### Гайд для запоминания кода (пошагово):
1. **Структура формы**:
   - 2 TextBox: `loginTextBox`, `passwordTextBox`
   - 1 Button: `loginButton` с обработчиком `loginButton_Click`
   - 1 Label: `guestLabel` для входа как гость с обработчиком `guestLabel_Click`

2. **Логика аутентификации**:
```csharp
// Получаем данные из полей
string login = loginTextBox.Text.Trim();
string password = passwordTextBox.Text.Trim();

// Проверка на пустые поля
if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
{
    MessageBox.Show("введите логин и пароль!", "ошибка входа");
    return;
}
```

3. **Поиск пользователя**:
```csharp
// Ищем пользователя в БД
var user = usersTableAdapter.GetData().FirstOrDefault(u => u.login == login);

if (user == null)
{
    MessageBox.Show("пользователь не найден!", "ошибка входа");
    return;
}
```

4. **Проверка статуса блокировки**:
```csharp
if (user.block_status == "block")
{
    MessageBox.Show("Ваша учетная запись заблокирована!", "ошибка входа");
    return;
}
```

5. **Проверка даты последнего входа**:
```csharp
if (!user.Isenter_dateNull())
{
    TimeSpan timeSinceLastLogin = DateTime.Now - user.enter_date;
    if (timeSinceLastLogin.TotalDays > 30)
    {
        MessageBox.Show("последний вход был более 30 дней назад, поэтому Ваша учетная запись заблокирована!", "ошибка входа");
        user.block_status = "block";
        usersTableAdapter.Update(user);
        return;
    }
}
```

6. **Проверка пароля и счетчик ошибок**:
```csharp
if (user.password != password)
{
    error_counter++;
    MessageBox.Show($"Неверный пароль! осталось попыток: {3 - error_counter}", "ошибка входа");
    if (error_counter == 3)
    {
        MessageBox.Show("превышен лимит попыток входа! ваш учетная запись заблокирована! обратитесь к администратору системы", "ошибка входа");
        user.block_status = "block";
        usersTableAdapter.Update(user);
        return;
    }
    return;
}
```

7. **Успешная авторизация**:
```csharp
//при успешной попытке входа
MessageBox.Show("вы успешно авторизовались!", "оповещение");
if (user.password == "new_password")
{
    new ChangePassword(user).Show();
}

// Обновляем дату входа
user.enter_date = DateTime.Now;
usersTableAdapter.Update(user);

// Перенаправление на главную форму с ролью
new MainForm(user.role_id).Show();
this.Hide();
```

8. **Вход как гость**:
```csharp
private void guestLabel_Click(object sender, EventArgs e)
{
    int guest = 4; // Роль гостя
    new MainForm(guest).Show();
    this.Hide();
}
```
9. Перенаправление по ролям:
```csharp
private int _userRole;
            public Main(int userRole)
            {
                InitializeComponent();
                _userRole = userRole;
                if (_userRole == 1) //админ
                {
                    adminToolStripMenuItem.Visible = true;
                    servicesToolStripMenuItem.Visible = true;
                    bookingServicesToolStripMenuItem.Visible = true;
                    bookingsToolStripMenuItem.Visible = true;
                    guestsToolStripMenuItem.Visible = true;
                    shiftsToolStripMenuItem.Visible = true;
                    employeesToolStripMenuItem.Visible = true;
                }
                else if (_userRole == 2)//менеджер
                {
                    servicesToolStripMenuItem.Visible = true;
                    employeesToolStripMenuItem.Visible = true;
                    shiftsToolStripMenuItem.Visible = true;
                }
            }
```

## <a id="title2">Форма ChangePassword</a>
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


### Советы для запоминания:
1. **Порядок проверок**:
   - Пустые поля → Поиск пользователя → Статус блокировки → Дата последнего входа → Пароль → Успешная авторизация
2. **Ключевые методы**:
   - `FirstOrDefault()` для поиска пользователя
   - `Isenter_dateNull()` для проверки `NULL` в `enter_date`
   - `usersTableAdapter.Update()` для сохранения изменений в базе данных
   - `Hide()/Show()` для переключения форм
   - `error_counter` для подсчета ошибок
3. **Обратите внимание**:
   - `usersTableAdapter` — сгенерированный код для работы с таблицей `users`
   - `hotelDataSet` — название вашего DataSet
   - Столбец `login` имеет уникальное ограничение (`UNIQUE`) в базе данных
   - Значение `block_status` равно `"block"` при блокировке

### Быстрый чек-лист для воспроизведения:
1. Создать форму с:
   - 2 TextBox (`loginTextBox`, `passwordTextBox`)
   - 1 Button (`loginButton`)
   - 1 Label (`guestLabel`)
2. Добавить обработчик `loginButton_Click` с проверками:
   - Пустые поля
   - Существование пользователя
   - Статус блокировки
   - Дату последнего входа (более 30 дней)
   - Пароль и счетчик ошибок
3. Реализовать обновление `enter_date` и сохранение в базе данных
4. Добавить переход на `MainForm` с передачей `role_id` или значения для гостя
5. Добавить обработчик `guestLabel_Click` для входа как гость
6. Не забыть сброс `error_counter` при необходимости (например, в `Auth_Load`)

## <a id="title3">Форма пользователя (н-р Admin)</a>

### Гайд для запоминания кода (пошагово):
1. Структура формы:

- Главный элемент: MenuStrip (меню)

- Элемент меню: usersToolStripMenuItem (пункт "Пользователи")

- Контейнер: panel_admin (панель для отображения содержимого)

2. Обработчик клика по меню:

```csharp
private void usersToolStripMenuItem_Click(object sender, EventArgs e)
{
    // 1. Очищаем панель
    panel_admin.Controls.Clear();
    
    // 2. Создаем пользовательский контрол
    AdminUserControl adminUserControl = new AdminUserControl();
    
    // 3. Добавляем его на панель
    panel_admin.Controls.Add(adminUserControl);
}
```

### Советы для запоминания:
1. Запомните последовательность действий:

- Clear() → new Контрол() → Add()

2. Ключевые компоненты:

- panel_admin - зона для отображения контента

- AdminUserControl - ваша кастомная форма/контрол

3. Важные моменты:

- Всегда сначала очищайте панель (Clear())

- Контрол добавляется именно в Controls панели

### Быстрый чек-лист для воспроизведения:
1. Создать форму с:

- MenuStrip (меню)

- Panel (панель для контента)

2. Добавить в меню пункт (например, "Пользователи")

3. Создать обработчик клика:

```csharp
private void НазваниеПунктаМеню_Click(object sender, EventArgs e)
{
    panel_main.Controls.Clear();
    var control = new ВашКонтрол();
    panel_main.Controls.Add(control);
}
```
4. Привязать обработчик к пункту меню (через свойства в дизайнере)

## <a id="title4">UserControl</a>

### Гайд для запоминания кода (пошагово):

1. Инициализация и загрузка данных:
```csharp
 public UserControl1()
        {
            InitializeComponent();
            usersTableAdapter.Fill(hotelDataSet.users);
            rolesTableAdapter.Fill(hotelDataSet.roles);
        }
```
2. Логика сохранения:

```csharp
private void button_save_Click(object sender, EventArgs e)
{
    if (dataSet1.HasChanges()) // Если есть изменения
    {
        // Запрос подтверждения
        DialogResult dialogResult = MessageBox.Show(
            "Вы уверены, что хотите сохранить изменения?", 
            "Предупреждение", 
            MessageBoxButtons.YesNo);
            
        if (dialogResult == DialogResult.Yes)
        {
            usersTableAdapter.Update(dataSet1.users); // Сохранение
            MessageBox.Show("Данные успешно сохранены.");
        }
        else if (dialogResult == DialogResult.No)
        {
            usersTableAdapter.Fill(dataSet1.users); // Отмена изменений
        }
    }
    else if (!dataSet1.HasChanges()) // Если изменений нет
    {
        MessageBox.Show("Нет изменений для сохранения.");
    }
    else // Обработка ошибок
    {
        MessageBox.Show("Ошибка, обратитесь к администратору");
    }
}
```
или
```csharp
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
### Ключевые моменты для запоминания

1. Последовательность работы:

- Загрузка данных при инициализации (Fill)

- Проверка изменений (HasChanges)

- Подтверждение действия (MessageBox)

- Сохранение/отмена (Update/Fill)

2. Важные компоненты:

- usersTableAdapter - для работы с БД

- dataSet1 - локальное хранилище данных

- button_save - кнопка сохранения

3. Типовые проверки:

```csharp
if (dataSet1.HasChanges()) // Есть изменения
if (!dataSet1.HasChanges()) // Нет изменений
```
### Чек-лист для воспроизведения

1. Создайте UserControl с:

- Кнопкой сохранения (button_save)

- Элементами для отображения/редактирования данных

2. В конструкторе:

```csharp
usersTableAdapter.Fill(dataSet1.users);
```
3. В обработчике сохранения:

```csharp
// Проверка изменений
if (dataSet1.HasChanges())
{
    // Запрос подтверждения
    // Сохранение или отмена
}
```
4. Не забудьте:

- Обработку случая без изменений

- Обработку ошибок

## <a id="title5">Руководство пользователя</a>

### Структура руководства
**Введение**
- Название: [название системы]  
- Версия: 1.0  
- Предметная область: [такой-то бизнес]  

**1. Назначение системы (1 абзац)**
"Система позволяет [делать то, то и то]. Основные функции: [перечислить ключевые функции]."

**2. Инструкция (шаблонные фразы)**
- 2.1 Запуск:  
- Откройте [Название].sln → F5/кнопка Start  

- 2.2 Функции:  
- Аутентификация: Логин + пароль → "Войти"  
- Добавление, изменение и удаление пользователей:
- Функция 3:  

**4. Техподдержка**
- По вопросам обращайтесь к администратору системы.
- Что указать: [версию, описание ошибки]  

### Как запомнить структуру:
1. **Введение** (3 пункта: Название, Версия, Предметная область)  
2. **Назначение** (1 предложение о цели + функции)  
3. **Инструкция** (Запуск → 2-3 функции)  
4. **Поддержка** (куда писать и что указать)