using System;

namespace Homework_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "dataBase.txt";
            // Загрузка из файла
            // Инициализация базы данных
            // Инициализация пользовательского интерфейса
            UserInterface ui = new UserInterface(FileRepository.Load(path));

            // Показать справку по командам
            ui.ShowHelp();

            // Запустить главное меню
            ui.StartMenu();

        }
    }
}