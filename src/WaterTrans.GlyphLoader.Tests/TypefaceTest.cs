using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WaterTrans.GlyphLoader.TestFonts;

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

        public TestContext TestContext { get; set; }

        // The GlyphTypeface is getting the 'Microsoft method' baseline. I changed to the 'Adobe method' baseline.
        // see https://forum.glyphsapp.com/t/why-do-glyphs-fonts-seem-to-have-so-much-space-above-the-letters/266/3
        // see https://glyphsapp.com/tutorials/vertical-metrics
        // see https://social.msdn.microsoft.com/Forums/azure/en-US/7b21047f-bce9-41d3-ab39-55ab8850caca/how-to-get-glyphtypeface-line-gap?forum=wpf
        [DataTestMethod]
        [DataRow("Roboto-Regular.ttf", 0.75)]
        [DataRow("RobotoMono-Regular.ttf", 1.0478515625)]
        [DataRow("Lora-VariableFont_wght.ttf", 1.006)]
        [DataRow("NotoSansJP-Regular.otf", 0.88)]
        [DataRow("NotoSerifJP-Regular.otf", 0.88)]
        public void Baseline_Equal_KnownValue(string fontFile, double baseline)
        {
            var tf = _typefaceCache[fontFile];
            Assert.AreEqual(tf.Baseline, baseline);
        }

        [TestMethod]
        public void StrikethroughPosition_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.StrikethroughPosition, gt.StrikethroughPosition);
            }
        }

        [TestMethod]
        public void StrikethroughThickness_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.StrikethroughThickness, gt.StrikethroughThickness);
            }
        }

        [TestMethod]
        public void UnderlinePosition_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.UnderlinePosition, gt.UnderlinePosition);
            }
        }

        [TestMethod]
        public void UnderlineThickness_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.UnderlineThickness, gt.UnderlineThickness);
            }
        }

        [TestMethod]
        public void CapsHeight_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.CapsHeight, gt.CapsHeight);
            }
        }

        // The GlyphTypeface is getting the 'Microsoft method' baseline. I changed to the 'Adobe method' baseline.
        // see https://forum.glyphsapp.com/t/why-do-glyphs-fonts-seem-to-have-so-much-space-above-the-letters/266/3
        // see https://glyphsapp.com/tutorials/vertical-metrics -> 'winAscent' and 'winDescent' are FONT RENDERING BOX, It is Not ascent or descent.
        // see https://social.msdn.microsoft.com/Forums/azure/en-US/7b21047f-bce9-41d3-ab39-55ab8850caca/how-to-get-glyphtypeface-line-gap?forum=wpf
        [DataTestMethod]
        [DataRow("Roboto-Regular.ttf", 1.0)]
        [DataRow("RobotoMono-Regular.ttf", 1.31884765625)]
        [DataRow("Lora-VariableFont_wght.ttf", 1.28)]
        [DataRow("NotoSansJP-Regular.otf", 1.0)]
        [DataRow("NotoSerifJP-Regular.otf", 1.0)]
        public void Height_Equal_KnownValue(string fontFile, double height)
        {
            var tf = _typefaceCache[fontFile];
            Assert.AreEqual(tf.Height, height);
        }

        [TestMethod]
        public void XHeight_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.XHeight, gt.XHeight);
            }
        }

        [TestMethod]
        public void GlyphCount_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                Assert.AreEqual(tf.GlyphCount, gt.GlyphCount);
            }
        }

        [TestMethod]
        public void AdvanceWidths_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.AdvanceWidths[i] != gt.AdvanceWidths[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    Assert.AreEqual(tf.AdvanceWidths[i], gt.AdvanceWidths[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void AdvanceHeights_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.AdvanceHeights[i] != gt.AdvanceHeights[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.AdvanceHeights[i], gt.AdvanceHeights[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        [TestMethod]
        public void CharacterToGlyphMap_Equal_GlyphTypefaceValue()
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

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void LeftSideBearings_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.LeftSideBearings[i] != gt.LeftSideBearings[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.LeftSideBearings[i], gt.LeftSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void RightSideBearings_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.RightSideBearings[i] != gt.RightSideBearings[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.RightSideBearings[i], gt.RightSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void TopSideBearings_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.TopSideBearings[i] != gt.TopSideBearings[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.TopSideBearings[i], gt.TopSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void BottomSideBearings_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.BottomSideBearings[i] != gt.BottomSideBearings[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.BottomSideBearings[i], gt.BottomSideBearings[i], "Font:" + fontFile + " GlyphIndex:" + i);
                }
            }
        }

        // The Typeface gives priority to the measured values, therefore provides different values ​​than GlyphTypeface.
        // [TestMethod]
        public void DistancesFromHorizontalBaselineToBlackBoxBottom_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];
                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    if (tf.DistancesFromHorizontalBaselineToBlackBoxBottom[i] != gt.DistancesFromHorizontalBaselineToBlackBoxBottom[i])
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                    }
                    // Assert.AreEqual(tf.DistancesFromHorizontalBaselineToBlackBoxBottom[i], gt.DistancesFromHorizontalBaselineToBlackBoxBottom[i], "Font:" + fontFile + " GlyphIndex:" + i);
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
        public void Constructor_NoException_IfStreamCanSeekIsFalse()
        {
            string fontPath = Path.Combine(Environment.CurrentDirectory, "Roboto-Regular.ttf");
            var stream = new Mock<MemoryStream>(File.ReadAllBytes(fontPath)) { CallBase = true };
            stream.SetupGet(x => x.CanSeek).Returns(false);
            new Typeface(stream.Object);
        }

        [TestMethod]
        public void Constructor_NoException_TrueTypeCollection()
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

        [TestMethod]
        public void GetGlyphOutline_ZeroGlyphIndex_IfGlyphIndexOutOfRange()
        {
            var gt = _glyphTypefaceCache[_fontFiles[0]];
            var tf = _typefaceCache[_fontFiles[0]];
            Assert.AreEqual(
                gt.GetGlyphOutline(ushort.MaxValue, 100, 100).ToString(),
                gt.GetGlyphOutline(0, 100, 100).ToString());

            Assert.AreEqual(
                tf.GetGlyphOutline(ushort.MaxValue, 100).ToString(),
                tf.GetGlyphOutline(0, 100).ToString());
        }

        [TestMethod]
        public void GetGlyphOutline_Equal_GlyphTypefaceValue()
        {
            foreach (string fontFile in _fontFiles)
            {
                var gt = _glyphTypefaceCache[fontFile];
                var tf = _typefaceCache[fontFile];

                for (ushort i = 0; i < tf.GlyphCount; i++)
                {
                    var sourceGeometry = tf.GetGlyphOutline(i, 128);
                    var mediaGeometry1 = ConvetToWpfPathGeometry(sourceGeometry);
                    var mediaGeometry2 = gt.GetGlyphOutline(i, 128, 128);
                    var flatGeometry1 = mediaGeometry1.GetFlattenedPathGeometry();
                    var flatGeometry2 = mediaGeometry2.GetFlattenedPathGeometry();


                    double totalDistance1 = GetTotalDistance(flatGeometry1);
                    double totalDistance2 = GetTotalDistance(flatGeometry2);
                    double errorRate = Math.Abs(totalDistance1 - totalDistance2) / totalDistance1;

                    // Over 99% match
                    if (errorRate > 0.01)
                    {
                        System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, i));
                        Assert.Fail("Match rate is less than 99%. " + "Font:" + fontFile + " GlyphIndex:" + i);
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

        // [TestMethod]
        public void OtherFont()
        {
            string fontFile = "FontAwesome.otf";
            string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
            
            using (var fontStream = File.OpenRead(fontPath))
            {
                var tf = new Typeface(fontStream);
                var gt = new GlyphTypeface(new Uri(fontPath));
            }
        }

        // For individual glyph analysis
        // [TestMethod]
        public void CreateGlyphComparisonGraph()
        {
            string fontFile = "Roboto-Regular.ttf";
            ushort glyphIndex = 60;
            System.Diagnostics.Trace.WriteLine(CreateGlyphComparison(fontFile, glyphIndex));
        }

        #region Private method

        #region Graph

        private const int GraphEmSize = 200;

        public string CreateGlyphComparison(string fontFile, ushort glyphIndex)
        {
            var renderer = new GraphRenderer();
            var tf = _typefaceCache[fontFile];
            var typefaceGeometry = tf.GetGlyphOutline(glyphIndex, GraphEmSize);
            var typefaceGraph = renderer.CreateGraph(
                typefaceGeometry,
                tf.Baseline,
                tf.AdvanceWidths[glyphIndex],
                tf.AdvanceHeights[glyphIndex],
                tf.LeftSideBearings[glyphIndex],
                tf.RightSideBearings[glyphIndex],
                tf.TopSideBearings[glyphIndex],
                tf.BottomSideBearings[glyphIndex],
                GraphEmSize);

            var gt = _glyphTypefaceCache[fontFile];

            var geometry = gt.GetGlyphOutline(glyphIndex, GraphEmSize, GraphEmSize);
            var wpfGeometry = geometry is PathGeometry ? (PathGeometry)geometry : geometry.GetOutlinedPathGeometry();
            var wpfGraph = CreateWPFGraph(
                wpfGeometry,
                gt.Baseline,
                gt.AdvanceWidths[glyphIndex],
                gt.AdvanceHeights[glyphIndex],
                gt.LeftSideBearings[glyphIndex],
                gt.RightSideBearings[glyphIndex],
                gt.TopSideBearings[glyphIndex],
                gt.BottomSideBearings[glyphIndex],
                GraphEmSize);

            return string.Format(TextResources.GlyphComparison, fontFile, glyphIndex, typefaceGraph, wpfGraph);
        }


        public string CreateWPFGraph(
            PathGeometry geometry,
            double baseline,
            double advanceWidth,
            double advanceHeight,
            double leftSideBearing,
            double rightSideBearing,
            double topSideBearing,
            double bottomSideBearing,
            int graphEmSize)
        {
            var renderer = new GraphRenderer();
            var miniLanguage = geometry.Figures.ToString();

            var graph = new StringBuilder();
            var glyphPath = string.Format(GraphResources.StrokePath, miniLanguage, renderer.GraphOffsetX, renderer.GraphOffsetY + baseline * graphEmSize);
            graph.AppendLine(glyphPath);

            // origin point
            graph.AppendLine(renderer.GetBaseline(baseline * graphEmSize));
            graph.AppendLine(renderer.GetOriginPoint(0, baseline * graphEmSize));

            // advance box
            graph.AppendLine(renderer.GetAdvanceBox(0, 0, advanceWidth * graphEmSize, advanceHeight * graphEmSize));

            // black box
            graph.AppendLine(renderer.GetBlackBox(
                leftSideBearing * graphEmSize,
                topSideBearing * graphEmSize,
                (advanceWidth - leftSideBearing - rightSideBearing) * graphEmSize,
                (advanceHeight - topSideBearing - bottomSideBearing) * graphEmSize));

            foreach (var figure in geometry.Figures)
            {
                graph.AppendLine(renderer.GetCurvePoint(figure.StartPoint.X,
                    figure.StartPoint.Y + baseline * graphEmSize,
                    figure.StartPoint.X,
                    figure.StartPoint.Y));

                foreach (var segment in figure.Segments)
                {
                    if (segment is LineSegment l)
                    {
                        graph.AppendLine(renderer.GetCurvePoint(figure.StartPoint.X,
                            figure.StartPoint.Y + baseline * graphEmSize,
                            figure.StartPoint.X,
                            figure.StartPoint.Y));
                    }
                    else if (segment is PolyLineSegment pl)
                    {
                        foreach (var p in pl.Points)
                        {
                            graph.AppendLine(renderer.GetCurvePoint(
                                p.X, 
                                p.Y + baseline * graphEmSize,
                                p.X,
                                p.Y));
                        }
                    }
                    else if (segment is QuadraticBezierSegment q)
                    {
                        graph.AppendLine(renderer.GetControlPoint(q.Point1.X,
                            q.Point1.Y + baseline * graphEmSize,
                            q.Point1.X,
                            q.Point1.Y));

                        graph.AppendLine(renderer.GetCurvePoint(q.Point2.X,
                            q.Point2.Y + baseline * graphEmSize,
                            q.Point2.X,
                            q.Point2.Y));
                    }
                    else if (segment is PolyQuadraticBezierSegment pq)
                    {
                        for (int i = 0; i < pq.Points.Count; i++)
                        {
                            if (i % 2 == 1)
                            {
                                graph.AppendLine(renderer.GetCurvePoint(
                                    pq.Points[i].X,
                                    pq.Points[i].Y + baseline * graphEmSize,
                                    pq.Points[i].X,
                                    pq.Points[i].Y));
                            }
                            else
                            {
                                graph.AppendLine(renderer.GetControlPoint(pq.Points[i].X,
                                    pq.Points[i].Y + baseline * graphEmSize,
                                    pq.Points[i].X,
                                    pq.Points[i].Y));
                            }
                        }
                    }
                    else if (segment is BezierSegment b)
                    {
                        graph.AppendLine(renderer.GetControlPoint(b.Point1.X,
                            b.Point1.Y + baseline * graphEmSize,
                            b.Point1.X,
                            b.Point1.Y));

                        graph.AppendLine(renderer.GetControlPoint(b.Point2.X,
                            b.Point2.Y + baseline * graphEmSize,
                            b.Point2.X,
                            b.Point2.Y));

                        graph.AppendLine(renderer.GetCurvePoint(b.Point3.X,
                            b.Point3.Y + baseline * graphEmSize,
                            b.Point3.X,
                            b.Point3.Y));
                    }
                    else if (segment is PolyBezierSegment pb)
                    {
                        for (int i = 0; i < pb.Points.Count; i++)
                        {
                            if (i % 3 == 2)
                            {
                                graph.AppendLine(renderer.GetCurvePoint(pb.Points[i].X,
                                    pb.Points[i].Y + baseline * graphEmSize,
                                    pb.Points[i].X,
                                    pb.Points[i].Y));
                            }
                            else
                            {
                                graph.AppendLine(renderer.GetControlPoint(pb.Points[i].X,
                                    pb.Points[i].Y + baseline * graphEmSize,
                                    pb.Points[i].X,
                                    pb.Points[i].Y));
                            }
                        }
                    }
                }
            }

            return string.Format(GraphResources.Grid, graph.ToString());
        }

        #endregion

        #region Unit test

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

        #endregion

        #endregion
    }
}
