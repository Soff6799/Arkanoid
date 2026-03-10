namespace ArcadeGame;
using System;
using System.Drawing; 
using System.Windows.Forms;
/// <summary>
/// Основная форма приложения игры. Управляет отрисовкой, игровым циклом и обработкой ввода пользователя.
/// </summary>
public partial class Form : System.Windows.Forms.Form
{
    private GameEngine engine;
    private Timer timer;
    /// <summary>
    /// Конструктор формы. Инициализирует игровой движок, таймер,устанавливает обработчики событий и запускает игровой цикл.
    /// </summary>
    public Form()
    {
        InitializeComponent();
        engine = new GameEngine( Constants.DefoultScreenWidth, Constants.DefoultScreenHeight );
        BackgroundImage = Resources.background;
        BackgroundImageLayout = ImageLayout.Stretch;
        
        timer = new Timer();
        timer.Interval = Constants.TimerIntervalMc; 
        timer.Tick += TimerTick; 
        timer.Start();
        MouseMove += (sender, e) => engine.MovePaddle(e.X);
        MouseClick += Form_MouseClick;
        ButtonStartAgain.Visible = false;
    }
    private void TimerTick(object sender, EventArgs e)
    {
        engine.Update();
        using (Graphics g = CreateGraphics())
        {
            using (BufferedGraphics buffer = BufferedGraphicsManager.Current.Allocate(g, ClientRectangle))
            {
                RenderScene(buffer.Graphics);
                buffer.Render(g);
            }
        }
    }
    private void RenderScene(Graphics g)
    {
        if (BackgroundImage != null) g.DrawImage(BackgroundImage, ClientRectangle);
        using (Pen wallPen = new Pen(Color.LemonChiffon, Constants.BordersWidth)) g.DrawRectangle(wallPen, Constants.DrawingBordersX, Constants.DrawingBordersY, engine.GameWidth, engine.GameHeight);
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
        
        var heartsX = engine.GameWidth + Constants.UISidebarPadding;
        Font titleFont = new Font(Constants.GameFontFamily, Constants.TitleFontSize, FontStyle.Bold);
        Font statusFont = new Font(Constants.GameFontFamily, Constants.StatusFontSize, FontStyle.Bold);
        g.DrawString("Arcade game", titleFont, Brushes.White, heartsX, Constants.DistanceYName);
        using (Pen redPen = new Pen(Color.Red, Constants.WidthOfCirclesOfLives))
        {
            for (var i = 0; i < Constants.NumberOfLives; i++)
            {
                var yPos = Constants.DistanceOfLives + (i * Constants.DistanceBetweenLives);
                g.DrawEllipse(redPen, heartsX, yPos, Constants.LifeCircleDiameter, Constants.LifeCircleDiameter);
                if (i < engine.Lives) g.FillEllipse(Brushes.Red, heartsX, yPos, Constants.LifeCircleDiameter, Constants.LifeCircleDiameter);
            }
        }
        if (engine.GameOver) g.DrawString("You've lost!", statusFont, Brushes.Red, heartsX, Constants.DistanceToGameSituation);
        if (engine.GameWon) g.DrawString("You've won!", statusFont, Brushes.Gold, heartsX, Constants.DistanceToGameSituation);
        if (engine.GameOver || engine.GameWon) ButtonStartAgain.Visible = true;
        if (engine.IsBonusActive)
        {
            g.FillEllipse(Brushes.Magenta, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
            g.DrawEllipse(Pens.White, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
        }
        string scoreText = $"{engine.Score} / {engine.MaxScore}";
        float scoreX = heartsX; 
        float scoreY = engine.GameHeight - 40;
        g.DrawString(scoreText, statusFont, Brushes.White, scoreX, scoreY);
    }

    private void Form_Load(object sender, EventArgs e)
    {
        Point paddleCenterClient = new Point(
            (int)(engine.PaddleX + engine.PaddleWidth / 2),
            (int)(engine.PaddleY + engine.PaddleHeight / 2));
        Point paddleCenterScreen = PointToScreen(paddleCenterClient);
        Cursor.Position = paddleCenterScreen;
    }
    
    private void Form_MouseClick(object sender, MouseEventArgs e) => engine.StartBall();
    
    private void ResetGame()
    {
        engine.Lives = Constants.NumberOfLives;
        engine.GameOver = false;
        engine.GameWon = false;
        engine.Score = 0;
        engine.CreateBlocks();
        engine.ResetBall();
        ButtonStartAgain.Visible = false;
        engine.IsBonusActive = false;
        engine.PaddleWidth = Constants.StandardPlatformWidth;
    }
    
    private void ButtonStartAgain_Click(object sender, EventArgs e) => ResetGame();
}