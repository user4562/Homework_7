using System;
using System.IO;

namespace Homework_7
{
    /// <summary>
    /// Структура со статичесикми методами для комуникации с файлом базы данных
    /// </summary>
    internal struct FileRepository
    {
        /// <summary>
        /// Путь к базе данных
        /// </summary>
        private static string FilePath = "DataBase.txt";

        /// <summary>
        /// Метод загрузки базы данных из файла
        /// </summary>
        /// <returns>Возвращяет структуру базы данных</returns>
        public static Repository Load()
        {
            Check();
            int countItems = 0;
            string[] lines = File.ReadAllLines(FilePath);
            string[] items = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)                  // Удаление всех пустых строк
            {
                if (lines[i].Trim() == string.Empty) continue;
                items[countItems] = lines[i];
                countItems++;
            }

            Array.Resize(ref items, countItems);

            return new Repository(items);
        }

        /// <summary>
        /// Метод сохранения базы данных в файл
        /// </summary>
        /// <param name="dataBase">Структура базы данных</param>
        public static void Save(ref Repository dataBase)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (Worker emplo in dataBase.Employees)
                    sw.WriteLine(emplo.ToPackString());
            }
        }


        /// <summary>
        /// Проверяет существует для файл, если нет - создает его
        /// </summary>
        private static void Check()
        {
            if (!File.Exists(FilePath)) File.Create(FilePath).Close();
        }
    }
}
