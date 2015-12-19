using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace FactbookMiner
{
    public static class MainMiner
    {
        private static IWebDriver _driver;

        //private static void init()
        //{
        //    _driver = new PhantomJSDriver();
        //    _driver.Navigate().GoToUrl("https://www.cia.gov/library/publications/the-world-factbook/index.html");

        //    IWebElement dropDownCountryList = _driver.FindElement(By.XPath("//select[@name='selecter_links']"));

        //    SelectElement countryList = new SelectElement(dropDownCountryList);
        //    IList<IWebElement> options = countryList.Options;

        //    countryList.SelectByText("Colombia");
        //}

        public static string GetPresident()
        {
            //_driver = new PhantomJSDriver();
            //_driver.Navigate().GoToUrl("https://www.cia.gov/library/publications/the-world-factbook/index.html");

            //IWebElement dropDownCountryList = _driver.FindElement(By.XPath("//select[@name='selecter_links']"));
            //Console.WriteLine(dropDownCountryList.Text);
            //SelectElement countryList = new SelectElement(dropDownCountryList);
            //IList<IWebElement> options = countryList.Options;

            //countryList.SelectByText("Colombia");
            //Console.WriteLine(_driver.Url);
            //IWebElement pE = _driver.FindElement(By.XPath("//h2[@sectiontitle='Government']"));
            //pE.Click();
            //Console.WriteLine(pE.Text);
            //string headOfGovernmentKey = "head of government: ";
            //IWebElement presidentElement = pE.FindElement(By.XPath("//span[contains(.,'" + headOfGovernmentKey + "')]"));
            //Console.WriteLine("Getting main data");
            //Console.WriteLine(presidentElement.GetParent().Text);
            //Console.WriteLine(presidentElement.TagName + " " + presidentElement.GetAttribute("class") + " " + presidentElement.Text);
            //Console.WriteLine(presidentElement.ToString());

            //presidentElement = GetParent(presidentElement).FindElement(By.XPath(".//span[@class='category_data']"));
            //Console.WriteLine("Got president: {0}", presidentElement.Text);

            //return presidentElement.Text;

            string president = PropertyManager.GetValueFromKeyAndCountry("head of government: ", "Colombia", "Government");
            PropertyManager.SetValueIntoMainDictionary("ColombiaPresident", president);
            return president;
        }

        public static void LoadContent()
        {
            PropertyManager.SetCountries();
            PropertyManager.SetCategories();
        }


        //public static IWebElement GetParent(this IWebElement e)
        //{
        //    return e.FindElement(By.XPath(".."));
        //}
    }
}
