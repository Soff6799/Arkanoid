namespace ArcadeGame;
using System;
using System.Windows.Forms;

/// <summary>
/// Главный класс приложения, отвечающий за точку входа в игру.
/// </summary>
internal static  class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new ArkanoidForm());
    }
}