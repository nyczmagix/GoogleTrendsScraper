using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace GoogleTrendsScraper.Lib.PageObjects
{
    public class TrendsPage : BasePage
    {
        #region Constructor
        public TrendsPage(IWebDriver webdriver)
           : base(webdriver,
                 "https://www.google.com/trends")
        {
            WaitUntilPageIsDisplayed();
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets at least the amount of stories indicated
        /// </summary>
        public IList<string> GetStories(int numOfStories)
        {
            var stories = FetchStories().ToList();
            while (stories.Count < numOfStories)
            {
                ScrollPageDownAndLoad();
                var newStories = FetchStories(stories.Count);
                if (newStories.Count() > 0)
                    stories.AddRange(newStories);
            }

            return stories;
        } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Collect stories that is currently loaded on page
        /// </summary>
        private IEnumerable<string> FetchStories()
            => FetchStories(0);

        /// <summary>
        /// Collect stories that are currently loaded on page starting from a specified position.
        /// </summary>
        private IEnumerable<string> FetchStories(int storyNumber)
        {
            string exp = $"(//a[contains(@class, 'trending-story')]/div[contains(@class, 'title')]/span)[position() > {storyNumber}]";
            return GetElements(exp)
                .Select(x => x.Text);
        }

        /// <summary>
        /// Indicates if the page is currently loading more stories
        /// </summary>
        private bool IsPageLoadingStories()
            => GetElement("//md-progress-circular").Displayed;

        /// <summary>
        /// Simulate a Page Down, which causes more stories to load
        /// </summary>
        private void ScrollPageDownAndLoad()
        {
            Actions.SendKeys(Keys.PageDown).Perform();
            while (IsPageLoadingStories())
                WaitOneSec();
        }

        /// <summary>
        /// Indicates if the Trends page has been successfully connected
        /// </summary>
        private bool IsPageLoaded()
        {
            var e = GetElement("//div[contains(@class, 'trends-wrapper')]");
            return e != null && e.Displayed;
        }

        /// <summary>
        /// Wait until the Trends page is reached
        /// </summary>
        private void WaitUntilPageIsDisplayed()
        {
            while (!IsPageLoaded())
                WaitOneSec();
        } 
        #endregion
    }
}
