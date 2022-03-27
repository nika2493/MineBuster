using System;
using System.Collections.Generic;
using SeleniumDemo.Solution;

namespace SeleniumDemo;

public class GameController
{
    private readonly MineField _mineField;

    public GameController(MineField mineField)
    {
        _mineField = mineField;
    }

    public void OpenCell(int x, int y)
    {
        _mineField.OpenCell(x, y);
    }

    public void PlantFlag(int x, int y)
    {
        _mineField.PlantFlag(x, y);
    }
    public void MakeFirsMove()
    {
        var randomX = new Random().Next(0, Config.BoardWith);
        var randomY = new Random().Next(0, Config.BoardHeight);
        _mineField.OpenCell(randomX, randomY);
    }
    public void MakeMove(Move move)
    {
        if (move.Flag) PlantFlag(move.X, move.Y);
        else OpenCell(move.X, move.Y);
    }
    public void MakeMove(List<Move> moves)
    {
        foreach (var move in moves)
        {
            if (move.Flag) PlantFlag(move.X, move.Y);
            else OpenCell(move.X, move.Y);
        }
    }
}
