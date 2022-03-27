namespace SeleniumDemo.Solution;

public class MineBuster
{
    private readonly int _boardHeight = Config.BoardHeight;
    private readonly int _boardWith = Config.BoardWith;
    private Cell[,] _board;
    private int _numberOfMines = Config.BombCount;

    public List<Move> NextMove(SolutionBoard board)
    {
        _board = board.Board;
        var result = new List<Move>();
        var obviousBombs = GetObviousBombs();
        foreach (var cell in obviousBombs)
        {
            result.Add(cell.Flag());
        }

        var obviousSafeClosedCells = GetObviousSafeClosedCells();
        foreach (var cell in obviousSafeClosedCells)
        {
            result.Add(cell.Open());
        }

        if (result.Count > 0)
        {
            return result.DistinctBy(c => new { c.X, c.Y, c.Flag }).ToList();
        }

        result.Add(RandomMove());
        return result;
    }

    private Move RandomMove()
    {
        var rng = GetRandomCell();

        while (!rng.IsClosed || rng.IsFlagged)
        {
            rng = GetRandomCell();
        }

        return rng.Open();
    }

    private Cell GetRandomCell()
    {
        var random = new Random();
        var rngX = random.Next(0, _boardWith);
        var rngY = random.Next(0, _boardHeight);
        return _board[rngX, rngY];
    }

    private List<Cell> GetObviousSafeClosedCells()
    {
        var result = new List<Cell>();
        for (var x = 0; x < _boardWith; x++)
        {
            for (var y = 0; y < _boardHeight; y++)
            {
                var cell = _board[x, y];
                if (cell.IsFlagged || cell.NeighbourBombCount == 0) continue;
                var cells = ObviousSafeClosedCells(cell);
                result.AddRange(cells);
            }
        }

        return result;
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

        foreach (var neighbour in closedNeighbours)
        {
            neighbour.BombChance = 0;
            if (!neighbour.IsFlagged && neighbour.IsClosed)
            {
                result.Add(neighbour);
            }
        }

        return result;
    }

    private List<Cell> GetObviousBombs()
    {
        var result = new List<Cell>();
        for (var x = 0; x < _boardWith; x++)
        {
            for (var y = 0; y < _boardHeight; y++)
            {
                var cell = _board[x, y];
                if (cell.IsFlagged || cell.NeighbourBombCount == 0)
                {
                    continue;
                }

                var cells = ObviousBombs(cell);
                result.AddRange(cells);
            }
        }

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

        foreach (var neighbour in closedNeighbours)
        {
            neighbour.BombChance = 100;
            if (!neighbour.IsFlagged)
            {
                result.Add(neighbour);
            }
        }

        return result;
    }
}
