using System.IO;
using System.Collections.Generic;

namespace Homework_7
{
    /// <summary>
    /// Статический класс комуникации с файлом базы данных
    /// </summary>
    internal static class FileRepository
    {
        /// <summary>
        /// Метод загрузки базы данных из файла
        /// </summary>
        /// <returns>Возвращяет класс базы данных</returns>
        public static Repository Load(string path)
        {
            if (File.Exists(path)) return new Repository(new List<string>(), path);

            string[] lines = File.ReadAllLines(path);
            List<string> items = new List<string>(lines.Length);

            foreach(string line in lines)                  // Удаление всех пустых строк
            {
                if (line.Trim() == string.Empty) continue;
                items.Add(line);
            }

            return new Repository(items, path);
        }

        /// <summary>
        /// Метод сохранения базы данных в файл
        /// </summary>
        /// <param name="dataBase">Класс базы данных</param>
        public static void Save(Repository dataBase)
        {
            using (StreamWriter sw = new StreamWriter(dataBase.Path))
            {
                foreach (Worker emplo in dataBase.ToArray())
                    sw.WriteLine(emplo.ToPackString());
            }
        }
    }
}
