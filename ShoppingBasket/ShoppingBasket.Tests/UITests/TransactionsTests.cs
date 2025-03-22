using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading; // For Thread.Sleep()
using SeleniumTests.Utils;

namespace SeleniumTests.Tests
{
    [TestClass]
    public class TransactionsTests : TestBase
    {
        private void HighlightElement(IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].style.border='3px solid blue'", element);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// This test verifies that users can view the list of transactions.
        /// </summary>
        [TestMethod]
        public void ViewTransactions()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Click "Show Transactions"
            var toggleButton = driver.FindElement(By.XPath("//button[contains(text(),'Show Transactions')]"));
            HighlightElement(toggleButton);
            toggleButton.Click();
            Thread.Sleep(3000);

            // Wait for transactions to load
            wait.Until(d => d.FindElement(By.CssSelector(".transactions-scrollable-container")).Displayed);

            var transactionsContainer = driver.FindElement(By.CssSelector(".transactions-scrollable-container"));
            HighlightElement(transactionsContainer);
            Assert.IsTrue(transactionsContainer.Displayed, "Transactions section is not visible!");

            Console.WriteLine("Transactions are visible!");
        }

        /// <summary>
        /// This test verifies that users can delete all transactions.
        /// </summary>
        [Ignore]
        [TestMethod]
        public void DeleteAllTransactions()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Show transactions
            var toggleButton = driver.FindElement(By.XPath("//button[contains(text(),'Show Transactions')]"));
            HighlightElement(toggleButton);
            toggleButton.Click();
            Thread.Sleep(3000);

            wait.Until(d => d.FindElement(By.CssSelector(".transactions-scrollable-container")).Displayed);

            // Click "Delete All Transactions"
            var deleteButton = driver.FindElement(By.XPath("//button[contains(text(),'Delete All Transactions')]"));
            HighlightElement(deleteButton);
            deleteButton.Click();
            Thread.Sleep(2000);

            // Confirm alert
            driver.SwitchTo().Alert().Accept();
            Thread.Sleep(2000);

            // Verify transactions are deleted
            wait.Until(d => !d.FindElement(By.CssSelector(".transactions-scrollable-container")).Displayed);

            Console.WriteLine("All transactions deleted successfully!");
        }
    }
}