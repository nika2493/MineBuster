using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumDemo;

public class Cell
{
    private readonly IWebDriver _driver;
    private readonly int _x;
    private readonly int _y;
    private int? _status;
    private IWebElement _webElement;

    public Cell(IWebDriver driver, IWebElement webElement, int x, int y)
    {
        _webElement = webElement;
        _driver = driver;
        _x = x;
        _y = y;
    }

    public void PlantFlag()
    {
        var rightClick = new Actions(_driver).ContextClick(_webElement);
        rightClick.Perform();
        _status = -1;
    }

    public void Open()
    {
        _webElement.Click();
    }

    public void Update()
    {
        _webElement = _driver.FindElement(By.Id($"cell_{_x}_{_y}"));
        var status = _webElement.GetAttribute("class");
        if (status.Contains("hd_type"))
        {
            var value = status[^1] - 48;
            _status = value;
        }

        if (status.Contains("hd_flag"))
        {
            _status = -1;
        }
    }

    public int? Status()
    {
        return _status;
    }
}
