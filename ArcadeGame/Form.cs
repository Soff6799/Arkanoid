namespace ArcadeGame;
using System;
using System.Drawing; 
using System.Windows.Forms;
/// <summary>
/// Основная форма приложения игры. Управляет отрисовкой, игровым циклом и обработкой ввода пользователя.
/// </summary>
public partial class Form : System.Windows.Forms.Form
{
    private GameEngine _engine;
    private System.Windows.Forms.Timer _timer;
    /// <summary>
    /// Конструктор формы. Инициализирует игровой движок, таймер,устанавливает обработчики событий и запускает игровой цикл.
    /// </summary>
    public Form()
    {
        InitializeComponent();
        _engine = new GameEngine( Constants.DefoultScreenWidth, Constants.DefoultScreenHeight );
        BackgroundImage = ArcadeGame.Resources.background;
        BackgroundImageLayout = ImageLayout.Stretch;
        
        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = Constants.TimerIntervalMc; 
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
        using (Pen wallPen = new Pen(Color.LemonChiffon, Constants.BordersWidth)) g.DrawRectangle(wallPen, Constants.DrawingBordersX, Constants.DrawingBordersY, _engine.GameWidth, _engine.GameHeight);
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
        
        var heartsX = _engine.GameWidth + Constants.UISidebarPadding;
        Font titleFont = new Font(Constants.GameFontFamily, Constants.TitleFontSize, FontStyle.Bold);
        Font statusFont = new Font(Constants.GameFontFamily, Constants.StatusFontSize, FontStyle.Bold);
        g.DrawString("Arcade game", titleFont, Brushes.White, heartsX, Constants.DistanceYName);
        using (Pen redPen = new Pen(Color.Red, Constants.WidthOfCirclesOfLives))
        {
            for (var i = 0; i < Constants.NumberOfLives; i++)
            {
                var yPos = Constants.DistanceOfLives + (i * Constants.DistanceBetweenLives);
                g.DrawEllipse(redPen, heartsX, yPos, Constants.LifeCircleDiameter, Constants.LifeCircleDiameter);
                if (i < _engine.Lives) g.FillEllipse(Brushes.Red, heartsX, yPos, Constants.LifeCircleDiameter, Constants.LifeCircleDiameter);
            }
        }
        if (_engine.GameOver) g.DrawString("You've lost!", statusFont, Brushes.Red, heartsX, Constants.DistanceToGameSituation);
        if (_engine.GameWon) g.DrawString("You've won!", statusFont, Brushes.Gold, heartsX, Constants.DistanceToGameSituation);
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
        _engine.Lives = Constants.NumberOfLives;
        _engine.GameOver = false;
        _engine.GameWon = false;
        _engine.Score = 0;
        _engine.CreateBlocks();
        _engine.ResetBall();
        ButtonStartAgain.Visible = false;
        _engine.IsBonusActive = false;
        _engine.PaddleWidth = Constants.StandardPlatformWidth;
    }
    
    private void ButtonStartAgain_Click(object sender, EventArgs e) => ResetGame();
}