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

        [TestMethod]
        // Test that no discount is applied when the basket is empty
        public void CalculateDiscounts_EmptyBasket_NoDiscounts()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Test that discount is applied correctly when buying an odd number of soups
        public void CalculateDiscounts_OddNumberOfSoups_OnlyFullDiscountApplied()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Test that discount is capped at the number of breads available (e.g., more soups than breads)
        public void CalculateDiscounts_MoreSoupsThanBreads_DiscountCappedByBreadCount()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        // Test that no discount is applied when only bread is purchased (without soup)
        public void CalculateDiscounts_BreadWithoutSoup_NoDiscount()
        {
            throw new NotImplementedException();
        }
    }
}