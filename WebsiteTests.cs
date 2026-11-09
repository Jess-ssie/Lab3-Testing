using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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

    [Test]
    public void ContextMenu_RightClick_ShowsAlert()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/context_menu");
        var area = driver.FindElement(By.Id("hot-spot"));
        new Actions(driver).ContextClick(area).Perform();

        var alert = wait.Until(d =>
        {
            try { return d.SwitchTo().Alert(); }
            catch (NoAlertPresentException) { return null; }
        });
        Assert.That(alert!.Text, Is.EqualTo("You selected a context menu"));
        alert.Accept();
    }

    [Test]
    public void EntryAd_CloseModal_ModalIsHidden()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/entry_ad");
        var modal = wait.Until(d =>
        {
            var element = d.FindElement(By.Id("modal"));
            if (element.Displayed)
            {
                return element;
            }
            return null;
        });
        modal!.FindElement(By.CssSelector(".modal-footer p")).Click();
        Assert.That(wait.Until(d => !d.FindElement(By.Id("modal")).Displayed), Is.True);
    }

    [Test]
    public void DynamicLoading_ClickStart_ShowsHelloWorld()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/dynamic_loading/2");
        driver.FindElement(By.CssSelector("#start button")).Click();
        var text = wait.Until(d =>
        {
            var elements = d.FindElements(By.CssSelector("#finish h4"));
            if (elements.Count > 0 && elements[0].Displayed)
            {
                return elements[0].Text;
            }
            return null;
        });
        Assert.That(text, Is.EqualTo("Hello World!"));
    }

    [Test]
    public void DynamicLoading_HiddenElement_BecomesVisible()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/dynamic_loading/1");
        var result = driver.FindElement(By.Id("finish"));
        Assert.That(result.Displayed, Is.False);
        driver.FindElement(By.CssSelector("#start button")).Click();
        wait.Until(_ => result.Displayed);
        Assert.That(result.Text, Is.EqualTo("Hello World!"));
    }


    [Test]
    public void ForgotPassword_SubmitEmail_ShowsConfirmation()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/forgot_password");
        driver.FindElement(By.Id("email")).SendKeys("e.vretik2026@gmail.com");
        driver.FindElement(By.Id("form_submit")).Click();
        wait.Until(d => d.Url.Contains("/email_sent"));
        Assert.That(driver.PageSource, Does.Contain("Your e-mail's been sent!"));
    }

    [Test]
    public void Typos_CheckText_HasCorrectSpelling()
    {
        driver.Navigate().GoToUrl(HomeUrl + "/typos");
        var text = driver.FindElement(By.CssSelector(".example")).Text;
        Assert.That(text, Does.Contain("Sometimes you'll see a typo, other times you won't."));
    }

    [Test]
    public void BasicAuth_ValidCredentials_ShowsSuccessMessage()
    {
        driver.Navigate().GoToUrl("https://admin:admin@the-internet.herokuapp.com/basic_auth");
        var message = driver.FindElement(By.CssSelector(".example p"));
        Assert.That(message.Text, Does.Contain("Congratulations! You must have the proper credentials."));
    }


    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
    }
}
