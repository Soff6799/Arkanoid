namespace ArcadeGame.Logic.Logic; 

/// <summary>
/// Представляет один игровой блок (кирпичик), который может быть уничтожен мячом.
/// </summary>
public class Block
{
    /// <summary>
    /// Горизонтальная координата (X) верхнего левого угла блока.
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// Вертикальная координата (Y) верхнего левого угла блока.
    /// </summary>
    public int Y { get; set; }
    
    /// <summary>
    /// Ширина блока.
    /// </summary>
    public int Width { get; set; } = 60;
    
    /// <summary>
    /// Высота блока.
    /// </summary>
    public int Height { get; set; } = 20;
    
    /// <summary>
    /// Текущее "здоровье" блока. Определяет, сколько ударов мяча блок может выдержать.Влияет на цвет блока.
    /// </summary>
    public int Health { get; set; }
    
    /// <summary>
    /// Конструктор для создания нового экземпляра блока.
    /// </summary>
    public Block(int x, int y, int health)
    {
        X = x;
        Y = y;
        Health = health;
    }
}