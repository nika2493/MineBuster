using System;
using SeleniumDemo;
using SeleniumDemo.Solution;

var setup = new Setup();
var gameState = setup.SelectDifficulty(1);
var controller = gameState.Controller();
controller.MakeFirsMove();
gameState.Update();
while (true)
{
    var move = new MineBuster().NextMove(gameState.BoardState());
    controller.MakeMove(move);
    gameState.Update();
    if (gameState.IsFinished())
    {
        gameState = setup.Restart();
        controller = gameState.Controller();
        Console.Clear();
        Console.WriteLine($"Win:{Config.Win}");
        Console.WriteLine($"Lose:{Config.Lose}");
    }
}
