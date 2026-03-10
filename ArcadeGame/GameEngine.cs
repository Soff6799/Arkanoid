namespace ArcadeGame;
using System;
using System.Collections.Generic; 

public class GameEngine
{
    /// <summary>
    /// Текущее количество очков игрока.
    /// </summary>
    public int Score { get; set; }
    /// <summary>
    /// Максимально возможное количество очков за полное уничтожение всех блоков.
    /// </summary>
    public int MaxScore { get; }
    /// <summary>
    /// Координата X центра мяча
    /// </summary>
    public float BallX { get; set; }
    /// <summary>
    /// Координата Y центра мяча.
    /// </summary>
    public float BallY { get; set; }
    /// <summary>
    /// Скорость мяча по оси X (горизонтальная).
    /// </summary>
    public float BallVX { get; set; }
    /// <summary>
    /// Скорость мяча по оси Y (вертикальная).
    /// </summary>
    public float BallVY { get; set; }
    /// <summary>
    /// Размер (диаметр) мяча.
    /// </summary>
    public int BallSize { get; } = 12; 
    /// <summary>
    /// Флаг, указывающий, запущена ли игра (мяч движется).
    /// </summary>
    public bool IsStarted { get; set; } 
    /// <summary>
    /// Координата X верхнего левого угла платформы.
    /// </summary>
    public float PaddleX { get; set; }
    /// <summary>
    /// Координата Y верхнего левого угла платформы.
    /// </summary>
    public float PaddleY { get; }
    /// <summary>
    /// Ширина платформы.
    /// </summary>
    public int PaddleWidth { get; set; } = 100;
    /// <summary>
    /// Высота платформы.
    /// </summary>
    public int PaddleHeight { get; } = 15;
    /// <summary>
    /// Ширина игрового поля (без отступов).
    /// </summary>
    public int GameWidth { get; }
    /// <summary>
    /// Высота игрового поля (без отступов).
    /// </summary>
    public int GameHeight { get; }
    /// <summary>
    /// Список всех блоков на игровом поле.
    /// </summary>
    public List<Block> Blocks { get; }
    /// <summary>
    /// Количество жизней игрока.
    /// </summary>
    public int Lives { get; set; } = 3;
    /// <summary>
    /// Флаг, указывающий, завершена ли игра (проигрыш).
    /// </summary>
    public bool GameOver { get; set; }
    /// <summary>
    /// Флаг, указывающий, выиграл ли игрок.
    /// </summary>
    public bool GameWon { get; set; } 
    /// <summary>
    /// Координата X выпадающего бонуса.
    /// </summary>
    public float BonusX { get; set; }
    /// <summary>
    /// Координата Y выпадающего бонуса.
    /// </summary>
    public float BonusY { get; set; }
    /// <summary>
    /// Флаг, указывающий, активен ли бонус (падает или находится на платформе).
    /// </summary>
    public bool IsBonusActive { get; set; } 
    /// <summary>
    /// Размер (диаметр) бонуса.
    /// </summary>
    public int BonusSize { get; set; } = 20;

    private int healthNow = 3;
    private int bonusSpawnTimer;
    private int bonusEffectTimer;
    private readonly Random random = new Random();
    
    /// <summary>
    /// Конструктор движка игры. Инициализирует размеры поля, создает список блоков
    /// и рассчитывает максимальное количество очков.
    /// </summary>
    public GameEngine(int width, int height)
    {
        GameWidth = width;
        GameHeight = height;
        Blocks = new List<Block>();
        MaxScore = 0;
        for (var i = 0; i < Constants.RowsOfArrayBlock; i++)
        {
            for (var j = 0; j < Constants.ColsOfArrayBlock; j++)
            {
                int blockPositionX = Constants.PaddleHorizontalMargin + Constants.GridOffsetX + j * (Constants.BWidth+Constants.BlockSpacing);
                int blockPositionY = Constants.PaddleHorizontalMargin + Constants.GridOffsetY + i * (Constants.BHeight+Constants.BlockSpacing);
                int healthResult = healthNow - i;
                Blocks.Add(new Block(blockPositionX, blockPositionY, healthResult));

            } 
            int currentBlockHealthForScore = healthNow - i; 
            if (currentBlockHealthForScore < 1) currentBlockHealthForScore = 1; 
            MaxScore += Constants.ColsOfArrayBlock * currentBlockHealthForScore * 10;
        }
        PaddleY = height - Constants.PaddleBottomOffset; 
        PaddleX = (width / 2f) - (PaddleWidth / 2f);
    }
    /// <summary>
    /// Создает новый набор блоков на игровом поле.
    /// Очищает старые блоки перед созданием новых.
    /// </summary>
    public void CreateBlocks()
    {
        Blocks.Clear();
        for (var i = 0; i < Constants.RowsOfArrayBlock; i++)
        {
            for (var j = 0; j < Constants.ColsOfArrayBlock; j++)
            {
                int blockPositionX = Constants.PaddleHorizontalMargin + Constants.GridOffsetX + j * (Constants.BWidth+Constants.BlockSpacing);
                int blockPositionY = Constants.PaddleHorizontalMargin + Constants.GridOffsetY + i * (Constants.BHeight+Constants.BlockSpacing);
                int healthResult = healthNow - i;
                Blocks.Add(new Block(blockPositionX, blockPositionY, healthResult));
            } 
        }
    }
    /// <summary>
    /// Сбрасывает состояние игры к начальному
    /// </summary>
    public void ResetBall()
    {
        IsStarted = false;
        BallVX = 0;
        BallVY = 0; 
        BallX = PaddleX + (PaddleWidth / 2f) - (BallSize / 2f);
        BallY = PaddleY - BallSize;
    }
    /// <summary>
    /// Обрабатывает движение мыши для управления платформой.
    /// </summary>
    public void MovePaddle(int mouseX)
    {
        var newX = mouseX - (PaddleWidth / 2f);
        if (newX < Constants.PaddleHorizontalMargin) newX = Constants.PaddleHorizontalMargin; 
        if (newX + PaddleWidth > GameWidth + Constants.PaddleHorizontalMargin) newX = GameWidth + Constants.PaddleHorizontalMargin - PaddleWidth;
        PaddleX = newX;
        if (!IsStarted)
        {
            BallX = PaddleX + (PaddleWidth / 2f) -  (BallSize / 2f);
            BallY = PaddleY - BallSize;
        }
    }
    /// <summary>
    /// Запускает мяч с платформы при клике мыши.
    /// </summary>
    public void StartBall()
    {
        if (!IsStarted)
        {
            IsStarted = true;
            BallVX = Constants.InitialBallSpeedX;
            BallVY = Constants.InitialBallSpeedY;
        }
    }
    /// <summary>
    /// Основной метод обновления состояния игры (игровой цикл).
    /// </summary>
    public void Update()
    {
        if (GameOver || GameWon) return;
        if (IsStarted)
        {
            BallX += BallVX;
            BallY += BallVY;
        }
        if (BallX <= Constants.PaddleHorizontalMargin)
        {
            BallX = Constants.PaddleHorizontalMargin; 
            BallVX = Math.Abs(BallVX);
        }
        else if (BallX + BallSize >= GameWidth + Constants.PaddleHorizontalMargin)
        {
            BallX = GameWidth + Constants.PaddleHorizontalMargin - BallSize;
            BallVX = -Math.Abs(BallVX); 
        }
        if (BallY <= Constants.TopWallOffset)
        {
            BallY = Constants.TopWallOffset;
            BallVY = Math.Abs(BallVY); 
        }
        if (BallVY > 0 && BallY + BallSize >= PaddleY && BallY + BallSize <= PaddleY + PaddleHeight && BallX + BallSize >= PaddleX && BallX <= PaddleX + PaddleWidth)
        {
            BallVY = -Math.Abs(BallVY);
            var paddleCenter = PaddleX + (PaddleWidth / 2f);
            var ballCenter = BallX + (BallSize / 2f);
            BallVX = (ballCenter - paddleCenter) * Constants.PaddleBounceInfluence;
        }
        for (var i = Blocks.Count - 1; i >=0; i--)
        {
            var b = Blocks[i];
            if (!(BallX + BallSize >= b.X && BallX <= b.X + b.Width &
                BallY + BallSize >= b.Y && BallY <= b.Y + b.Height)) continue;
            BallVY = -BallVY; 
            b.Health--;
            Score += 10;
            if (b.Health <= 0) Blocks.RemoveAt(i); 
            if (Blocks.Count == 0) GameWon = true;
            break;
        }
        if (BallY+BallSize >= GameHeight + Constants.DeadZoneOffset)
        {
            Lives--;
            if (Lives <= 0) 
            {
                Lives = 0;
                GameOver = true;
            }
            else ResetBall();
        };
        if (IsStarted)
        {
            bonusSpawnTimer++;
            if (bonusSpawnTimer >= Constants.BonusSpawnInterval)
            {
                SpawnBonus();
                bonusSpawnTimer = 0;
            }
        }
        if (IsBonusActive && IsStarted)
        {
            BonusY += Constants.BonusFallSpeed;
            if (BonusY + BonusSize >= PaddleY &&
                BonusY <= PaddleY + PaddleHeight &&
                BonusX + BonusSize >= PaddleX &&
                BonusX <= PaddleX + PaddleWidth)
            {
                ActivateBonusEffect();
                IsBonusActive = false;
            }
            if (BonusY > GameHeight + Constants.DeadZoneOffset) IsBonusActive = false;
        }
        if (bonusEffectTimer > 0 && IsStarted)
        {
            bonusEffectTimer--;
            if (bonusEffectTimer <= 0) PaddleWidth = Constants.DefaultPaddleWidth;
        }
    }
    /// <summary>
    /// Спавнит новый бонус в случайной позиции над игровым полем.
    /// </summary>
    private void SpawnBonus()
    {
        BonusX = random.Next(Constants.BonusSpawnMinX, GameWidth);
        BonusY = Constants.BonusSpawnY;
        IsBonusActive = true;
    }
    /// <summary>
    /// Активирует эффект пойманного бонуса.
    /// </summary>
    private void ActivateBonusEffect()
    {
        PaddleWidth = Constants.DefaultPaddleWidth * 2;
        bonusEffectTimer = Constants.BonusEffectDuration;
    }
}