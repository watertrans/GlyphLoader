using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class TypefaceTest
    {
        private readonly string[] _fontFiles = {
            "NotoSansJP-Regular.otf",
            "NotoSerifJP-Regular.otf",
            "Roboto-Regular.ttf",
            "RobotoMono-Regular.ttf",
            "Lora-VariableFont_wght.ttf",
        };

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
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.Baseline, gt.Baseline);
                }
            }
        }

        [TestMethod]
        public void StrikethroughPosition_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.StrikethroughPosition, gt.StrikethroughPosition);
                }
            }
        }

        [TestMethod]
        public void StrikethroughThickness_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.StrikethroughThickness, gt.StrikethroughThickness);
                }
            }
        }

        [TestMethod]
        public void UnderlinePosition_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.UnderlinePosition, gt.UnderlinePosition);
                }
            }
        }

        [TestMethod]
        public void UnderlineThickness_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.UnderlineThickness, gt.UnderlineThickness);
                }
            }
        }

        [TestMethod]
        public void CapsHeight_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.CapsHeight, gt.CapsHeight);
                }
            }
        }

        [TestMethod]
        public void Height_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.Height, gt.Height);
                }
            }
        }

        [TestMethod]
        public void XHeight_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.XHeight, gt.XHeight);
                }
            }
        }

        [TestMethod]
        public void GlyphCount_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    Assert.AreEqual(tf.GlyphCount, gt.GlyphCount);
                }
            }
        }

        // TODO
        //[TestMethod]
        public void AdvanceWidths_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    for (ushort i = 0; i < tf.GlyphCount; i++)
                    {
                        Assert.AreEqual(tf.AdvanceWidths[i], gt.AdvanceWidths[i], "GlyphIndex:" + i);
                    }
                }
            }
        }

        // TODO
        //[TestMethod]
        public void AdvanceHeights_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    for (ushort i = 0; i < tf.GlyphCount; i++)
                    {
                        Assert.AreEqual(tf.AdvanceHeights[i], gt.AdvanceHeights[i], "GlyphIndex:" + i);
                    }
                }
            }
        }


        [TestMethod]
        public void CharacterToGlyphMap_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    foreach (int charCode in gt.CharacterToGlyphMap.Keys)
                    {
                        Assert.AreEqual(tf.CharacterToGlyphMap[charCode], gt.CharacterToGlyphMap[charCode], "CharCode:" + charCode);
                    }
                }
            }
        }

        // TODO 
        // [TestMethod]
        public void LeftSideBearings_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    for (ushort i = 0; i < tf.GlyphCount; i++)
                    {
                        Assert.AreEqual(tf.LeftSideBearings[i], gt.LeftSideBearings[i], "GlyphIndex:" + i);
                    }
                }
            }
        }

        // TODO
        // [TestMethod]
        public void TopSideBearings_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);
                    for (ushort i = 0; i < tf.GlyphCount; i++)
                    {
                        Assert.AreEqual(tf.TopSideBearings[i], gt.TopSideBearings[i], "GlyphIndex:" + i);
                    }
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

        //[TestMethod]
        public void GetGlyphOutline_OK_SameResultsAsGlyphTypeface()
        {
            foreach (string fontFile in _fontFiles)
            {
                string fontPath = Path.Combine(Environment.CurrentDirectory, fontFile);
                var gt = new GlyphTypeface(new Uri(fontPath));
                using (var fontStream = File.OpenRead(fontPath))
                {
                    var tf = new Typeface(fontStream);

                    for (ushort i = 0; i < tf.GlyphCount; i++)
                    {
                        var sourceGeometry = tf.GetGlyphOutline(i, 128);
                        var mediaGeometry1 = ConvetToWpfPathGeometry(sourceGeometry);
                        mediaGeometry1.Transform = new TranslateTransform(0, tf.Baseline * 128);

                        var mediaGeometry2 = gt.GetGlyphOutline(i, 128, 128);
                        mediaGeometry2.Transform = new TranslateTransform(0, gt.Baseline * 128);

                        var byteArray1 = RenderBitmap(mediaGeometry1);
                        var byteArray2 = RenderBitmap(mediaGeometry2);

                        int hitCount = 0;
                        for (int j = 0; j < byteArray1.Length; j++)
                        {
                            if (RoundByte(byteArray1[j]) == RoundByte(byteArray2[j]))
                            {
                                hitCount++;
                            }
                        }

                        // Match at least 99.5%
                        if (hitCount < 65208)
                        {
                            // Please see with your own eyes.
                            System.Diagnostics.Trace.WriteLine(GlyphWarningMessage
                                .Replace("{fontFile}", fontFile)
                                .Replace("{glyphIndex}", i.ToString())
                                .Replace("{pathData1}", mediaGeometry1.GetOutlinedPathGeometry().ToString().Remove(0, 2))
                                .Replace("{pathData2}", mediaGeometry2.GetOutlinedPathGeometry().ToString().Remove(0, 2)));
                        }

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                }
            }
        }

        private byte RoundByte(byte value)
        {
            if (value < 32)
            {
                return 0;
            }
            else if (value < 64)
            {
                return 31;
            }
            else if (value < 96)
            {
                return 63;
            }
            else if (value < 128)
            {
                return 95;
            }
            else if (value < 160)
            {
                return 127;
            }
            else if (value < 192)
            {
                return 159;
            }
            else if (value < 224)
            {
                return 191;
            }
            else if (value < 255)
            {
                return 223;
            }
            return 255;
        }

        private System.Windows.Media.Geometry ConvetToWpfPathGeometry(WaterTrans.GlyphLoader.Geometry.PathGeometry geometry)
        {
            return System.Windows.Media.Geometry.Parse(geometry.ToString(0, 0, 4)).Clone();
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
