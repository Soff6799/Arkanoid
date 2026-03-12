namespace ArcadeGame;
/// <summary>
/// Класс содержащий константы
/// </summary>
public static class ConstantsForm
{
    /// <summary>
    /// Ширина экрана по умолчанию.
    /// </summary>
    public const int DefaultScreenWidth = 640;   
    
    /// <summary> Высота экрана по умолчанию. </summary>
    public const int DefaultScreenHeight = 480;
    
    /// <summary>
    /// Интервал обновления таймера в мс (соответствует 50 кадрам в секунду).
    /// </summary>
    public const int TimerIntervalMc = 20; 

    /// <summary>
    /// Отступ игрового поля по горизонтали при отрисовке рамки.
    /// </summary>
    public const int DrawingBordersX = 20;
    
    /// <summary>
    /// Отступ игрового поля по вертикали при отрисовке рамки.
    /// </summary>
    public const int DrawingBordersY = 40;
    
    /// <summary>
    /// Толщина линии рамки игрового поля.
    /// </summary>
    public const int BordersWidth = 5;
    
    /// <summary>
    /// Отступ боковой панели интерфейса.
    /// </summary>
    public const int UiSidebarPadding = 40;
    
    /// <summary>
    /// Семейство шрифтов, используемое в игре.
    /// </summary>
    public const string GameFontFamily = "Segoe UI";
    
    /// <summary>
    /// Размер шрифта для заголовков.
    /// </summary>
    public const float TitleFontSize = 18f;
    
    /// <summary>
    /// Размер шрифта для статусных надписей (победа/проигрыш).
    /// </summary>
    public const float StatusFontSize = 20f;

    /// <summary>
    /// Координата Y для отрисовки названия игры.
    /// </summary>
    public const int DistanceYName = 50;

    /// <summary>
    /// Начальное количество жизней игрока.
    /// </summary>
    public const int NumberOfLives = 3;
    
    /// <summary>
    /// Толщина обводки индикаторов жизней.
    /// </summary>
    public const int WidthOfCirclesOfLives = 2;
    
    /// <summary>
    /// Координата X для начала отрисовки индикаторов жизней.
    /// </summary>
    public const int DistanceOfLives = 110;
    
    /// <summary>
    /// Расстояние между иконками жизней.
    /// </summary>
    public const int DistanceBetweenLives = 40;
    
    /// <summary>
    /// Диаметр круга, обозначающего жизнь игрока.
    /// </summary>
    public const int LifeCircleDiameter = 25;
    
    /// <summary>
    /// Координата Y для вывода надписей о завершении игры.
    /// </summary>
    public const int DistanceToGameSituation = 260;
    
    /// <summary>
    /// Стандартная (базовая) ширина платформы.
    /// </summary>
    public const int StandardPlatformWidth = 100;
}