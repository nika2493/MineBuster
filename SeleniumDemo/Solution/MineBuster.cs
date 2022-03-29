namespace SeleniumDemo.Solution;

public class MineBuster
{
    private readonly int _boardHeight = Config.BoardHeight;
    private readonly int _boardWith = Config.BoardWith;
    private Cell[,] _board = null!;
    private int _numberOfMines = Config.BombCount;

    public List<Move> NextMove(SolutionBoard board)
    {
        _board = board.Board;
        var result = GetObviousMoves();
        if (result.Count > 0)
        {
            return result.DistinctBy(c => new { c.X, c.Y, c.Flag }).ToList();
        }

        var randomCells = GetCellWithEightNeighbouredClosedCell();
        if (randomCells.Any())
        {
            var rng = new Random();
            var cell = randomCells[rng.Next(randomCells.Count)];
            return new List<Move> { cell.Open() };
        }

        result.Add(RandomMove());
        return result;
    }

    private List<Cell> GetCellWithEightNeighbouredClosedCell()
    {
        var cells = new List<Cell>();
        foreach (var cell in _board)
        {
            if (!cell.IsClosed)
            {
                continue;
            }

            var ngb = cell.GetNeighbours(_board);
            if (ngb.Count(c => c.IsClosed) == 8)
            {
                cells.Add(cell);
            }
        }

        return cells;
    }

    private List<Move> GetObviousMoves()
    {
        var result = new List<Move>();
        for (var x = 0; x < _boardWith; x++)
        {
            for (var y = 0; y < _boardHeight; y++)
            {
                var cell = _board[x, y];
                if (cell.IsFlagged || cell.NeighbourBombCount == 0)
                {
                    continue;
                }

                result.AddRange(ObviousBombs(cell).Select(bomb => bomb.Flag()));
                result.AddRange(ObviousSafeClosedCells(cell).Select(bomb => bomb.Open()));
            }
        }

        return result;
    }

    private Move RandomMove()
    {
        var randomCell = GetRandomCell();

        while (!randomCell.IsClosed || randomCell.IsFlagged)
        {
            randomCell = GetRandomCell();
        }

        return randomCell.Open();
    }

    private Cell GetRandomCell()
    {
        var random = new Random();
        var rngX = random.Next(0, _boardWith);
        var rngY = random.Next(0, _boardHeight);
        return _board[rngX, rngY];
    }

    private List<Cell> ObviousSafeClosedCells(Cell cell)
    {
        var result = new List<Cell>();
        var neighbours = cell.GetNeighbours(_board);
        var flaggedCells = neighbours.Where(c => c.IsFlagged).ToList();
        var closedNeighbours = neighbours.Where(c => c.IsClosed && !c.IsFlagged).ToList();
        if (cell.NeighbourBombCount != flaggedCells.Count)
        {
            return result;
        }

        result.AddRange(closedNeighbours.Where(neighbour => !neighbour.IsFlagged && neighbour.IsClosed));

        return result;
    }

    private List<Cell> ObviousBombs(Cell cell)
    {
        var result = new List<Cell>();
        var neighbours = cell.GetNeighbours(_board);
        var closedNeighbours = neighbours.Where(c => c.IsClosed || c.IsFlagged).ToList();
        if (cell.NeighbourBombCount != closedNeighbours.Count)
        {
            return result;
        }

        result.AddRange(closedNeighbours.Where(neighbour => !neighbour.IsFlagged));

        return result;
    }
}
