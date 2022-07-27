using System;

namespace Homework_7
{
    /// <summary>
    /// Структура базы данных, обрабатывает набор записей
    /// </summary>
    internal struct Repository
    {
        /// <summary>
        /// Набор записей, хранящихся в базе данных
        /// </summary>
        public Worker[] Employees { get; private set; }

        /// <summary>
        /// Конструктор инициализирующий структуру базы
        /// из упакованных строк хранящихся в файле
        /// </summary>
        /// <param name="packItems">Массив строк из файла</param>
        public Repository(string[] packItems)
        {
            Employees = new Worker[packItems.Length];

            for (int i = 0; i < Count; i++)
            {
                Employees[i] = new Worker(packItems[i]);
            }
        }

        /// <summary>
        /// Конструктор инициализирющий базу из массива записей
        /// </summary>
        /// <param name="employees">Массив записей</param>
        public Repository(Worker[] employees)
        {
            Employees = employees;
        }

        /// <summary>
        /// Количество записей в базе
        /// </summary>
        public int Count
        {
            get { return Employees.Length; }
        }

        /// <summary>
        /// Возвращяет ближайший не занятый id 
        /// </summary>
        /// <returns>Ближайший не занятый id</returns>
        public int GetFreeId()
        {
            int[] ids = new int[Count];

            for (int i = 0; i < Count; i++)     // Берет все id в базе
                ids[i] = Employees[i].Id;

            Array.Sort(ids);                    // сортирует их по возрастанию

            for (int i = 0; i < Count; i++)     // увеличивает счетчик на 1 и и возвращяет тот
                if (ids[i] != i) return i;      // который не совпадает со счетчиком

            return Count;
        }

        /// <summary>
        /// Возвращяет структуру записи по ее id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Запись с указаным id</returns>
        public Worker ItemOfId(int id)
        {
            return Employees[IndexOfId(id)];
        }

        /// <summary>
        /// Возвращяет индекс записи в массиве записей по id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Индекс записи</returns>
        public int IndexOfId(int id)
        {
            for (int i = 0; i < Count; i++)
                if (Employees[i].Id == id) return i;

            return -1;
        }

        /// <summary>
        /// Существует ли запись с определенным id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>true - существует, false - не существует</returns>
        public bool ExistItem(int id)
        {
            return IndexOfId(id) != -1;
        }

        /// <summary>
        /// Добавляет запись в базу
        /// </summary>
        /// <param name="newItem">Новая запись</param>
        public void Added(Worker newItem)
        {
            Worker[] newEmp = new Worker[Employees.Length + 1];

            if (newEmp.Length == 1)
            {
                newEmp[0] = newItem;
            }
            else
            {
                Employees.CopyTo(newEmp, 0);
                newEmp[Employees.Length] = newItem;
            }

            Employees = newEmp;
            FileRepository.Save(ref this);
        }

        /// <summary>
        /// Удаляет запись из базы
        /// </summary>
        /// <param name="id">Id записи которую нужно удалить</param>
        public void Delete(int id)
        {
            Worker[] newEmp = new Worker[Count - 1];

            for (int i = 0, j = 0; i < Count; i++)
            {
                if (Employees[i].Id != id)
                {
                    newEmp[j] = Employees[i];
                    j++;
                }
            }

            Employees = newEmp;
            FileRepository.Save(ref this);
        }

        /// <summary>
        /// Сортирует записи по дате добавления
        /// </summary>
        /// <param name="up">Направление сортировки</param>
        /// <param name="save">Сохранять или нет в файл</param>
        /// <returns>Возвращяет отсортированные записи</returns>
        public Worker[] Sort(bool up, bool save)
        {
            Worker[] newEmp = Employees;
            Worker temp;

            for (int i = 0; i < Count - 1; i++)         // Сортирует
            {
                for (int j = i + 1; j < Count; j++)
                {
                    if (newEmp[i].DateAdded > newEmp[j].DateAdded)
                    {
                        temp = newEmp[i];
                        newEmp[i] = newEmp[j];
                        newEmp[j] = temp;
                    }
                }
            }

            if (!up) Array.Reverse(Employees);          // Если направление сортировки обратное то разворачевает найденое
            if (save)                                   // Созранение в файл
            {
                Employees = newEmp;
                FileRepository.Save(ref this);
            }

            return newEmp;
        }

        /// <summary>
        /// Находит записи по датам добавления
        /// </summary>
        /// <param name="from">Дата с которой нужно начать поиск</param>
        /// <param name="to">Дата до которой нужно вести поиск</param>
        /// <returns>Записи удовлетворяющие поиску</returns>
        public Worker[] FindByDates(DateTime from, DateTime to)
        {
            Worker[] newEmp = new Worker[Count];

            if (from > to)                              // Если дата начала больше даты конца поиска
            {                                           // функция меняет их местами
                DateTime temp = from;
                from = to;
                to = temp;
            }

            int count = 0;
            for (int i = 0; i < Count; i++)             // Ищет удовлетворяющие датам
            {
                if (Employees[i].DateAdded > from && Employees[i].DateAdded < to)
                {
                    newEmp[count] = Employees[i];
                    count++;
                }
            }

            Array.Resize(ref newEmp, count);

            return newEmp;
        }

    }
}





