using System;

namespace Homework_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Загрузка из файла
            // Инициализация базы данных
            // Инициализация пользовательского интерфейса
            UserInterface ui = new UserInterface(FileDataBase.Load());

            // Показать справку по командам
            ui.ShowHelp();

            // Запустить главное меню
            ui.StartMenu();

        }
    }
}