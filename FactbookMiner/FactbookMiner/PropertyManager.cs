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
            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
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
            Console.WriteLine("-----LOG: \n-Tagname: " + parent.TagName + " \n-Attribute: " + parent.GetAttribute("class") + " \n-Text: " + parent.Text);
            _wait.Until((d) =>
                                {
                                    return parent.FindElement(By.XPath(".//span[@class='category_data']"));
                                });
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
            }
            PropertyManager.Categories = items;
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
