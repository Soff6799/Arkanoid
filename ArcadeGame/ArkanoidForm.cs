namespace ArcadeGame;

using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Основная форма приложения игры. Управляет отрисовкой, игровым циклом и обработкой ввода пользователя.
/// </summary>
public partial class ArkanoidForm : Form
{
    private GameEngine engine;
    private Timer timer;
    private BufferedGraphics gameBuffer;
    private Pen fieldBorderPen;
    private SolidBrush scoreBrush;
    private Pen lifeIndicatorPen;
    private Font headerTitleFont;
    private Font gameStatusFont;
    private string scoreText;

    /// <summary>
    /// Конструктор формы. Инициализирует игровой движок,
    /// таймер,устанавливает обработчики событий и запускает игровой цикл.
    /// </summary>
    public ArkanoidForm()
    {
        InitializeComponent();
        scoreText = "0 / 0";
        fieldBorderPen = new Pen(Color.LemonChiffon, FormConstants.BordersWidth);
        scoreBrush = new SolidBrush(Color.White);
        lifeIndicatorPen = new Pen(Color.Red, FormConstants.WidthOfCirclesOfLives);
        headerTitleFont = new Font(FormConstants.GameFontFamily, FormConstants.TitleFontSize, FontStyle.Bold);
        gameStatusFont = new Font(FormConstants.GameFontFamily, FormConstants.StatusFontSize, FontStyle.Bold);
        engine = new GameEngine(FormConstants.DefaultScreenWidth, FormConstants.DefaultScreenHeight);
        BackgroundImage = Resources.background;
        BackgroundImageLayout = ImageLayout.Stretch;
        timer = new Timer();
        timer.Interval = FormConstants.TimerIntervalMc;
        timer.Tick += TimerTick;
        timer.Start();
        MouseMove += (_, e) => engine.MovePaddle(e.X);
        MouseClick += Form_MouseClick;
        ButtonStartAgain.Visible = false;
    }

    private void TimerTick(object sender, EventArgs e)
    {
        engine.Update();
        scoreText = $"{engine.Score} / {engine.MaxScore}";
        if (gameBuffer == null)
        {
            using (var tempG = CreateGraphics())
            {
                gameBuffer = BufferedGraphicsManager.Current.Allocate(tempG, ClientRectangle);
            }
        }
        var g = gameBuffer.Graphics;
        g.Clear(BackColor);
        RenderScene(g);
        using (var screenGraphics = CreateGraphics())
        {
            gameBuffer.Render(screenGraphics);
        }
    }

    private void RenderScene(Graphics g)
    {
        if (BackgroundImage != null)
        {
            g.DrawImage(BackgroundImage, ClientRectangle);
        }
        g.DrawRectangle(fieldBorderPen, FormConstants.DrawingBordersX, FormConstants.DrawingBordersY, engine.GameWidth,
            engine.GameHeight);
        foreach (var block in engine.Blocks)
        {
            Brush brush;
            switch (block.Health)
            {
                case 3: brush = Brushes.Black; break;
                case 2: brush = Brushes.Blue; break;
                case 1: brush = Brushes.Green; break;
                default: brush = Brushes.Gray; break;
            }
            g.FillRectangle(brush, block.X, block.Y, block.Width, block.Height);
            g.DrawRectangle(Pens.DimGray, block.X, block.Y, block.Width, block.Height);
        }
        g.FillEllipse(Brushes.Yellow, engine.BallX, engine.BallY, engine.BallSize, engine.BallSize);
        g.FillRectangle(Brushes.Black, engine.PaddleX, engine.PaddleY, engine.PaddleWidth, engine.PaddleHeight);
        g.DrawRectangle(Pens.DimGray, engine.PaddleX, engine.PaddleY, engine.PaddleWidth, engine.PaddleHeight);
        var heartsX = engine.GameWidth + FormConstants.UiSidebarPadding;
        g.DrawString("Arcade game", headerTitleFont, Brushes.White, heartsX, FormConstants.DistanceYName);
        for (var i = 0; i < FormConstants.NumberOfLives; i++)
        {
            var yPos = FormConstants.DistanceOfLives + (i * FormConstants.DistanceBetweenLives);
            g.DrawEllipse(lifeIndicatorPen, heartsX, yPos, FormConstants.LifeCircleDiameter,
                FormConstants.LifeCircleDiameter);
            if (i < engine.Lives)
            {
                g.FillEllipse(Brushes.Red, heartsX, yPos,
                    FormConstants.LifeCircleDiameter, FormConstants.LifeCircleDiameter);
            }
        }
        if (engine.GameOver)
        {
            g.DrawString("You've lost!", gameStatusFont, Brushes.Red, heartsX, FormConstants.DistanceToGameSituation);
        }
        if (engine.GameWon)
        {
            g.DrawString("You've won!", gameStatusFont, Brushes.Gold, heartsX, FormConstants.DistanceToGameSituation);
        }
        if (engine.GameOver || engine.GameWon)
        {
            ButtonStartAgain.Visible = true;
        }
        if (engine.IsBonusActive)
        {
            g.FillEllipse(Brushes.Magenta, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
            g.DrawEllipse(Pens.White, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
        }
        var scoreY = engine.GameHeight - FormConstants.ScoreDisplayOffsetY;
        g.DrawString(scoreText, gameStatusFont, scoreBrush, heartsX, scoreY);
    }

    private void Form_Load(object sender, EventArgs e)
    {
        var paddleCenterClient = new Point(
            (int)(engine.PaddleX + engine.PaddleWidth / 2),
            (int)(engine.PaddleY + engine.PaddleHeight / 2));
        var paddleCenterScreen = PointToScreen(paddleCenterClient);
        Cursor.Position = paddleCenterScreen;
    }

    private void Form_MouseClick(object sender, MouseEventArgs e) => engine.StartBall();

    private void ResetGame()
    {
        engine.Lives = FormConstants.NumberOfLives;
        engine.GameOver = false;
        engine.GameWon = false;
        engine.Score = 0;
        engine.CreateBlocks();
        engine.ResetBall();
        ButtonStartAgain.Visible = false;
        engine.IsBonusActive = false;
        engine.PaddleWidth = FormConstants.StandardPlatformWidth;
    }

    private void ButtonStartAgain_Click(object sender, EventArgs e) => ResetGame();
}