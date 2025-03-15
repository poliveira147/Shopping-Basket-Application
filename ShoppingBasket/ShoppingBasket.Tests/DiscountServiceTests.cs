namespace ShoppingBasket.Tests
{
    [TestClass]
    public class DiscountServiceTests
    {

        /// <summary>
        /// Tests that a 10% discount is correctly applied to apples.
        /// </summary>
        [TestMethod]
        public void CalculateDiscounts_ApplesDiscount_AppliesCorrectly()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests that the multi-buy discount (buy 2 soups, get 1 bread half price) is correctly applied.
        /// </summary>
        [TestMethod]
        public void CalculateDiscounts_MultiBuyBreadDiscount_AppliesCorrectly()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests that no discounts are applied for items that do not qualify for any discounts (e.g., milk).
        /// </summary>
        [TestMethod]
        public void CalculateDiscounts_NoDiscountsForMilk()
        {
            throw new NotImplementedException();
        }
    }
}