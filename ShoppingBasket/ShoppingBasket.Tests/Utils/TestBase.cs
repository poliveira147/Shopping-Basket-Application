using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests.Utils
{
    [TestClass]
    public class TestBase
    {
        protected IWebDriver driver;

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:65089/basket"); // Your Angular app URL
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
