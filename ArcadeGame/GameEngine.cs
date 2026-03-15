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
    public float BallX { get; private set; }

    /// <summary>
    /// Координата Y центра мяча.
    /// </summary>
    public float BallY { get; private set; }

    /// <summary>
    /// Скорость мяча по оси X (горизонтальная).
    /// </summary>
    public float BallVx { get; set; }

    /// <summary>
    /// Скорость мяча по оси Y (вертикальная).
    /// </summary>
    public float BallVy { get; set; }

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
    public float PaddleX { get; private set; }

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
        for (var i = 0; i < ConstantsGameEngine.RowsOfArrayBlock; i++)
        {
            for (var j = 0; j < ConstantsGameEngine.ColsOfArrayBlock; j++)
            {
                int blockPositionX = ConstantsGameEngine.PaddleHorizontalMargin + ConstantsGameEngine.GridOffsetX
                    + j * (ConstantsGameEngine.BWidth + ConstantsGameEngine.BlockSpacing);
                int blockPositionY = ConstantsGameEngine.PaddleHorizontalMargin + ConstantsGameEngine.GridOffsetY
                    + i * (ConstantsGameEngine.BHeight + ConstantsGameEngine.BlockSpacing);
                int healthResult = healthNow - i;
                Blocks.Add(new Block(blockPositionX, blockPositionY, healthResult));
            }
            int currentBlockHealthForScore = healthNow - i;
            if (currentBlockHealthForScore < 1)
            {
                currentBlockHealthForScore = 1;
            }
            MaxScore += ConstantsGameEngine.ColsOfArrayBlock * currentBlockHealthForScore *
                        ConstantsGameEngine.PointsPerHealthUnit;
        }
        PaddleY = height - ConstantsGameEngine.PaddleBottomOffset;
        PaddleX = (width / 2f) - (PaddleWidth / 2f);
    }

    /// <summary>
    /// Создает новый набор блоков на игровом поле.
    /// Очищает старые блоки перед созданием новых.
    /// </summary>
    public void CreateBlocks()
    {
        Blocks.Clear();
        for (var i = 0; i < ConstantsGameEngine.RowsOfArrayBlock; i++)
        {
            for (var j = 0; j < ConstantsGameEngine.ColsOfArrayBlock; j++)
            {
                int blockPositionX = ConstantsGameEngine.PaddleHorizontalMargin + ConstantsGameEngine.GridOffsetX
                    + j * (ConstantsGameEngine.BWidth + ConstantsGameEngine.BlockSpacing);
                int blockPositionY = ConstantsGameEngine.PaddleHorizontalMargin + ConstantsGameEngine.GridOffsetY
                    + i * (ConstantsGameEngine.BHeight + ConstantsGameEngine.BlockSpacing);
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
        BallVx = 0;
        BallVy = 0;
        BallX = PaddleX + (PaddleWidth / 2f) - (BallSize / 2f);
        BallY = PaddleY - BallSize;
    }

    /// <summary>
    /// Обрабатывает движение мыши для управления платформой.
    /// </summary>
    public void MovePaddle(int mouseX)
    {
        var newX = mouseX - (PaddleWidth / 2f);
        if (newX < ConstantsGameEngine.PaddleHorizontalMargin)
        {
            newX = ConstantsGameEngine.PaddleHorizontalMargin;
        }
        if (newX + PaddleWidth > GameWidth + ConstantsGameEngine.PaddleHorizontalMargin)
        {
            newX = GameWidth + ConstantsGameEngine.PaddleHorizontalMargin - PaddleWidth;
        }
        PaddleX = newX;
        if (!IsStarted)
        {
            BallX = PaddleX + (PaddleWidth / 2f) - (BallSize / 2f);
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
            BallVx = ConstantsGameEngine.InitialBallSpeedX;
            BallVy = ConstantsGameEngine.InitialBallSpeedY;
        }
    }

    /// <summary>
    /// Основной метод обновления состояния игры (игровой цикл).
    /// </summary>
    public void Update()
    {
        if (GameOver || GameWon)
        {
            return;
        }
        if (IsStarted)
        {
            BallX += BallVx;
            BallY += BallVy;
        }
        if (BallX <= ConstantsGameEngine.PaddleHorizontalMargin)
        {
            BallX = ConstantsGameEngine.PaddleHorizontalMargin;
            BallVx = Math.Abs(BallVx);
        }
        else if (BallX + BallSize >= GameWidth + ConstantsGameEngine.PaddleHorizontalMargin)
        {
            BallX = GameWidth + ConstantsGameEngine.PaddleHorizontalMargin - BallSize;
            BallVx = -Math.Abs(BallVx);
        }
        if (BallY <= ConstantsGameEngine.TopWallOffset)
        {
            BallY = ConstantsGameEngine.TopWallOffset;
            BallVy = Math.Abs(BallVy);
        }
        if (BallVy > 0 && BallY + BallSize >= PaddleY && BallY + BallSize <= PaddleY + PaddleHeight
            && BallX + BallSize >= PaddleX && BallX <= PaddleX + PaddleWidth)
        {
            BallVy = -Math.Abs(BallVy);
            var paddleCenter = PaddleX + (PaddleWidth / 2f);
            var ballCenter = BallX + (BallSize / 2f);
            BallVx = (ballCenter - paddleCenter) * ConstantsGameEngine.PaddleBounceInfluence;
        }
        for (var i = Blocks.Count - 1; i >= 0; i--)
        {
            var b = Blocks[i];
            if (!(BallX + BallSize >= b.X && BallX <= b.X + b.Width &
                    BallY + BallSize >= b.Y && BallY <= b.Y + b.Height)) continue;
            BallVy = -BallVy;
            b.Health--;
            Score += ConstantsGameEngine.PointsPerHealthUnit;
            if (b.Health <= 0) Blocks.RemoveAt(i);
            if (Blocks.Count == 0) GameWon = true;
            break;
        }
        if (BallY + BallSize >= GameHeight + ConstantsGameEngine.DeadZoneOffset)
        {
            Lives--;
            if (Lives <= 0)
            {
                Lives = 0;
                GameOver = true;
            }
            else ResetBall();
        }
        if (IsStarted)
        {
            bonusSpawnTimer++;
            if (bonusSpawnTimer >= ConstantsGameEngine.BonusSpawnInterval)
            {
                SpawnBonus();
                bonusSpawnTimer = 0;
            }
        }
        if (IsBonusActive && IsStarted)
        {
            BonusY += ConstantsGameEngine.BonusFallSpeed;
            if (BonusY + BonusSize >= PaddleY &&
                BonusY <= PaddleY + PaddleHeight &&
                BonusX + BonusSize >= PaddleX &&
                BonusX <= PaddleX + PaddleWidth)
            {
                ActivateBonusEffect();
                IsBonusActive = false;
            }
            if (BonusY > GameHeight + ConstantsGameEngine.DeadZoneOffset)
            {
                IsBonusActive = false;
            }
        }
        if (bonusEffectTimer > 0 && IsStarted)
        {
            bonusEffectTimer--;
            if (bonusEffectTimer <= 0)
            {
                PaddleWidth = ConstantsGameEngine.DefaultPaddleWidth;
            }
        }
    }

    /// <summary>
    /// Спавнит новый бонус в случайной позиции над игровым полем.
    /// </summary>
    private void SpawnBonus()
    {
        BonusX = random.Next(ConstantsGameEngine.BonusSpawnMinX, GameWidth);
        BonusY = ConstantsGameEngine.BonusSpawnY;
        IsBonusActive = true;
    }

    /// <summary>
    /// Активирует эффект пойманного бонуса.
    /// </summary>
    private void ActivateBonusEffect()
    {
        PaddleWidth = ConstantsGameEngine.DefaultPaddleWidth * 2;
        bonusEffectTimer = ConstantsGameEngine.BonusEffectDuration;
    }
}