using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading; // For Thread.Sleep()
using SeleniumTests.Utils;

namespace SeleniumTests.Tests
{
    [TestClass]
    public class BasketTests : TestBase
    {
        // Helper method to highlight elements
        private void HighlightElement(IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].style.border='3px solid red'", element);
            Thread.Sleep(1000); // Wait to see highlight
        }

        [TestMethod]
        public void AddItemsToBasket_AndCalculateTotal()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Select input fields and enter quantities
            var quantityInputs = driver.FindElements(By.CssSelector("input[type='number']"));
            Assert.IsTrue(quantityInputs.Count > 0, "No quantity inputs found!");

            HighlightElement(quantityInputs[0]);
            quantityInputs[0].SendKeys("2");
            Thread.Sleep(2000); // Wait to see change

            HighlightElement(quantityInputs[1]);
            quantityInputs[1].SendKeys("1");
            Thread.Sleep(2000);

            // Click Calculate Total
            var calculateButton = driver.FindElement(By.XPath("//button[contains(text(),'Calculate Total')]"));
            HighlightElement(calculateButton);
            calculateButton.Click();
            Thread.Sleep(3000);

            // Wait for total to be updated
            wait.Until(d => d.FindElement(By.CssSelector(".totals h3")).Displayed);
            var totalElement = driver.FindElement(By.CssSelector(".totals h3"));

            HighlightElement(totalElement);
            Assert.IsTrue(totalElement.Text.Contains("€"), "Total price not updated properly!");

            Console.WriteLine("✅ Total price calculated successfully!");
        }

        [TestMethod]
        public void GenerateReceipt()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Add items
            var quantityInputs = driver.FindElements(By.CssSelector("input[type='number']"));
            HighlightElement(quantityInputs[0]);
            quantityInputs[0].SendKeys("2");
            Thread.Sleep(2000);

            // Click Generate Receipt
            var generateButton = driver.FindElement(By.XPath("//button[contains(text(),'Generate Receipt')]"));
            HighlightElement(generateButton);
            generateButton.Click();
            Thread.Sleep(3000);

            // Wait for receipt to appear
            wait.Until(d => d.FindElement(By.CssSelector(".showReceipt")).Displayed);
            var receiptElement = driver.FindElement(By.CssSelector(".receipt-container pre"));

            HighlightElement(receiptElement);
            Assert.IsFalse(string.IsNullOrEmpty(receiptElement.Text), "Receipt is empty!");

            Console.WriteLine("✅ Receipt generated successfully!");
        }

        [TestMethod]
        public void ClearBasket()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Add items
            var quantityInputs = driver.FindElements(By.CssSelector("input[type='number']"));
            HighlightElement(quantityInputs[0]);
            quantityInputs[0].SendKeys("3");
            Thread.Sleep(2000);

            // Click Clear Basket
            var clearButton = driver.FindElement(By.XPath("//button[contains(text(),'Clear Basket')]"));
            HighlightElement(clearButton);
            clearButton.Click();
            Thread.Sleep(2000);

            try
            {
                // Wait for alert and accept it
                IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                Console.WriteLine("Alert Text: " + alert.Text);
                alert.Accept();  // Click "OK" on alert
                Console.WriteLine("✅ Alert accepted!");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ No alert appeared!");
            }

            Thread.Sleep(3000); // Wait for changes to apply

            // Verify all inputs are cleared
            quantityInputs = driver.FindElements(By.CssSelector("input[type='number']"));
            foreach (var input in quantityInputs)
            {
                HighlightElement(input);
                Assert.AreEqual("0", input.GetAttribute("value"), "Basket was not cleared properly!");
                Thread.Sleep(1000);
            }

            Console.WriteLine("✅ Basket cleared successfully!");
        }
    }
}
