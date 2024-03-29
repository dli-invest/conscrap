using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Threading;
using OpenQA.Selenium.Support;
using System.Collections.Generic;

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
            DriverOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddAdditionalOption("os_version", "11");
            options.AddAdditionalOption("resolution", "1920x1080");
            options.AddAdditionalOption("browser", "Chrome");
            options.AddAdditionalOption("browser_version", "latest");
            options.AddAdditionalOption("user-agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) ''Chrome/94.0.4606.81 Safari/537.36");
            // caps.Add("os", "Windows");
            // caps.Add("name", "BStack-[C_sharp] Sample Test"); // test name
            // caps.Add("buildName", "BStack Build Number 1"); // CI/CD job or build name
            string url = String.Format("https://{0}:{1}@hub-cloud.browserstack.com/wd/hub/",username, key);
            driver = new RemoteWebDriver(
              new Uri(url), options
            );
            return driver;
        }

        /// <summary>
        ///     Click show by newest comments button
        /// </summary>
        public static Boolean SortByNewestComments(IWebDriver driver)
        {
            try
            {
                Thread.Sleep(5000);
                // make into function
                string sortXPath = Constants.YahooXPaths.sortButtonXPath;
                var sortEle = driver.FindElement(By.XPath(sortXPath));
                sortEle.Click();
                Thread.Sleep(5000);
                string createdXPath = Constants.YahooXPaths.sortByCreatedAtXPath;
                var createdEle = driver.FindElement(By.XPath(createdXPath));
                createdEle.Click();
                Thread.Sleep(5000);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void ShowAllComments(IWebDriver driver)
        {
            // sort by newest
            string showMoreXPath = Constants.YahooXPaths.showMoreXPath;
            int numFailure = 0;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var element = driver.FindElement(By.XPath(showMoreXPath));
                    // need a delay to show elements
                    // click on element using javascript
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                    // element.Click();
                    Thread.Sleep(300);

                }
                catch (NoSuchElementException)
                {
                    numFailure++;
                    if (numFailure > 4)
                    {
                        Console.WriteLine(i + " Element does not exist! Stopping Loop");
                        break;
                    }
                } catch(OpenQA.Selenium.ElementClickInterceptedException ex) {
                    Console.WriteLine("ElementClickInterceptedException");
                    numFailure++;
                    Console.WriteLine(ex);
                    if (numFailure > 4)
                    {
                        Console.WriteLine(i + " ElementClickInterceptedException! Stopping Loop");
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Get all Entries from yahoo finance by constantly clicking button.
        /// </summary>
        /// \todo figure out how to show replies
        public static string GetAllEntries(string ticker = "PKK.CN")
        {
            // have to grab content from iframe this will not be fun.
            IWebDriver driver = Browser.MkBrowser();
            // use base url from contant
            string msgUrls = String.Format("https://finance.yahoo.com/quote/{0}/community?p={0}", ticker);
            Console.WriteLine(String.Format("Parsing messages for {0}", ticker));
            driver.Navigate().GoToUrl(msgUrls);
            // Boolean success = SortByNewestComments(driver);
            // if (!success) 
            // {
            //     Console.WriteLine(String.Format("Sort By newest Comments failed for {0}", ticker));
            //     SortByNewestComments(driver);
            // }
            // ShowAllComments(driver);
            //
            // jacSandbox_9209380
            // 
            // WebElement iFrame = driver.findElements(By.tagName("iframe")).get(1);
            // driver.switchTo().frame(iframe);
            // driver.getPageSource();
            // driver.switchTo().defaultContent();

            Thread.Sleep(10000);
            try {
                // find Maybe later by text Maybe Later
                IWebElement maybeLater = driver.FindElement(By.XPath("//button[contains(text(), 'Maybe later')]"));
                // IWebElement maybeLater = driver.FindElement(By.XPath("//button[contains(@class, 'btn btn-primary')]"));
                maybeLater.Click();
            } catch (NoSuchElementException) {
                Console.WriteLine("No Maybe Later Button");
            }
            IWebElement iFrame = driver.FindElement(By.XPath("//iframe[contains(@id, 'jacSandbox')]"));
            driver.SwitchTo().Frame(iFrame);
            Thread.Sleep(2500);
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

            // String pageSource = driver.PageSource;
            // content = driver.
            // Run javascript return document.getElementsByTagName('html')[0].innerHTML
            // using webdriver
            // Click Maybe Later button if available
            Thread.Sleep(1500);
            // IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            // String pageSource = js.ExecuteScript("return document.getElementsByTagName('html')[0].outerHTML").ToString();
            // get spotIm-in-jac-conversations
            // //*[@id="spotim-specific"]/div/div
            // get element by xpath
            try {
                IWebElement spotIm = driver.FindElement(By.XPath("//*[@id=\"spotim-specific\"]/div/div"));
            } catch (NoSuchElementException) {
                Console.WriteLine("No spotIm element");
            }
            // getShadowRoot
            // output content of spotIm
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            String getCommentsCmd = Constants.jsQuerySelectorAllShadows + """const results = querySelectorAllShadows("ul.spcv_messages-list"); return results[0].outerHTML.toString();""";
            String listElement = (String) js.ExecuteScript(getCommentsCmd);

            Console.WriteLine("listElement", listElement);

            // shadowRoot.FindElement(By.TagName("div"));
            // shadow_root = driver.execute_script('return arguments[0].shadowRoot', element)

            // var getShadowRoot = spotIm.GetShadowRoot();
            // get element spcv_mainContainer
            // get outer htlm from shadowroot
            // getShadowRoot Elements
            // var getShadowRootElements = shadowRoot.FindElements(By.Name("div"));
            // Console.WriteLine(getShadowRootElements.Count);
            // // var fullContent = getShadowRoot.FindElements(By.TagName("div"))[0];
            // // Console.WriteLine(fullContent.GetAttribute("outerHTML"));
            // IWebElement spcv_mainContainer = shadowRoot.FindElement(By.XPath("/div/div/div/div/div/div"));
            // print htmlCOntent of spcv_mainContainer
            // Console.WriteLine(spcv_mainContainer.GetAttribute("innerHTML"));
            // // String pageSource = getShadowRoot.PageSource;
            // // write pageSource to file
            // String pageSource = spcv_mainContainer.GetAttribute("innerHTML");
            // System.IO.File.WriteAllText(@"WriteText.html", listElement);
            driver.SwitchTo().DefaultContent();
            // System.IO.File.WriteAllText(@"WriteText.txt", pageSource);
            return listElement.ToString();
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