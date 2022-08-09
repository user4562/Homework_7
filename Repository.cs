using System;
using System.Collections.Generic;

namespace Homework_7
{
    /// <summary>
    /// Класс базы данных, обрабатывает набор записей
    /// </summary>
    internal class Repository
    {
        /// <summary>
        /// Конструктор инициализирующий структуру базы
        /// из упакованных строк хранящихся в файле
        /// </summary>
        /// <param name="packItems">Массив строк из файла</param>
        public Repository(List<string> packItems, string path)
        {
            employees = packItems.ConvertAll<Worker>(n => new Worker(n));
  
            Path = path;
        }

        /// <summary>
        /// Конструктор инициализирющий базу из массива записей
        /// </summary>
        /// <param name="employees">Массив записей</param>
        public Repository(Worker[] employees, string path)
        {
            this.employees = new List<Worker>(employees);

            Path = path;
        }

        /// <summary>
        /// Набор записей, хранящихся в базе данных
        /// </summary>
        private List<Worker> employees;

        public readonly string Path;


        public Worker this[int index] => employees[index];


        /// <summary>
        /// Копирует элементы Worker в новый массив 
        /// </summary>
        /// <returns>Массив Worker</returns>
        public Worker[] ToArray() => employees.ToArray();

        /// <summary>
        /// Количество записей в базе
        /// </summary>
        public int Count => employees.Count;

        /// <summary>
        /// Возвращяет ближайший не занятый от нуля id 
        /// </summary>
        /// <returns>Ближайший не занятый от нуля id</returns>
        public int GetFreeId()
        {
            int[] ids = new int[Count];

            for (int i = 0; i < Count; i++)     // Берет все id в базе
                ids[i] = employees[i].Id;

            Array.Sort(ids);                  

            for (int i = 0; i < Count; i++)     // увеличивает счетчик на 1 и возвращяет тот
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
            return employees[IndexOfId(id)];
        }

        /// <summary>
        /// Возвращяет индекс записи в массиве записей по id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Индекс записи</returns>
        private int IndexOfId(int id)
        {
            for (int i = 0; i < Count; i++)
                if (employees[i].Id == id) return i;

            return -1;
        }

        /// <summary>
        /// Существует ли запись с определенным id
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>true - существует, false - не существует</returns>
        public bool IsExist(int id)
        {
            return IndexOfId(id) != -1;
        }

        /// <summary>
        /// Добавляет запись в базу
        /// </summary>
        /// <param name="newItem">Новая запись</param>
        public void Add(Worker worker)
        {
            employees.Add(worker);

            Save();
        }

        /// <summary>
        /// Удаляет запись из базы
        /// </summary>
        /// <param name="id">Id записи которую нужно удалить</param>
        public void Remove(int id)
        {
            employees.RemoveAt(IndexOfId(id));
            Save();
        }

        public void Edit(Worker worker)
        {
            if(IsExist(worker.Id))
            {
                employees[IndexOfId(worker.Id)] = worker;
            }
            else
            {
                Add(worker);
            }
        }

        /// <summary>
        /// Сортирует записи по дате добавления
        /// </summary>
        /// <param name="up">Направление сортировки</param>
        /// <param name="save">Сохранять или нет в файл</param>
        /// <returns>Возвращяет отсортированные записи</returns>
        public List<Worker> Sort(bool up, bool save)
        {
            List<Worker> newEmployees = employees;

            newEmployees.Sort(delegate(Worker w1, Worker w2) 
            {
                if (w1.DateAdded < w2.DateAdded) return -1;
                if (w1.DateAdded > w2.DateAdded) return 1;
                return 0;
            });

            if (!up)
            {
                newEmployees.Reverse();                 // Если направление сортировки обратное то разворачевает найденое
            }
            if (save)                                   // Созранение в файл
            {
                employees = newEmployees;
                Save();
            }

            return newEmployees;
        }

        /// <summary>
        /// Находит записи по датам добавления
        /// </summary>
        /// <param name="from">Дата с которой нужно начать поиск</param>
        /// <param name="to">Дата до которой нужно вести поиск</param>
        /// <returns>Записи удовлетворяющие поиску</returns>
        public List<Worker> FindByDates(DateTime from, DateTime to)
        {
            List<Worker> newEmployees = new List<Worker>(Count);

            if (from > to)                              // Если дата начала больше даты конца поиска
            {                                           // функция меняет их местами
                DateTime temp = from;
                from = to;
                to = temp;
            }

            newEmployees = employees.FindAll(delegate (Worker worker)
            {
                return worker.DateAdded > from && worker.DateAdded < to;
            });

            return newEmployees;
        }

        /// <summary>
        /// Сохраняет
        /// </summary>
        private void Save()
        {
            FileRepository.Save(this);
        }
    }
}





