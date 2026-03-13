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
    private string scoreText;          // Готовая строка счета
    
    /// <summary>
    /// Конструктор формы. Инициализирует игровой движок,
    /// таймер,устанавливает обработчики событий и запускает игровой цикл.
    /// </summary>
    public ArkanoidForm()
    {
        InitializeComponent();
        scoreText = "0 / 0";
        fieldBorderPen = new Pen(Color.LemonChiffon, ConstantsForm.BordersWidth);
        scoreBrush = new SolidBrush(Color.White);
        lifeIndicatorPen = new Pen(Color.Red, ConstantsForm.WidthOfCirclesOfLives);
        headerTitleFont = new Font(ConstantsForm.GameFontFamily, ConstantsForm.TitleFontSize, FontStyle.Bold);
        gameStatusFont = new Font(ConstantsForm.GameFontFamily, ConstantsForm.StatusFontSize, FontStyle.Bold);
        engine = new GameEngine( ConstantsForm.DefaultScreenWidth, ConstantsForm.DefaultScreenHeight );
        BackgroundImage = Resources.background;
        BackgroundImageLayout = ImageLayout.Stretch;
        timer = new Timer();
        timer.Interval = ConstantsForm.TimerIntervalMc; 
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
            using (Graphics tempG = this.CreateGraphics())
            {
                gameBuffer = BufferedGraphicsManager.Current.Allocate(tempG, this.ClientRectangle);
            }
        }
        var g = gameBuffer.Graphics;
        g.Clear(this.BackColor);
        RenderScene(g);        
        using (Graphics screenGraphics = this.CreateGraphics())
        {
            gameBuffer.Render(screenGraphics);
        }
    }
    
    private void RenderScene(Graphics g)
    {
        if (BackgroundImage != null) {g.DrawImage(BackgroundImage, ClientRectangle);}
        g.DrawRectangle(fieldBorderPen, ConstantsForm.DrawingBordersX, ConstantsForm.DrawingBordersY, engine.GameWidth, engine.GameHeight);
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
        var heartsX = engine.GameWidth + ConstantsForm.UiSidebarPadding;
        g.DrawString("Arcade game", headerTitleFont, Brushes.White, heartsX, ConstantsForm.DistanceYName);
        for (var i = 0; i < ConstantsForm.NumberOfLives; i++)
        {
            var yPos = ConstantsForm.DistanceOfLives + (i * ConstantsForm.DistanceBetweenLives);
            g.DrawEllipse(lifeIndicatorPen, heartsX, yPos, ConstantsForm.LifeCircleDiameter, ConstantsForm.LifeCircleDiameter);
            if (i < engine.Lives) {g.FillEllipse(Brushes.Red, heartsX, yPos, 
                ConstantsForm.LifeCircleDiameter, ConstantsForm.LifeCircleDiameter);}
        }
        if (engine.GameOver) {g.DrawString("You've lost!", gameStatusFont, Brushes.Red, heartsX, ConstantsForm.DistanceToGameSituation);}
        if (engine.GameWon) {g.DrawString("You've won!", gameStatusFont, Brushes.Gold, heartsX, ConstantsForm.DistanceToGameSituation);}
        if (engine.GameOver || engine.GameWon) {ButtonStartAgain.Visible = true;}
        if (engine.IsBonusActive)
        {
            g.FillEllipse(Brushes.Magenta, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
            g.DrawEllipse(Pens.White, engine.BonusX, engine.BonusY, engine.BonusSize, engine.BonusSize);
        }
        var scoreY = engine.GameHeight - ConstantsForm.ScoreDisplayOffsetY;
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
        engine.Lives = ConstantsForm.NumberOfLives;
        engine.GameOver = false;
        engine.GameWon = false;
        engine.Score = 0;
        engine.CreateBlocks();
        engine.ResetBall();
        ButtonStartAgain.Visible = false;
        engine.IsBonusActive = false;
        engine.PaddleWidth = ConstantsForm.StandardPlatformWidth;
    }
    
    private void ButtonStartAgain_Click(object sender, EventArgs e) => ResetGame();
}