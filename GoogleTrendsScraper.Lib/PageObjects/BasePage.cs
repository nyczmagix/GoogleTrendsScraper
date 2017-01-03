using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using System.Threading;

namespace GoogleTrendsScraper.Lib.PageObjects
{
    abstract public class BasePage
    {
        protected IWebDriver Webdriver { get; set; }
        protected Actions Actions { get; set; }
        protected string BaseUrl { get; set;}

        public BasePage(IWebDriver webdriver)
        {
            Webdriver = webdriver;
            Actions = new Actions(webdriver);
        }

        public BasePage(IWebDriver webdriver, string baseUrl)
            : this(webdriver)   
        {
            webdriver.Navigate().GoToUrl(baseUrl);
        }

        public IWebElement GetElement(string xpath)
            => Webdriver.FindElement(By.XPath(xpath));

        public IList<IWebElement> GetElements(string xpath)
            => Webdriver.FindElements(By.XPath(xpath));

        public void Close()
            => Webdriver.Close();

        public void WaitOneSec()
            => Thread.Sleep(1000);     
    }
}
