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

        static PropertyManager()
        {
            _driver = new PhantomJSDriver();
            _driver.Navigate().GoToUrl(FACTBOOKURL);
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
            IList<IWebElement> options = countryList.Options;
            countryList.SelectByText(country);
            IWebElement pE = _driver.FindElement(By.XPath("//h2[@sectiontitle='" + category + "']"));
            pE.Click();
            IWebElement mainElement = pE.FindElement(By.XPath("//span[contains(.,'" + key + "')]"));
            Console.WriteLine("-----LOG: \n-" + mainElement.TagName + " \n-" + mainElement.GetAttribute("class") + " \n-" + mainElement.Text);
            IWebElement parent = GetParent(mainElement);
            mainElement = parent.FindElement(By.XPath(".//span[@class='category_data']"));
            return mainElement.Text;
        }

        public static void SetValueIntoMainDictionary(string key, string value)
        {
            MainData.Add(key, value);
        }

    }
}
