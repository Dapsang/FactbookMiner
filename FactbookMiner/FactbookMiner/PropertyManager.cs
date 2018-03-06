using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace FactbookMiner
{
    public static class PropertyManager
    {
        private static IWebDriver _driver;
        private const string FACTBOOKURL = "https://www.cia.gov/library/publications/the-world-factbook/index.html";
        private static WebDriverWait _wait;
        public static Dictionary<string, string> MainData = new Dictionary<string, string>();
        public static List<string> Countries;
        private static SelectElement countrySelect;
        public static List<string> Categories;

        static PropertyManager()
        {
            _driver = new PhantomJSDriver();
            _driver.Navigate().GoToUrl(FACTBOOKURL);
            //_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //_wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
        }

        public static IWebElement GetParent(this IWebElement e)
        {
            return e.FindElement(By.XPath(".."));
        }

        public static string GetValueFromKeyAndCountry(string key, string country, string category)
        {
            string res = string.Empty;
            IWebElement dropDownCountryList = _driver.FindElement(By.XPath("//select[@name='selecter_links']"));
            SelectElement countryList = new SelectElement(dropDownCountryList);
            countryList.SelectByText(country);
            IWebElement pE = _driver.FindElement(By.XPath("//h2[@sectiontitle='" + category + "']"));
            pE.Click();
            IWebElement mainElement = pE.FindElement(By.XPath("//span[contains(.,'" + key + "')]"));
            Console.WriteLine("-----LOG: \n-Tagname: " + mainElement.TagName + " \n-Attribute: " + mainElement.GetAttribute("class") + " \n-Text: " + mainElement.Text);
            IWebElement parent = GetParent(mainElement);
            // TODO: webelement.isDisplayed(); Use try catch with isDisplayed, then thread.sleep(100) etc...
            //_wait.Until((d) =>
            //                    {
            //                        return parent.FindElement(By.XPath(".//span[@class='category_data']"));
            //                    });
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("----LOG: INSIDE LOOP: \n Tagname: " + parent.TagName + " \n-Attribute: " + parent.GetAttribute("class") + " \n-Text: " + parent.Text);
            //    Console.WriteLine("Iteration: " + i.ToString());
            //    System.Threading.Thread.Sleep(500);
            //}
            System.Threading.Thread.Sleep(80);
            Console.WriteLine("-----LOG: \n-Tagname: " + parent.TagName + " \n-Attribute: " + parent.GetAttribute("class") + " \n-Text: " + parent.Text);
            mainElement = parent.FindElement(By.XPath(".//span[@class='category_data']"));
            return mainElement.Text;
        }

        public static void SetValueIntoMainDictionary(string key, string value)
        {
            MainData.Add(key, value);
        }

        public static void SetCountries()
        {
            IWebElement dropDownCountryList = _driver.FindElement(By.XPath("//select[@name='selecter_links']"));
            SelectElement countryList = new SelectElement(dropDownCountryList);
            PropertyManager.countrySelect = countryList;
            IList<IWebElement> options = countryList.Options;
            PropertyManager.Countries = options.Select(option => option.Text).ToList();
        }

        public static void SetCategories()
        {
            countrySelect.SelectByText("Andorra");
            IWebElement categories = _driver.FindElement(By.XPath("//ul[@class='expandcollapse']"));
            string afterreplace = categories.Text.Replace("\r\n", "_");
            List<string> items = afterreplace.Split('_').ToList();
            List<string> clone = new List<string>(items);
            foreach(string entry in clone)
            {
                if(string.Equals(entry, "Show"))
                {
                    items.Remove(entry);
                }
                else if (string.Equals(entry, "Panel - Collapsed"))
                {
                    items.Remove(entry);
                }
                else
                {
                    int index = entry.IndexOf("::");
                    string countryName = entry.Substring(index-1);
                    string newEntry = entry.Replace(countryName, "");
                    items.Remove(entry);
                    items.Add(newEntry);
                }
            }
            PropertyManager.Categories = items;
        }

        public static void SetSubCategories()
        {
            // TODO: Get the subcategories from the main categories. 
            // Odd is a category and even is the data for the given category (class of the li items)
            Dictionary<string, Dictionary<string, Dictionary<string,string>>> SubCategoriesAndData = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            foreach (string category in PropertyManager.Categories)
            {
                IWebElement pE = _driver.FindElement(By.XPath("//h2[@sectiontitle='" + category + "']"));
                pE.Click();
                IWebElement mainElement = pE.GetParent().GetParent();
                string oddOrEven = mainElement.GetAttribute("class").Split(' ')[0];
                if(oddOrEven == "odd")
                    oddOrEven = "even";
                else if (oddOrEven == "even")
                    oddOrEven = "odd";
                else
                    throw new ArgumentException("Next element is not a category");
                IWebElement contentElement = mainElement.FindElement(By.XPath("following-sibling::li[@class='" + oddOrEven + "']"));
                List<IWebElement> subcategories = contentElement.FindElements(By.XPath("child::div")).ToList();
                bool isField = true;
                string subCategoryName = string.Empty;
                Dictionary<string, Dictionary<string, string>> subCategoryValue = new Dictionary<string, Dictionary<string, string>>();
                // TODO: Use Dictionary<string, List<string>> ?? 
                foreach (IWebElement element in subcategories)
                {
                    if (element.GetAttribute("id") == "field")
                    {
                        isField = true;
                        subCategoryName = element.Text;
                    }
                    else
                    {
                        isField = false;
                    }
                    int i = 0;
                }
                int x = 0;
            }

        }
    }


    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }
}
