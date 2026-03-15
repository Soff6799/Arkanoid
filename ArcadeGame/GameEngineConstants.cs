namespace ArcadeGame;

public static class GameEngineConstants
{
    //GameEngine
    /// <summary>
    /// Начальная ширина ракетки.
    /// </summary>
    public const int DefaultPaddleWidth = 100;
    
    /// <summary>
    /// Количество рядов блоков.
    /// </summary>
    public const int RowsOfArrayBlock = 3;
    
    /// <summary>
    /// Количество колонок блоков.
    /// </summary>
    public const int ColsOfArrayBlock = 8;
    
    /// <summary>
    /// Ширина одного блока.
    /// </summary>
    public const int BWidth = 65;
    
    /// <summary>
    /// Высота одного блока.
    /// </summary>
    public const int BHeight = 20;
    
    /// <summary>
    /// Отступ платформы от нижнего края экрана.
    /// </summary>
    public const int PaddleBottomOffset = 40;

    /// <summary>
    /// Минимальный отступ платформы от боковых стен.
    /// </summary>
    public const int PaddleHorizontalMargin = 20;
    
    /// <summary>
    /// Нижняя граница ("мертвая зона"),
    /// при пересечении которой теряется жизнь.
    /// </summary>
    public const int DeadZoneOffset = 40;
    
    /// <summary>
    /// Смещение сетки блоков по оси X.
    /// </summary>
    public const int GridOffsetX = 20;
    
    /// <summary>
    /// Смещение сетки блоков по оси Y.
    /// </summary>
    public const int GridOffsetY = 40;
    
    /// <summary>
    /// Расстояние между блоками.
    /// </summary>
    public const int BlockSpacing = 5;
   
    /// <summary>
    /// Начальная скорость мяча по вертикали
    /// (отрицательная — вверх).
    /// </summary>
    public const float InitialBallSpeedY = -5f;
    
    /// <summary>
    /// Начальная скорость мяча по горизонтали.
    /// </summary>
    public const float InitialBallSpeedX = 0f;
    
    /// <summary>
    /// Отступ верхней границы столкновения мяча.
    /// </summary>
    public const int TopWallOffset = 40;
    
    /// <summary>
    /// Коэффициент влияния скорости платформы на угол отскока мяча.
    /// </summary>
    public const float PaddleBounceInfluence = 0.15f;
    
    /// <summary>
    /// Интервал появления бонусов.
    /// </summary>
    public const int BonusSpawnInterval = 1250;
    
    /// <summary>
    /// Скорость падения бонуса.
    /// </summary>
    public const float BonusFallSpeed = 3f;
    
    /// <summary>
    /// Минимальная координата X для появления бонуса.
    /// </summary>
    public const int BonusSpawnMinX = 30; 
    
    /// <summary>
    /// Начальная координата Y для появления бонуса.
    /// </summary>
    public const int BonusSpawnY = 40;
    
    /// <summary>
    /// Длительность действия бонуса (в циклах обновления).
    /// </summary>
    public const int BonusEffectDuration = 400;
    
    /// <summary>
    /// Базовое количество очков за единицу здоровья блока.
    /// </summary>
    public const int PointsPerHealthUnit = 10;
}