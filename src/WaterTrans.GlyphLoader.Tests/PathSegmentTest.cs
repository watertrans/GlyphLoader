using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class PathSegmentTest
    {
        [TestMethod]
        public void BezierSegment_ToString_Is_Culture_Independent()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var testCulture = new CultureInfo("fr-FR");
                CultureInfo.CurrentCulture = testCulture;
                CultureInfo.CurrentUICulture = testCulture;

                var expected = "C1.2,2.3 3.4,4.5 5.6,6.7";

                // Act
                var segment = new BezierSegment(new Point(1.2, 2.3), new Point(3.4, 4.5), new Point(5.6, 6.7), true);
                var actual = segment.ToString();

                // Assert
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [TestMethod]
        public void LineSegment_ToString_Is_Culture_Independent()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var testCulture = new CultureInfo("fr-FR");
                CultureInfo.CurrentCulture = testCulture;
                CultureInfo.CurrentUICulture = testCulture;

                var expected = "L1.2,2.3";

                // Act
                var segment = new LineSegment(new Point(1.2, 2.3), true);
                var actual = segment.ToString();

                // Assert
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [TestMethod]
        public void QuadraticBezierSegment_ToString_Is_Culture_Independent()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var testCulture = new CultureInfo("fr-FR");
                CultureInfo.CurrentCulture = testCulture;
                CultureInfo.CurrentUICulture = testCulture;

                var expected = "Q1.2,2.3 3.4,4.5";

                // Act
                var segment = new QuadraticBezierSegment(new Point(1.2, 2.3), new Point(3.4, 4.5), true);
                var actual = segment.ToString();

                // Assert
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }
}
