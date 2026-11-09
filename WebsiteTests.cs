using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Lab3Testing;

public class WebsiteTests
{
    private const string HomeUrl = "https://the-internet.herokuapp.com";
    private IWebDriver driver = null!;
    private WebDriverWait wait = null!;

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    [Test]
    public void Dropdown_SelectOption2_Option2IsSelected()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/dropdown");
        var dropdown = new SelectElement(driver.FindElement(By.Id("dropdown")));
        dropdown.SelectByText("Option 2");
        Assert.That(dropdown.SelectedOption.Text, Is.EqualTo("Option 2"));
    }

    [Test]
    public void HorizontalSlider_MoveRight_DisplaysOne()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/horizontal_slider");
        var slider = driver.FindElement(By.CssSelector("input[type='range']"));
        slider.SendKeys(Keys.Home);
        slider.SendKeys(Keys.ArrowRight + Keys.ArrowRight);
        Assert.That(driver.FindElement(By.Id("range")).Text, Is.EqualTo("1"));
    }

    [Test]
    public void RedirectLink_ClickLink_OpensStatusCodes()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/redirector");
        driver.FindElement(By.Id("redirect")).Click();
        wait.Until(d => d.Url.EndsWith("/status_codes"));
        Assert.That(driver.Url, Does.EndWith("/status_codes"));
        Assert.That(driver.FindElement(By.TagName("h3")).Text, Is.EqualTo("Status Codes"));
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
    }
}
