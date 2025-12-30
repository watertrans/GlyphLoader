using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader.Tests
{
    [TestClass]
    public class TypefaceReaderTest
    {
        [TestMethod]
        [DataRow((ushort)506, new byte[] { 255, 253 })]
        [DataRow((ushort)506, new byte[] { 254, 0 })]
        [DataRow((ushort)506, new byte[] { 253, 1, 250 })]
        public void Read255UInt16_Equal_KnownValue(ushort expected, byte[] byteArray)
        {
            var reader = new TypefaceReader(byteArray, 0);
            Assert.AreEqual(expected, reader.Read255UInt16());
        }

        [TestMethod]
        [DataRow((uint)63, new byte[] { 63 })]
        [DataRow((uint)4294967295, new byte[] { 143, 255, 255, 255, 127 })]
        public void ReadUIntBase128_Equal_KnownValue(uint expected, byte[] byteArray)
        {
            var reader = new TypefaceReader(byteArray, 0);
            Assert.AreEqual(expected, reader.ReadUIntBase128());
        }

        [TestMethod]
        [DataRow(new byte[] { 128, 63 })]
        [DataRow(new byte[] { 255, 128, 128, 128, 0 })]
        [DataRow(new byte[] { 143, 128, 128, 128, 128 })]
        public void ReadUIntBase128_Throws_FormatException(byte[] byteArray)
        {
            var reader = new TypefaceReader(byteArray, 0);
            Assert.Throws<FormatException>(() => reader.ReadUIntBase128());
        }
    }
}
