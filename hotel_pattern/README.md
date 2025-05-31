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
2. [Форма пользователя (н-р Admin)](#title2)
3. [UserControl](#title3)
4. [Руководство пользователя](#title4)

## <a id="title1">Форма Auth</a>

### Гайд для запоминания кода (пошагово):
1. Структура формы:

- 2 TextBox: `textBox_login`, `textBox_password`

- 1 Button: `button_auth` с обработчиком `button_auth_Click`

2. Логика аутентификации:
```csharp
// Получаем данные из полей
string login = textBox_login.Text;
string password = textBox_password.Text;

// Проверка на пустые поля
if (login == "" || password == "")
{
    MessageBox.Show("Введите логин и пароль!");
    return;
}
```

3. Проверка пользователя:
```csharp
// Ищем пользователя в БД
var users = new usersTableAdapter().GetData()
    .FirstOrDefault(user => user.login == login);

if (users == null) // Если не найден
{
    MessageBox.Show("Пользователь не найден!");
    return;
}
```

4. Проверка пароля и счетчик ошибок:
```csharp
if (users.password != password)
{
    error_counter++; // Увеличиваем счетчик
    MessageBox.Show("Неверный пароль!");
    
    if (error_counter == 3) // Если 3 ошибки
    {
        MessageBox.Show("Слишком много попыток!");
        this.Hide(); // Закрываем форму
        return;
    }
    return;
}
```

5. Перенаправление по ролям:
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

### Советы для запоминания:
1. Запомните порядок проверок:

Пустые поля → Поиск пользователя → Проверка пароля → Проверка роли

2. Запомните ключевые методы:

- `FirstOrDefault()` для поиска пользователя

- `Hide()/Show()` для переключения форм

- `error_counter` для подсчета ошибок

3. Обратите внимание на:

- `usersTableAdapter` - это сгенерированный код для работы с БД

- `DataSet1` - название вашего DataSet

### Быстрый чек-лист для воспроизведения:
1. Создать форму с 2 TextBox и 1 Button

2. Добавить обработчик кнопки

3. Реализовать 4 этапа проверки (по порядку из п.1 в советах для запоминания)

4. Не забыть про счетчик ошибок

5. Добавить переходы на другие формы

## <a id="title2">Форма пользователя (н-р Admin)</a>

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

## <a id="title3">UserControl</a>

### Гайд для запоминания кода (пошагово):

1. Инициализация и загрузка данных:
```csharp
public AdminUserControl()
{
    InitializeComponent();
    usersTableAdapter.Fill(dataSet1.users); // Загрузка данных при создании
}
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

## <a id="title4">Руководство пользователя</a>

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
