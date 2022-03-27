namespace SeleniumDemo.Solution;

public class Cell
{
    public int X { get; set; }

    public int Y { get; set; }

    public bool IsFlagged { get; set; }

    public bool IsClosed { get; set; }

    public int NeighbourBombCount { get; set; }

    public double BombChance { get; set; }

    public List<Cell> GetNeighbours(Cell[,] board)
    {
        var result = new List<Cell>();
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (Helpers.IsInsideBoard(X - i, Y - j))
                {
                    result.Add(board[X - i, Y - j]);
                }
            }
        }

        return result;
    }

    public Move Open()
    {
        return new Move() { X = X, Y = Y, Flag = false };
    }

    public Move Flag()
    {
        return new Move() { X = X, Y = Y, Flag = true };
    }
}
