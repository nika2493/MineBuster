using OpenQA.Selenium;
using SeleniumDemo.Solution;

namespace SeleniumDemo;

public class GameState
{
    private readonly GameController _controller;
    private readonly IWebDriver _driver;
    private readonly MineField _mineField;
    private bool _isFinished;
    private int _mineCount;

    public GameState(IWebDriver driver)
    {
        _driver = driver;
        var cells = _driver.FindElements(By.CssSelector("[id*=cell]"));
        var boarHeight = _driver.FindElements(By.ClassName("clear")).Count;
        var boardWith = cells.Count / boarHeight;
        _mineCount = ReadMineCount();
        _mineField = new MineField(boardWith, boarHeight, cells, _driver);
        _controller = new GameController(_mineField);
        Config.BoardWith = boardWith;
        Config.BoardHeight = boarHeight;
        Config.BombCount = _mineCount;
    }

    public void Update()
    {
        _mineCount = ReadMineCount();
        _mineField.Update();
        _isFinished = CheckGame();
    }

    public GameController Controller()
    {
        return _controller;
    }

    public SolutionBoard BoardState()
    {
        return new SolutionBoard
            { Board = _mineField.BoardState() };
    }

    public int NumberOfBombs()
    {
        return _mineCount;
    }

    public bool IsFinished()
    {
        return _isFinished;
    }

    private int ReadMineCount()
    {
        var mineCountDisplays = _driver.FindElements(By.CssSelector("[id*=top_area_mines]"));
        var count = 0;
        foreach (var display in mineCountDisplays)
        {
            var id = display.GetAttribute("id");
            var parsedId = id.Where(char.IsDigit).ToArray();
            var data = display.GetAttribute("class");
            var parsedData = data.Where(char.IsDigit).ToArray();
            count += int.Parse(parsedId) * int.Parse(parsedData);
        }

        return count;
    }

    private bool CheckGame()
    {
        var face = _driver.FindElement(By.Id("top_area_face"));
        var value = face.GetAttribute("class");
        if (value.Contains("lose"))
        {
            Config.Lose++;
            return true;
        }

        if (value.Contains("win"))
        {
            Config.Win++;
            return true;
        }

        return false;
    }
}
