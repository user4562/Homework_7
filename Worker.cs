using System;


namespace Homework_7
{
    /// <summary>
    /// Структура записи, харнит в себе все свойства записи и 
    /// некоторые функии для их обработки.
    /// Новая запись может быть создана только в базе данных
    /// </summary>
    internal struct Worker
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Дата добавления записи
        /// </summary>
        public DateTime DateAdded { get; }

        /// <summary>
        /// Полное имя
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; private set; }

        /// <summary>
        /// Рост
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string PlaceOfBirth { get; private set; }


        /// <summary>
        /// Приватный конструктор для инициализации уже созданых записей из файла
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <param name="dateAdded">Дата добавления записи</param>
        /// <param name="fullName">Полное имя</param>
        /// <param name="age">Возраст</param>
        /// <param name="height">Рост</param>
        /// <param name="dateOfBirth">Дата рождения</param>
        /// <param name="placeOfBirth">Место рождения</param>
        private Worker(int id, DateTime dateAdded, string fullName, int age,
                    int height, DateTime dateOfBirth, string placeOfBirth)
        {
            Id = id;
            DateAdded = dateAdded;

            FullName = fullName;
            Age = age;
            Height = height;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
        }

        /// <summary>
        /// Конструктор инициалихирует струтуру из упакованной строки
        /// хранящейся в файле
        /// </summary>
        /// <param name="packItem">Упакованная строка из файла</param>
        public Worker(string packItem)
        {
            string[] data = packItem.Split('#');

            Id = int.Parse(data[0]);
            DateAdded = DateTime.Parse(data[1]);

            FullName = data[2];
            Age = int.Parse(data[3]);
            Height = int.Parse(data[4]);
            DateOfBirth = DateTime.Parse(data[5]);
            PlaceOfBirth = data[6];
        }

        /// <summary>
        /// Упаковывает структуру в строку в специальном формате
        /// для записи в файл
        /// </summary>
        /// <returns>Строка для записи в файл</returns>
        public string ToPackString()
        {
            return $"{Id}#{DateAdded}#{FullName}#{Age}#" +
                   $"{Height}#{DateOfBirth}#{PlaceOfBirth}";
        }

        /// <summary>
        /// Формирует строку для отображения в консоле
        /// </summary>
        /// <returns>Строка для отображения в консоле</returns>
        public string ToShowString()
        {
            return $"ID:\t\t\t{Id}\n" +
                   $"Время добавления:\t{DateAdded}\n" +
                   $"Ф.И.О.:\t\t\t{FullName}\n" +
                   $"Возраст:\t\t{Age}\n" +
                   $"Рост:\t\t\t{Height}\n" +
                   $"Дата рождения:\t\t{DateOfBirth.ToShortDateString()}\n" +
                   $"Место рождения:\t\t{PlaceOfBirth}";
        }

        /// <summary>
        /// Изменяет запись.
        /// Если в аргументах указать определенные значения то эти свойства 
        /// не будут изменены:
        /// для string = null
        /// для int = -1
        /// для DateTime = DateTime.MinValue
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <param name="age">Возраст</param>
        /// <param name="height">Рост</param>
        /// <param name="dateOfBirth">Дата рождения</param>
        /// <param name="placeOfBirth">Место рождения</param>
        public void Edit(string fullName, int age,
                    int height, DateTime dateOfBirth, string placeOfBirth)
        {
            if (fullName != null) FullName = fullName;
            if (age != -1) Age = age;
            if (height != -1) Height = height;
            if (dateOfBirth != DateTime.MinValue) DateOfBirth = dateOfBirth;
            if (placeOfBirth != null) PlaceOfBirth = placeOfBirth;
        }

        /// <summary>
        /// Метод для создания новой записи
        /// Новую запись можно создать только в базе даных
        /// </summary>
        /// <param name="context">Контекст базы данных в которой создается новая запись</param>
        /// <param name="fullName">Полное имя</param>
        /// <param name="age">Возраст</param>
        /// <param name="height">Рост</param>
        /// <param name="dateOfBirth">Дата рождения</param>
        /// <param name="placeOfBirth">Место рождения</param>
        /// <returns>Возращяет Id только что созданной записи</returns>
        public static int Create(ref Repository context, string fullName, int age,
                    int height, DateTime dateOfBirth, string placeOfBirth)
        {
            Worker newEmployee = new Worker(context.GetFreeId(), DateTime.Now, fullName,
                                    age, height, dateOfBirth, placeOfBirth);

            context.Add(newEmployee);
            return newEmployee.Id;
        }
    }
}
