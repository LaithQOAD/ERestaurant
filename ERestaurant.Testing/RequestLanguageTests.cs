using ERestaurant.API.Middlewares.LanguageMiddleware;
using System.Globalization;

namespace ERestaurant.Testing
{
    public class RequestLanguageTests
    {
        [Theory]
        [InlineData("ar", true, "ar")]
        [InlineData("ar-JO", true, "ar")]
        [InlineData("en", false, "en")]
        [InlineData("en-US", false, "en")]
        [InlineData("fr-FR", false, "fr")]
        public void IsArabicAndTwoLetterAreComputedFromCurrentUICulture(
            string uiCulture, bool expectedIsArabic, string expectedTwoLetter)
        {
            var original = CultureInfo.CurrentUICulture;
            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo(uiCulture);

                var sut = new RequestLanguage();

                Assert.Equal(expectedTwoLetter, sut.TwoLetter);
                Assert.Equal(expectedIsArabic, sut.IsArabic);
            }
            finally
            {
                CultureInfo.CurrentUICulture = original;
            }
        }

        [Fact]
        public void UsesCurrentUICultureNotCurrentCulture()
        {
            var originalUi = CultureInfo.CurrentUICulture;
            var original = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("ar-JO");
                CultureInfo.CurrentCulture = new CultureInfo("en-US");

                var sut = new RequestLanguage();

                Assert.Equal("ar", sut.TwoLetter);
                Assert.True(sut.IsArabic);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalUi;
                CultureInfo.CurrentCulture = original;
            }
        }
    }
}
