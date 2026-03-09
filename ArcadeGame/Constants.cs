namespace ArcadeGame;
/// <summary>
/// Класс содержащий константы
/// </summary>
public static class Constants
{
    public const int DefoultScreenWidth = 640;   
    public const int DefoultScreenHeight = 480;
    public const int TimerIntervalMc = 20; // 50 кадров в секунду

    public const int DrawingBordersX = 20;
    public const int DrawingBordersY = 40;
    public const int BordersWidth = 5;
    
    public const int UISidebarPadding = 40;
    public const string GameFontFamily = "Segoe UI";
    public const float TitleFontSize = 18f;
    public const float StatusFontSize = 20f;

    public const int DistanceYName = 50;

    public const int NumberOfLives = 3;
    public const int WidthOfCirclesOfLives = 2;
    public const int DistanceOfLives = 110;
    public const int DistanceBetweenLives = 40;
    public const int LifeCircleDiameter = 25;
    
    public const int DistanceToGameSituation = 260;
    public const int StandardPlatformWidth = 100;
    
    //GameEngine
    public const int DefaultPaddleWidth = 100;
    public const int RowsOfArrayBlock = 3;
    public const int ColsOfArrayBlock = 8;
    public const int BWidth = 65;
    public const int BHeight = 20;
    
    public const int PaddleBottomOffset = 40;

    public const int PaddleHorizontalMargin = 20;
    public const int DeadZoneOffset = 40;
    
    public const int GridOffsetX = 20;
    public const int GridOffsetY = 40;
    public const int BlockSpacing = 5;
    
    public const float InitialBallSpeedY = -5f;
    public const float InitialBallSpeedX = 0f;
    
    public const int TopWallOffset = 40;
    public const float PaddleBounceInfluence = 0.15f;
    
    public const int BonusSpawnInterval = 1250;
    public const float BonusFallSpeed = 3f;
    public const int BonusSpawnMinX = 30; 
    public const int BonusSpawnY = 40;
    public const int BonusEffectDuration = 400; 
}