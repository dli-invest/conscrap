using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace ConScrap 
{
    public class Browser {
        public static void TestBrowser() {

            string username = Environment.GetEnvironmentVariable("REMOTE_SELENIUM_USERNAME");
            
            string key = Environment.GetEnvironmentVariable("REMOTE_SELENIUM_KEY");
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
            driver.Navigate().GoToUrl("https://www.google.com");
            Console.WriteLine(driver.Title);
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("Browserstack");
            query.Submit();
            Console.WriteLine(driver.Title);
            // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
            if (string.Equals(driver.Title.Substring(0,12), "BrowserStack"))
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