using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Windows.Media;

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

        [TestMethod]
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
        //[TestMethod]
        //public void LeftSideBearings_OK_SameResultsAsGlyphTypeface()
        //{
        //    string fontPath = Path.Combine(Environment.CurrentDirectory, "NotoSansJP-Regular.otf");
        //    var gt = new GlyphTypeface(new Uri(fontPath));
        //    using (var fontStream = File.OpenRead(fontPath))
        //    {
        //        var tf = new Typeface(fontStream);
        //        for (ushort i = 0; i < tf.GlyphCount; i++)
        //        {
        //            Assert.AreEqual(gt.LeftSideBearings[i], tf.LeftSideBearings[i]);
        //        }
        //    }
        //}

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
    }
}
