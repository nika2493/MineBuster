using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumDemo;

public class Setup
{
    private readonly IWebDriver _driver;

    public Setup()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _driver.Navigate().GoToUrl("https://minesweeper.online/");
    }

    public GameState SelectDifficulty(int lvl)
    {
        _driver.FindElement(By.ClassName($"homepage-level-{lvl}")).Click();
        WaitForGameLoad(_driver);
        var gameState = new GameState(_driver);
        return gameState;
    }

    private void WaitForGameLoad(IWebDriver driver)
    {
        var waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waiter.Until(ExpectedConditions.ElementExists(By.Id("top_area_face")));
    }

    public GameState Restart()
    {
        IWebElement face = _driver.FindElement(By.Id("top_area_face"));
        face.Click();
        var gameState = new GameState(_driver);
        return gameState;
    }
}
