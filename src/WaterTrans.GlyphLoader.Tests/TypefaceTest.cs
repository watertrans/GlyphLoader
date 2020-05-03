using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class TypefaceTest
    {
        private static readonly string[] _fontFiles = {
            "Roboto-Regular.ttf",
            "RobotoMono-Regular.ttf",
            "Lora-VariableFont_wght.ttf",
            "NotoSansJP-Regular.otf",
            "NotoSerifJP-Regular.otf",
            // "KozGoPr6N-Medium.otf",
        };

        private static readonly Dictionary<string, Typeface> _typefaceCache = LoadAllTypeface();
        private static readonly Dictionary<string, GlyphTypeface> _glyphTypefaceCache = LoadAllGlyphTypeface();

        private const string GlyphWarningMessage = @"
            <!DOCTYPE html><html><head><style>dt { font-weight: bold; } svg { border: 1px solid #000; }</style></head>
            <body><h1>This glyph may be incorrect.</h1>
            <dl><dt>File: </dt><dd>{fontFile}</dd></dl>
            <dl><dt>GlyphIndex: </dt><dd>{glyphIndex}</dd></dl>
            <dl><dt>Typeface: </dt><dd><svg width='128' height='128' viewBox='0 0 128 128' xmlns='http://www.w3.org/2000/svg' version='1.1'><path d='{pathData1}' fill='black' stroke='black' stroke-width='1' /></svg></dd></dl>
            <dl><dt>GlyphTypeface: </dt><dd><svg width='128' height='128' viewBox='0 0 128 128' xmlns='http://www.w3.org/2000/svg' version='1.1'><path d='{pathData2}' fill='black' stroke='black' stroke-width='1' /></svg></dd></dl>
            </body></html>
        ";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Baseline_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.Baseline, gt.Baseline);
            }
        }

        [TestMethod]
        public void StrikethroughPosition_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.StrikethroughPosition, gt.StrikethroughPosition);
            }
        }

        [TestMethod]
        public void StrikethroughThickness_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.StrikethroughThickness, gt.StrikethroughThickness);
            }
        }

        [TestMethod]
        public void UnderlinePosition_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.UnderlinePosition, gt.UnderlinePosition);
            }
        }

        [TestMethod]
        public void UnderlineThickness_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.UnderlineThickness, gt.UnderlineThickness);
            }
        }

        [TestMethod]
        public void CapsHeight_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.CapsHeight, gt.CapsHeight);
            }
        }

        [TestMethod]
        public void Height_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.Height, gt.Height);
            }
        }

        [TestMethod]
        public void XHeight_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.XHeight, gt.XHeight);
            }
        }

        [TestMethod]
        public void GlyphCount_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.GlyphCount, gt.GlyphCount);
            }
        }

        [TestMethod]
        public void AdvanceWidths_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    Assert.AreEqual(tf.AdvanceWidths[i], gt.AdvanceWidths[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        [TestMethod]
        public void AdvanceHeights_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    Assert.AreEqual(tf.AdvanceHeights[i], gt.AdvanceHeights[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        [TestMethod]
        public void CharacterToGlyphMap_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                foreach (int charCode in gt.CharacterToGlyphMap.Keys)
                {
                    Assert.AreEqual(tf.CharacterToGlyphMap[charCode], gt.CharacterToGlyphMap[charCode], "Font:" + fontFile + " CharCode:" + charCode);
                }
            }
        }

        // TODO 
        // [TestMethod]
        public void LeftSideBearings_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    Assert.AreEqual(tf.LeftSideBearings[i], gt.LeftSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // TODO
        // [TestMethod]
        public void TopSideBearings_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    Assert.AreEqual(tf.TopSideBearings[i], gt.TopSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfStreamIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Typeface(null));
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfStreamCanReadIsFalse()
        {
            var stream = new Mock<Stream>();
            stream.SetupGet(x => x.CanRead).Returns(false);
            Assert.ThrowsException<NotSupportedException>(() => new Typeface(stream.Object));
        }

        [TestMethod]
        public void Constructor_OK_IfStreamCanSeekIsFalse()
        {
            string fontPath = Path.Combine(Environment.CurrentDirectory, "Roboto-Regular.ttf");
            var stream = new Mock<MemoryStream>(File.ReadAllBytes(fontPath)) { CallBase = true };
            stream.SetupGet(x => x.CanSeek).Returns(false);
            new Typeface(stream.Object);
        }

        [TestMethod]
        public void Constructor_OK_TrueTypeCollection()
        {
            string fontPath = @"C:\Windows\Fonts\msgothic.ttc";
            if (!File.Exists(fontPath))
            {
                Assert.Inconclusive();
            }
            using (var fontStream = File.OpenRead(fontPath))
            {
                var tf = new Typeface(fontStream, 1);
            }
        }

        // For analysis
        // [TestMethod]
        public void GetGlyphOutline_TESTRUN()
        {
            // TODO analysis GLYF 106
            foreach (string fontFile in _fontFiles)
            {
                Typeface tf;
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                using (var fontStream = File.OpenRead(fontPath))
                {
                    tf = new Typeface(fontStream);
                }
                var geometry = tf.GetGlyphOutline(150, 128);
            }
        }

        [TestMethod]
        public void GetGlyphOutline_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];

                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    var sourceGeometry = tf.GetGlyphOutline(i, 128);
                    var mediaGeometry1 = ConvetToWpfPathGeometry(sourceGeometry);
                    mediaGeometry1.Transform = new TranslateTransform(0, tf.Baseline * 128);

                    var mediaGeometry2 = gt.GetGlyphOutline(i, 128, 128);
                    mediaGeometry2.Transform = new TranslateTransform(0, gt.Baseline * 128);

                    var flatGeometry1 = mediaGeometry1.GetFlattenedPathGeometry();
                    var flatGeometry2 = mediaGeometry2.GetFlattenedPathGeometry();


                    double totalDistance1 = GetTotalDistance(flatGeometry1);
                    double totalDistance2 = GetTotalDistance(flatGeometry2);
                    double errorRate = Math.Abs(totalDistance1 - totalDistance2) / totalDistance1;

                    // Over 99% match
                    if (errorRate > 0.01)
                    {
                        // Please see with your own eyes.
                        string message = GlyphWarningMessage
                            .Replace("{fontFile}", fontFile)
                            .Replace("{glyphIndex}", i.ToString())
                            .Replace("{pathData1}", mediaGeometry1.GetOutlinedPathGeometry().ToString().Remove(0, 2))
                            .Replace("{pathData2}", mediaGeometry2.GetOutlinedPathGeometry().ToString().Remove(0, 2));
                        System.Diagnostics.Trace.WriteLine(message);
                        Assert.Fail(message);
                    }

                    if (i % 500 == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                }
            }
        }

        private static Dictionary<string, Typeface> LoadAllTypeface()
        {
            var result = new Dictionary<string, Typeface>();
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                using (var fontStream = File.OpenRead(fontPath))
                {
                    result[fontFile] = new Typeface(fontStream);
                }
            }
            return result;
        }

        private static Dictionary<string, GlyphTypeface> LoadAllGlyphTypeface()
        {
            var result = new Dictionary<string, GlyphTypeface>();
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                result[fontFile] = new GlyphTypeface(new Uri(fontPath));
            }
            return result;
        }

        private System.Windows.Media.Geometry ConvetToWpfPathGeometry(WaterTrans.GlyphLoader.Geometry.PathGeometry geometry)
        {
            return System.Windows.Media.Geometry.Parse(geometry.ToString(0, 0, 4)).Clone();
        }

        private double GetTotalDistance(System.Windows.Media.PathGeometry path)
        {
            var pointList = GetPointList(path);
            double result = 0;

            for (int i = 0; i < pointList.Count; i++)
            {
                var points = pointList[i];
                for (int j = 0; j < points.Count; j++)
                {
                    if (j + 1 >= points.Count)
                    {
                        result += GetDistance(points[j], points[0]);
                    }
                    else
                    {
                        result += GetDistance(points[j], points[j + 1]);
                    }
                }
            }
            return result;
        }

        private List<List<System.Windows.Point>> GetPointList(System.Windows.Media.PathGeometry path)
        {
            var result = new List<List<System.Windows.Point>>();

            for (int i = 0; i < path.Figures.Count; i++)
            {
                var list = new List<System.Windows.Point>();
                list.Add(path.Figures[i].StartPoint);
                for (int j = 0; j < path.Figures[i].Segments.Count; j++)
                {
                    var poly = (PolyLineSegment)path.Figures[i].Segments[j];
                    list.AddRange(poly.Points);
                }
                result.Add(list);
            }

            return result;
        }

        private double GetDistance(System.Windows.Point p1, System.Windows.Point p2)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)));
        }

        private byte[] RenderBitmap(System.Windows.Media.Geometry geometry)
        {
            var bmp = new RenderTargetBitmap(128, 128, 96, 96, PixelFormats.Default);

            DrawingVisual viz = new DrawingVisual();
            DrawingContext dc = viz.RenderOpen();
            dc.DrawGeometry(Brushes.Black, null, geometry);
            dc.Close();
            bmp.Render(viz);

            var result = new byte[65536];
            bmp.CopyPixels(result, 512, 0);
            bmp.Clear();
            bmp = null;
            dc = null;
            viz = null;
            return result;
        }
    }
}
