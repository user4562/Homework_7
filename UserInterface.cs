using System;

namespace Homework_7
{

    /// <summary>
    /// Структура реализующяя пользовательскй интерфейс 
    /// </summary>
    internal struct UserInterface
    {
        /// <summary>
        /// База данных которую будет обробатывать этот пользовательский интерфейс
        /// </summary>
        private DataBase context;

        /// <summary>
        /// Константы имен команд 
        /// </summary>
        private const string showAllCommand = "show-all";
        private const string showAll2Command = "show all";
        private const string showCommand = "show";
        private const string countCommand = "count";
        private const string createCommand = "create";
        private const string deleteCommand = "delete";
        private const string delCommand = "del";
        private const string editCommand = "edit";
        private const string filterDatesCommand = "filter-dates";
        private const string filterDates2Command = "filter dates";
        private const string sortCommand = "sort";
        private const string helpCommand = "help";
        private const string exitCommand = "exit";


        /// <summary>
        /// Константы сообщения ошибок для функции ErrorMessage
        /// </summary>
        private const string errorCommandMessage = "Команда введена не верно.";
        private const string errorAgrumentMessage = "Параметр указан не верно.";
        private const string errorIdExsistMessage = "Нет записи с таким id.";
        private const string errorSearchMessage = "Нет элементов удовлетворяющих поиску.";
        private const string errorCountArgumentMessage = "Не правильное количество элементов";
        private const string errorDateMessage = "Дата указана не верно";
        private const string errorIdLessZiroMessage = "Id не может быть меньше нуля";

        /// <summary>
        /// Коструктор принимает в качестве параметра структуру базы данных
        /// к которой и будет применен этот пользовательский интерфейс
        /// </summary>
        /// <param name="context"></param>
        public UserInterface(DataBase context)
        {
            this.context = context;

            Console.WriteLine("Homeworkd 7.");
            Console.WriteLine("Программа для чтения и записи в файл.");
            Console.WriteLine();
        }

        /// <summary>
        /// Показывает справку по командам
        /// </summary>
        public void ShowHelp()
        {
            Console.WriteLine($"1/{showAllCommand} \t\t\tПоказать все записи.");
            Console.WriteLine($"2/{showCommand} [id] \t\t\tПоказать запись по id.");
            Console.WriteLine($"3/{countCommand} \t\t\tПоказать количество записей в базе.");
            Console.WriteLine($"4/{createCommand} \t\t\tСоздать новую запись.");
            Console.WriteLine($"5/{deleteCommand} [id] \t\t\tУдалить запись по id.");
            Console.WriteLine($"6/{editCommand} [id] \t\t\tИзменить запись по id.\n");

            Console.WriteLine($"7/{filterDatesCommand} [date1 date2] \tПоказать записи в диапазоне дат.");
            Console.WriteLine("\t\t\t\tdate1 - верхний предел диапазона.");
            Console.WriteLine("\t\t\t\tdate2 - нижний предел диапазона.\n");

            Console.WriteLine($"8/{sortCommand} [up|down] [save] " +
                                        "\tСортировка записей по датам добавления.");
            Console.WriteLine("\t\t\t\tup - сортировка от большего к меньшему (default).");
            Console.WriteLine("\t\t\t\tdown - сортировка от меньшего к большему.");
            Console.WriteLine("\t\t\t\tsave - с сохранением результата в файл.");
            Console.WriteLine("\t\t\t\tбез параметров: от большего к меньшему, без сохранения\n");


            Console.WriteLine($"9/{helpCommand} \t\t\t\tПоказать справку по командам.");
            Console.WriteLine($"0/{exitCommand} \t\t\t\tВыход из программы.");
            Console.WriteLine();
        }

        /// <summary>
        /// Запускает главное меню
        /// </summary>
        public void StartMenu()
        {
            string input;
            while (true)                                        // Запускается главный цыкл
            {
                Console.Write("menu>");
                input = Console.ReadLine().ToLower().Trim();    // Принимается команда

                if (string.IsNullOrEmpty(input)) continue;      // Если строка пустая
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                    case showAllCommand:
                    case showAll2Command:
                        ShowAll(); break;                       // Команда: показать все записи
                    case "2":
                    case showCommand:
                        ShowItem(null); break;                  // Команда: показать запись по id
                    case "3":
                    case countCommand:
                        ShowCount(); break;                     // Команда: показать количество записей
                    case "4":
                    case createCommand:
                        CreateItem(); break;                    // Команда: создать запись 
                    case "5":
                    case deleteCommand:
                    case delCommand:
                        DeleteItem(null); break;                // Команда: удалить запись
                    case "6":
                    case editCommand:
                        EditItem(null); break;                  // Команда: изменить запись
                    case "7":
                    case filterDatesCommand:
                    case filterDates2Command:
                        ShowByDates(null); break;               // Команда: показать записи по диапазону дат
                    case "8":
                    case sortCommand:
                        Sort(null); break;                      // Команда: сортировать
                    case "9":
                    case helpCommand:
                        ShowHelp(); break;                      // Команда: показать справку
                    case "0":
                    case exitCommand: return;                   // Команда: выход

                    default:
                        if (input.StartsWith(showCommand)) ShowItem(input);                 // Показать запись по id
                        else if (input.StartsWith(deleteCommand)) DeleteItem(input);        // Удалить запись по id
                        else if (input.StartsWith(delCommand)) DeleteItem(input);
                        else if (input.StartsWith(editCommand)) EditItem(input);            // Изменить запись по id
                        else if (input.StartsWith(filterDatesCommand)) ShowByDates(input);  // Показать записи по датам
                        else if (input.StartsWith(filterDates2Command)) ShowByDates(input);
                        else if (input.StartsWith(sortCommand)) Sort(input);                // Сортировать записи
                        else
                            ErrorMessage(errorCommandMessage);

                        break;
                }
            }
        }

        /// <summary>
        /// Показать все записи в базе 
        /// </summary>
        public void ShowAll()
        {
            if (context.Count == 0)                      // Если нет записей
            {
                Console.WriteLine("Нет записей.");
                Console.WriteLine();
                return;
            }

            ShowCount();                                // Показать количество

            for (int i = 0; i < context.Count; i++)
            {
                Console.WriteLine(context.Employees[i].ToShowString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Показать запись по id
        /// </summary>
        /// <param name="itemId">Строка с id</param>
        public void ShowItem(string itemId)
        {
            int id;

            DelNameCommand(ref itemId, showCommand);
            if (!GetId(itemId, out id)) return;

            Console.WriteLine(context.ItemOfId(id).ToShowString());
            Console.WriteLine();
        }

        /// <summary>
        /// Показать количество записей в базе
        /// </summary>
        public void ShowCount()
        {
            Console.WriteLine($"Всего записей: {context.Count}");
            Console.WriteLine();
        }


        /// <summary>
        /// Показать записи в диапазоне дат
        /// </summary>
        /// <param name="datesParam">Строка с двумя датами</param>
        public void ShowByDates(string datesParam)
        {
            DateTime from, to;

            if (datesParam == null)
            {
                string input;

                while (true)                                                // Получение первой даты
                {
                    Console.WriteLine("Введите дату с которой нужно начать поиск.");
                    Console.Write("date min: ");

                    input = Console.ReadLine().Trim();
                    Console.WriteLine();

                    if (string.IsNullOrEmpty(input)) return;
                    if (DateTime.TryParse(input, out from)) break;

                    ErrorMessage(errorDateMessage);
                }

                while (true)                                                // Получение второй даты
                {
                    Console.WriteLine("Введите дату до которой нужно продолжать поиск.");
                    Console.Write("date max: ");

                    input = Console.ReadLine().Trim();
                    Console.WriteLine();

                    if (string.IsNullOrEmpty(input)) return;
                    if (DateTime.TryParse(input, out to)) break;

                    ErrorMessage(errorDateMessage);
                }
            }
            else
            {
                DelNameCommand(ref datesParam, filterDatesCommand);         // Удаление из строки имени команды
                DelNameCommand(ref datesParam, filterDates2Command);

                if (datesParam.Split(' ').Length != 2)
                {
                    ErrorMessage(errorCountArgumentMessage);
                    return;
                }

                string input;

                input = datesParam.Substring(0, datesParam.IndexOf(' '));   // Получение первой даты

                if (!DateTime.TryParse(input, out from))
                {
                    ErrorMessage(errorDateMessage);
                    return;
                }

                input = datesParam.Substring(datesParam.LastIndexOf(' '));  // Получение второй даты

                if (!DateTime.TryParse(input, out to))
                {
                    ErrorMessage(errorDateMessage);
                    return;
                }
            }

            Employee[] employees = context.FindByDates(from, to);

            if (employees.Length == 0)
            {
                ErrorMessage(errorSearchMessage);
            }
            else
            {
                Console.WriteLine("Элементы удовлетворяющие поиску.");
                Console.WriteLine();
                Console.WriteLine($"Всего найдено: {employees.Length}.");
                Console.WriteLine();

                ShowItems(employees);
            }
        }


        /// <summary>
        /// Удаляет запись из бызы
        /// </summary>
        /// <param name="itemId">Id записи</param>
        public void DeleteItem(string itemId)
        {
            int id;

            DelNameCommand(ref itemId, deleteCommand);
            DelNameCommand(ref itemId, delCommand);
            if (!GetId(itemId, out id)) return;

            context.Delete(id);

            Console.WriteLine("Элемент удален.");
            Console.WriteLine();
        }

        /// <summary>
        /// Метод для сортировки записей по времени добавления их в базу.
        /// Если ввести команду без параметров то сортировка будет по стандартным
        /// параметрам (от большего к меньшему, без сохранения). 
        /// up - сортировка от большего к меньшему
        /// down - сортировка от меньшего к большему
        /// save - с сохранением результата в файл
        /// </summary>
        /// <param name="sortParam"></param>
        public void Sort(string sortParam)
        {
            bool trend = true;
            bool save = false;

            if (sortParam != null)
            {
                DelNameCommand(ref sortParam, sortCommand);                 // Удаление из строки имени команды

                int indexSaver = sortParam.IndexOf("save");

                if (indexSaver != -1)                                        // Проверка на наличие "save" в команде
                {
                    save = true;
                    sortParam = sortParam.Remove(indexSaver, 4).Trim();
                }

                if (sortParam == "down")                                     // Проверка на направление сортировки
                {
                    trend = false;
                }
                else if (sortParam != "up")
                {
                    ErrorMessage(errorAgrumentMessage);
                    return;
                }
            }

            ShowCount();
            ShowItems(context.Sort(trend, save));
        }

        /// <summary>
        /// Метод для создания записи и добавления ее в базу
        /// </summary>
        public void CreateItem()
        {
            string fullName;
            int age;
            int height;
            DateTime dateOfBirth;
            string placeOfBirth;

            while (true)                                           // Инициализация полного имени
            {
                Console.Write("Ф.И.О.: ");
                fullName = Console.ReadLine();

                if (!string.IsNullOrEmpty(fullName)) break;

                Console.WriteLine();
                ErrorMessage("Имя не может быть пустым");
            }

            while (true)                                            // Инициализация возраста
            {
                Console.Write("Возраст: ");

                if (int.TryParse(Console.ReadLine(), out age))
                {
                    if (age > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        ErrorMessage("Возраст не может быть меньше или равен нулю.");
                    }
                }
                else
                {
                    Console.WriteLine();
                    ErrorMessage("Возраст указан не верно.");
                }
            }

            while (true)                                            // Инициализация роста
            {
                Console.Write("Рост: ");
                if (int.TryParse(Console.ReadLine(), out height))
                {
                    if (height > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        ErrorMessage("Рост не может быть меньше или равен нулю.");
                    }
                }
                else
                {
                    Console.WriteLine();
                    ErrorMessage("Рост указан не верно.");
                }
            }

            while (true)                                            // Инициализация даты рождения
            {
                Console.Write("Дата рождения: ");
                if (DateTime.TryParse(Console.ReadLine(), out dateOfBirth)) break;

                Console.WriteLine();
                ErrorMessage("Дата рождения указана не верно");
            }

            while (true)                                            // Инициализация места рождения
            {
                Console.Write("Место рождения: ");
                placeOfBirth = Console.ReadLine();

                if (!string.IsNullOrEmpty(placeOfBirth)) break;

                Console.WriteLine();
                ErrorMessage("Место рождения не может быть пустым");

            }

            int id = Employee.Create(ref context, fullName, age, height, dateOfBirth, placeOfBirth);

            Console.WriteLine();
            Console.WriteLine($"Новая запись c id {id} создана.");
            Console.WriteLine();
        }

        /// <summary>
        /// Изменение записи.
        /// Метод спрашивает у пользователя новые значения + показывает старые.
        /// Если пользователь пропустит ввод (нажмет Enter не ведя значения) 
        /// по какому либо значению то оно не будет измененно.
        /// </summary>
        /// <param name="itemId">id записи которую нужно изменить</param>
        public void EditItem(string itemId)
        {
            int id;

            DelNameCommand(ref itemId, editCommand);
            if (!GetId(itemId, out id)) return;                             // Получение id из строки ввода

            int index = context.IndexOfId(id);                              // Находит запись по id для изменения

            string fullName;                                                // Если пользователь не ввел значения
            int age;                                                        // параметру то ему будет присвоенно
            int height;                                                     // значение сигнализирующее функции
            DateTime dateOfBirth;                                           // Employee.Edit что этот параметр 
            string placeOfBirth;                                            // записи не следует изменять
                                                                            // string (null), int (-1),
            string temp;                                                    // DateTime (DateTime.MinValue)

            Console.Write($"({context.Employees[index].FullName}) Ф.И.О.: ");                // Получение полного имени
            fullName = Console.ReadLine();
            if (string.IsNullOrEmpty(fullName))
                fullName = null;

            Console.Write($"({context.Employees[index].Age}) Возраст: ");                    // Получение возраста
            temp = Console.ReadLine();
            if (string.IsNullOrEmpty(temp))
                age = -1;
            else if (!int.TryParse(temp, out age))
                age = -1;

            Console.Write($"({context.Employees[index].Height}) Рост: ");                    // Получение роста
            temp = Console.ReadLine();
            if (string.IsNullOrEmpty(temp))
                height = -1;
            else if (!int.TryParse(temp, out height))
                height = -1;

            Console.Write($"({context.Employees[index].DateOfBirth}) Дата рождения: ");      // Получение даты рождения
            temp = Console.ReadLine();
            if (string.IsNullOrEmpty(temp))
                dateOfBirth = DateTime.MinValue;
            else if (!DateTime.TryParse(temp, out dateOfBirth))
                dateOfBirth = DateTime.MinValue;

            Console.Write($"({context.Employees[index].PlaceOfBirth}) Место рождения: ");    // Получение места рождения
            placeOfBirth = Console.ReadLine();
            if (string.IsNullOrEmpty(placeOfBirth))
                placeOfBirth = null;

            Console.WriteLine();

            context.Employees[index].Edit(fullName, age, height, dateOfBirth, placeOfBirth);


            Console.WriteLine($"Запись с id {id} изменена.");
            Console.WriteLine();

        }

        /// <summary>
        /// Показывает сообщение ошибка
        /// </summary>
        /// <param name="message">Сообщение ошибки</param>
        private void ErrorMessage(string message)               // Цвет сообщения темно-желтый    
        {
            ConsoleColor userColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.WriteLine();
            Console.ForegroundColor = userColor;
        }

        /// <summary>
        /// Показывает по одному все записи в массиве записей
        /// </summary>
        /// <param name="items">Массив записей</param>
        private void ShowItems(Employee[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine(items[i].ToShowString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Удаляет из строки ввода имя команды
        /// </summary>
        /// <param name="fullInput">Полная строка ввода</param>
        /// <param name="nameCommand">Имя команда</param>
        private void DelNameCommand(ref string fullInput, string nameCommand)
        {
            if (fullInput == null) return;
            if (fullInput.IndexOf(nameCommand) != -1)
                fullInput = fullInput.Substring(nameCommand.Length).Trim();
        }

        /// <summary>
        /// Преобразует строку ввода в id 
        /// </summary>
        /// <param name="input">Строка ввода</param>
        /// <param name="id">Id для инициализации</param>
        /// <returns>Получен ли id</returns>
        private bool GetId(string input, out int id)
        {
            id = -1;

            if (input == null)                                  // Если строка ввода равна null то она берется
            {
                return GetIdFromUser(out id);                   // от пользователя при помощи GetIdFromUser
            }
            else
            {
                if (!int.TryParse(input, out id))               // Проверка на корректность ввода
                {
                    ErrorMessage(errorAgrumentMessage);
                    return false;
                }

                if (id < 0)                                     // Проверка на знак числа
                {
                    ErrorMessage(errorIdLessZiroMessage);
                    return false;
                }

                else if (!context.ExistItem(id))                // Проверка есть ли запись с таким id в базе
                {
                    ErrorMessage(errorIdExsistMessage);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Получает ID от пользователя
        /// </summary>
        /// <param name="id">переменная с id для инициализации</param>
        /// <returns>Получен ли id</returns>
        private bool GetIdFromUser(out int id)
        {
            string input;
            id = -1;

            while (true)                                        // Цыкл завершиться только если пользователь
            {                                                   // введет коректный id 
                Console.Write("Введите ID:");
                input = Console.ReadLine().Trim();
                Console.WriteLine();

                if (string.IsNullOrEmpty(input)) return false;  // либо введет пустую строку

                if (GetId(input, out id)) return true;          // Для проверки на коректность вызывается GetId
            }
        }

    }
}
