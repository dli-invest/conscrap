using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;
namespace ConScrap
{
    public class Browser
    {
        /// <summary>
        /// make selenium connection with browserstack
        /// </summary>
        public static IWebDriver MkBrowser()
        {

            string username = Environment.GetEnvironmentVariable("REMOTE_SELENIUM_USERNAME");
            string key = Environment.GetEnvironmentVariable("REMOTE_SELENIUM_KEY");
            if (username == null)
                throw new InvalidOperationException("Missing REMOTE_SELENIUM_USERNAME env var");
            if (key == null) throw new InvalidOperationException("Missing REMOTE_SELENIUM_KEY env var");
            // get environment variables for browserstack
            IWebDriver driver;
            OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
            // options.AddAdditionalCapability("os_version", "10", true);
            // options.AddAdditionalCapability("resolution", "1920x1080", true);
            // options.AddAdditionalCapability("browser", "Chrome", true);
            // options.AddAdditionalCapability("browser_version", "latest", true);
            options.AddAdditionalCapability("os", "Windows", true);
            options.AddAdditionalCapability("name", "BStack-[C_sharp] Sample Test", true); // test name
            options.AddAdditionalCapability("build", "BStack Build Number 1", true); // CI/CD job or build name
            options.AddAdditionalCapability("browserstack.user", username, true);
            options.AddAdditionalCapability("browserstack.key", key, true);

            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), options
            );
            return driver;
        }


        /// <summary>
        ///     Get all Entries from yahoo finance by constantly clicking button.
        /// </summary>
        /// \todo figure out how to show replies
        public static string GetAllEntries(string ticker = "PKK.CN")
        {
            IWebDriver driver = Browser.MkBrowser();
            // use base url from contant
            string msgUrls = String.Format("https://finance.yahoo.com/quote/{0}/community?p={0}", ticker);
            driver.Navigate().GoToUrl(msgUrls);
            // driver.Navigate().GoToUrl(msgUrls);
            // add wait for element to load in v2
            Thread.Sleep(5000);
            string showMoreXPath = Constants.YahooXPaths.showMoreXPath;
            var element = driver.FindElement(By.XPath(showMoreXPath));

            // Console.WriteLine(element);
            // Console.WriteLine(element.GetAttribute("innerHTML"));
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    driver.FindElement(By.XPath(showMoreXPath));
                    element.Click();
                    Thread.Sleep(300);

                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine(i + " Element does not exist! Stopping Loop");
                    break;
                }
            }
            // click on all the replies elements
            // string repliesXPath = Constants.YahooXPaths.repliesXPath;
            // driver.FindElement(By.ClassName("replies-button")).Click();
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(driver);
            // foreach (var reply in replies)
            // {
            //     try
            //     {
            //         reply.Click();
            //         Console.WriteLine(reply.TagName);
            //         // Thread.Sleep(100);
            //     }
            //     catch (OpenQA.Selenium.StaleElementReferenceException)
            //     {
            //         Console.WriteLine("Element does not exist! Stopping Loop");
            //         break;
            //     }
            // }

            String pageSource = driver.PageSource;
            // System.IO.File.WriteAllText(@"WriteText.txt", pageSource);
            return pageSource;
        }

        public static void TestBrowser()
        {
            IWebDriver driver = MkBrowser();
            driver.Navigate().GoToUrl("https://www.google.com");
            Console.WriteLine(driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Browserstack");
            query.Submit();
            Console.WriteLine(driver.Title);
            // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
            if (string.Equals(driver.Title.Substring(0, 12), "BrowserStack"))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }
            driver.Quit();
        }
    }
}