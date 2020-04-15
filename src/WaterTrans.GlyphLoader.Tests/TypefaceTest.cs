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
            "Roboto-Regular.ttf", 
            "RobotoMono-Regular.ttf", 
            "NotoSansJP-Regular.otf", 
            "NotoSerifJP-Regular.otf", 
            "Lora-VariableFont_wght.ttf",
        };

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

        // TODO CFF AdvanceWidths
        // [TestMethod]
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
                        Assert.AreEqual(gt.AdvanceWidths[i], tf.AdvanceWidths[i]);
                    }
                }
            }
        }

        // TODO CFF AdvanceHeights
        // [TestMethod]
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
                        Assert.AreEqual(gt.AdvanceHeights[i], tf.AdvanceHeights[i]);
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
                        Assert.AreEqual(gt.CharacterToGlyphMap[charCode], tf.CharacterToGlyphMap[charCode]);
                    }
                }
            }
        }

        // TODO CFF LeftSideBearings
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
                        Assert.AreEqual(gt.LeftSideBearings[i], tf.LeftSideBearings[i]);
                    }
                }
            }
        }

        // TODO CFF TopSideBearings 
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
                        Assert.AreEqual(gt.TopSideBearings[i], tf.TopSideBearings[i]);
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

        [TestMethod]
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
                        var sourceGeometry = tf.GetGlyphOutline(i, 128, 128);
                        var mediaGeometry1 = ConvetToWpfPathGeometry(sourceGeometry);
                        mediaGeometry1.Transform = new TranslateTransform(0, tf.Baseline * 128);

                        var mediaGeometry2 = gt.GetGlyphOutline(i, 128, 128);
                        mediaGeometry2.Transform = new TranslateTransform(0, tf.Baseline * 128);

                        var byteArray1 = RenderBitmap(mediaGeometry1);
                        var byteArray2 = RenderBitmap(mediaGeometry2);

                        int hitCount = 0;
                        for (int j = 0; j < byteArray1.Length; j++)
                        {
                            if (byteArray1[j] == byteArray2[j])
                            {
                                hitCount++;
                            }
                        }

                        // Match at least 98%
                        Assert.IsTrue(hitCount >= 64225,
                            " [GlyphId]" + i +
                            " [Typeface]" + mediaGeometry1.ToString() +
                            " [GlyphTypeface]" + mediaGeometry2.ToString());
                    }
                }
            }
        }

        private System.Windows.Media.Geometry ConvetToWpfPathGeometry(WaterTrans.GlyphLoader.Geometry.PathGeometry geometry)
        {
            return System.Windows.Media.Geometry.Parse(geometry.ToString()).Clone();
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
