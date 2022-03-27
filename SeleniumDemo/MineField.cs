using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace SeleniumDemo;

public class MineField
{
    private readonly Cell[,] _board;
    private readonly IWebDriver _driver;
    private readonly int _height;
    private readonly int _with;
    public MineField(int with, int height, ReadOnlyCollection<IWebElement> cells, IWebDriver driver)
    {
        _with = with;
        _height = height;
        _driver = driver;
        _board = new Cell[with, height];
        foreach (var cell in cells)
        {
            var x = int.Parse(cell.GetAttribute("data-x").Where(char.IsDigit).ToArray());
            var y = int.Parse(cell.GetAttribute("data-y").Where(char.IsDigit).ToArray());
            _board[x, y] = new Cell(_driver, cell, x, y);
        }
    }
    public void PlantFlag(int x, int y)
    {
        _board[x, y].PlantFlag();
    }
    public void OpenCell(int x, int y)
    {
        _board[x, y].Open();
        Update();
    }
    public void Update()
    {
        foreach (var cell in _board) cell.Update();
    }
    public Solution.Cell[,] BoardState()
    {
        var result = new Solution.Cell[_with, _height];
        for (var x = 0; x < _with; x++)
        for (var y = 0; y < _height; y++)
        {
            var browserCell = _board[x, y];
            var cell = new Solution.Cell
            {
                X = x,
                Y = y
            };
            if (browserCell.Status() is null)
            {
                cell.IsClosed = true;
            }
            else
            {
                if (browserCell.Status() is -1) cell.IsFlagged = true;
                else
                {
                    cell.NeighbourBombCount = (int)browserCell.Status();
                }
            }
            result[x, y] = cell;
        }
        return result;
    }
}
