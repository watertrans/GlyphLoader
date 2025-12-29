using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class PathFigureTest
    {
        [TestMethod]
        public void PathFigure_ToString_Is_Culture_Independent()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var testCulture = new CultureInfo("fr-FR");
                CultureInfo.CurrentCulture = testCulture;
                CultureInfo.CurrentUICulture = testCulture;

                var bezierSegment = new BezierSegment(new Point(1.2, 2.3), new Point(3.4, 4.5), new Point(5.6, 6.7), true);
                var quadraticBezierSegment = new QuadraticBezierSegment(new Point(1.2, 2.3), new Point(3.4, 4.5), true);
                var lineSegment = new LineSegment(new Point(1.2, 2.3), true);
                var pathFigure = new PathFigure(new Point(1.2, 2.3), new List<PathSegment>() { bezierSegment, quadraticBezierSegment, lineSegment }, true);

                var expected = "M1.2,2.3C1.2,2.3 3.4,4.5 5.6,6.7Q1.2,2.3 3.4,4.5L1.2,2.3z ";

                // Act
                var actual = pathFigure.ToString();

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
