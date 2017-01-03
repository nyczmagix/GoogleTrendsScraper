using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace GoogleTrendsScraper.Lib.PageObjects.Tests
{
    public class TrendsPageTests
    {
        private TrendsPage _trendPage;
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
            => _driver = new ChromeDriver();

        [OneTimeTearDown]
        public void OneTimeTearDown()
            => _driver.Dispose();

        [SetUp]
        public void SetUp()
            => _trendPage = new TrendsPage(_driver);

        [TearDown]
        public void Teardown()
            => _trendPage.Close();

        [Test]
        [Description("Load Google Trends and load up to 50 stories")]
        public void GetAtLeast50Stories()
        {
            const int desiredStoriesCount = 50;
            var stories = _trendPage.GetStories(desiredStoriesCount);
            Assert.GreaterOrEqual(stories.Distinct().Count(), desiredStoriesCount, $"Expected stories to be {desiredStoriesCount} or more");
        }
    }
}