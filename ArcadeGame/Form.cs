namespace ArcadeGame;
using System;
using System.Drawing; 
using System.Windows.Forms;
/// <summary>
/// Основная форма приложения игры. Управляет отрисовкой, игровым циклом и обработкой ввода пользователя.
/// </summary>
public partial class Form : System.Windows.Forms.Form
{
    private const int DefoultScreenWidth = 640;   
    private const int DefoultScreenHeight = 480;
    private const int TimerIntervalMc = 20; // 50 кадров в секунду

    private const int DrawingBordersX = 20;
    private const int DrawingBordersY = 40;
    private const int BordersWidth = 5;
    
    private const int UISidebarPadding = 40;
    private const string GameFontFamily = "Segoe UI";
    private const float TitleFontSize = 18f;
    private const float StatusFontSize = 20f;

    private const int DistanceYName = 50;

    private const int NumberOfLives = 3;
    private const int WidthOfCirclesOfLives = 2;
    private const int DistanceOfLives = 110;
    private const int DistanceBetweenLives = 40;
    private const int LifeCircleDiameter = 25;
    
    private const int DistanceToGameSituation = 260;
    private const int StandardPlatformWidth = 100;
    
    private GameEngine _engine;
    private System.Windows.Forms.Timer _timer;
    /// <summary>
    /// Конструктор формы. Инициализирует игровой движок, таймер,устанавливает обработчики событий и запускает игровой цикл.
    /// </summary>
    public Form()
    {
        InitializeComponent();
        _engine = new GameEngine( DefoultScreenWidth, DefoultScreenHeight );
        BackgroundImage = ArcadeGame.Resources.background;
        BackgroundImageLayout = ImageLayout.Stretch;
        
        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = TimerIntervalMc; 
        _timer.Tick += TimerTick; 
        _timer.Start();
        MouseMove += (sender, e) => _engine.MovePaddle(e.X);
        MouseClick += Form_MouseClick;
        ButtonStartAgain.Visible = false;
    }
    private void TimerTick(object sender, EventArgs e)
    {
        _engine.Update();
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
        using (Pen wallPen = new Pen(Color.LemonChiffon, BordersWidth)) g.DrawRectangle(wallPen, DrawingBordersX, DrawingBordersY, _engine.GameWidth, _engine.GameHeight);
        foreach (var block in _engine.Blocks)
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
        g.FillEllipse(Brushes.Yellow, _engine.BallX, _engine.BallY, _engine.BallSize, _engine.BallSize);
        g.FillRectangle(Brushes.Black, _engine.PaddleX, _engine.PaddleY, _engine.PaddleWidth, _engine.PaddleHeight);
        g.DrawRectangle(Pens.DimGray, _engine.PaddleX, _engine.PaddleY, _engine.PaddleWidth, _engine.PaddleHeight);
        
        var heartsX = _engine.GameWidth + UISidebarPadding;
        Font titleFont = new Font(GameFontFamily, TitleFontSize, FontStyle.Bold);
        Font statusFont = new Font(GameFontFamily, StatusFontSize, FontStyle.Bold);
        g.DrawString("Arcade game", titleFont, Brushes.White, heartsX, DistanceYName);
        using (Pen redPen = new Pen(Color.Red, WidthOfCirclesOfLives))
        {
            for (var i = 0; i < NumberOfLives; i++)
            {
                var yPos = DistanceOfLives + (i * DistanceBetweenLives);
                g.DrawEllipse(redPen, heartsX, yPos, LifeCircleDiameter, LifeCircleDiameter);
                if (i < _engine.Lives) g.FillEllipse(Brushes.Red, heartsX, yPos, LifeCircleDiameter, LifeCircleDiameter);
            }
        }
        if (_engine.GameOver) g.DrawString("You've lost!", statusFont, Brushes.Red, heartsX, DistanceToGameSituation);
        if (_engine.GameWon) g.DrawString("You've won!", statusFont, Brushes.Gold, heartsX, DistanceToGameSituation);
        if (_engine.GameOver || _engine.GameWon) ButtonStartAgain.Visible = true;
        if (_engine.IsBonusActive)
        {
            g.FillEllipse(Brushes.Magenta, _engine.BonusX, _engine.BonusY, _engine.BonusSize, _engine.BonusSize);
            g.DrawEllipse(Pens.White, _engine.BonusX, _engine.BonusY, _engine.BonusSize, _engine.BonusSize);
        }
        string scoreText = $"{_engine.Score} / {_engine.MaxScore}";
        float scoreX = heartsX; 
        float scoreY = _engine.GameHeight - 40;
        g.DrawString(scoreText, statusFont, Brushes.White, scoreX, scoreY);
    }

    private void Form_Load(object sender, EventArgs e)
    {
        Point paddleCenterClient = new Point(
            (int)(_engine.PaddleX + _engine.PaddleWidth / 2),
            (int)(_engine.PaddleY + _engine.PaddleHeight / 2));
        Point paddleCenterScreen = PointToScreen(paddleCenterClient);
        Cursor.Position = paddleCenterScreen;
    }
    
    private void Form_MouseClick(object sender, MouseEventArgs e) => _engine.StartBall();
    
    private void ResetGame()
    {
        _engine.Lives = NumberOfLives;
        _engine.GameOver = false;
        _engine.GameWon = false;
        _engine.Score = 0;
        _engine.CreateBlocks();
        _engine.ResetBall();
        ButtonStartAgain.Visible = false;
        _engine.IsBonusActive = false;
        _engine.PaddleWidth = StandardPlatformWidth;
    }
    
    private void ButtonStartAgain_Click(object sender, EventArgs e) => ResetGame();
}