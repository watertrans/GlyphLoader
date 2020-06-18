// <copyright file="TripletEncoding.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

using System;

namespace WaterTrans.GlyphLoader.Internal.WOFF2
{
    /// <summary>
    /// The triplet encoding.
    /// </summary>
    internal class TripletEncoding
    {
        /// <summary>
        /// The triplet encoding items.
        /// </summary>
        internal static readonly TripletEncoding[] Items =
            new TripletEncoding[]
            {
                new TripletEncoding(2, 0, 8, 0, 0, 0, -1),
                new TripletEncoding(2, 0, 8, 0, 0, 0, 1),
                new TripletEncoding(2, 0, 8, 0, 256, 0, -1),
                new TripletEncoding(2, 0, 8, 0, 256, 0, 1),
                new TripletEncoding(2, 0, 8, 0, 512, 0, -1),
                new TripletEncoding(2, 0, 8, 0, 512, 0, 1),
                new TripletEncoding(2, 0, 8, 0, 768, 0, -1),
                new TripletEncoding(2, 0, 8, 0, 768, 0, 1),
                new TripletEncoding(2, 0, 8, 0, 1024, 0, -1),
                new TripletEncoding(2, 0, 8, 0, 1024, 0, 1),
                new TripletEncoding(2, 8, 0, 0, 0, -1, 0),
                new TripletEncoding(2, 8, 0, 0, 0, 1, 0),
                new TripletEncoding(2, 8, 0, 256, 0, -1, 0),
                new TripletEncoding(2, 8, 0, 256, 0, 1, 0),
                new TripletEncoding(2, 8, 0, 512, 0, -1, 0),
                new TripletEncoding(2, 8, 0, 512, 0, 1, 0),
                new TripletEncoding(2, 8, 0, 768, 0, -1, 0),
                new TripletEncoding(2, 8, 0, 768, 0, 1, 0),
                new TripletEncoding(2, 8, 0, 1024, 0, -1, 0),
                new TripletEncoding(2, 8, 0, 1024, 0, 1, 0),
                new TripletEncoding(2, 4, 4, 1, 1, -1, -1),
                new TripletEncoding(2, 4, 4, 1, 1, 1, -1),
                new TripletEncoding(2, 4, 4, 1, 1, -1, 1),
                new TripletEncoding(2, 4, 4, 1, 1, 1, 1),
                new TripletEncoding(2, 4, 4, 1, 17, -1, -1),
                new TripletEncoding(2, 4, 4, 1, 17, 1, -1),
                new TripletEncoding(2, 4, 4, 1, 17, -1, 1),
                new TripletEncoding(2, 4, 4, 1, 17, 1, 1),
                new TripletEncoding(2, 4, 4, 1, 33, -1, -1),
                new TripletEncoding(2, 4, 4, 1, 33, 1, -1),
                new TripletEncoding(2, 4, 4, 1, 33, -1, 1),
                new TripletEncoding(2, 4, 4, 1, 33, 1, 1),
                new TripletEncoding(2, 4, 4, 1, 49, -1, -1),
                new TripletEncoding(2, 4, 4, 1, 49, 1, -1),
                new TripletEncoding(2, 4, 4, 1, 49, -1, 1),
                new TripletEncoding(2, 4, 4, 1, 49, 1, 1),
                new TripletEncoding(2, 4, 4, 17, 1, -1, -1),
                new TripletEncoding(2, 4, 4, 17, 1, 1, -1),
                new TripletEncoding(2, 4, 4, 17, 1, -1, 1),
                new TripletEncoding(2, 4, 4, 17, 1, 1, 1),
                new TripletEncoding(2, 4, 4, 17, 17, -1, -1),
                new TripletEncoding(2, 4, 4, 17, 17, 1, -1),
                new TripletEncoding(2, 4, 4, 17, 17, -1, 1),
                new TripletEncoding(2, 4, 4, 17, 17, 1, 1),
                new TripletEncoding(2, 4, 4, 17, 33, -1, -1),
                new TripletEncoding(2, 4, 4, 17, 33, 1, -1),
                new TripletEncoding(2, 4, 4, 17, 33, -1, 1),
                new TripletEncoding(2, 4, 4, 17, 33, 1, 1),
                new TripletEncoding(2, 4, 4, 17, 49, -1, -1),
                new TripletEncoding(2, 4, 4, 17, 49, 1, -1),
                new TripletEncoding(2, 4, 4, 17, 49, -1, 1),
                new TripletEncoding(2, 4, 4, 17, 49, 1, 1),
                new TripletEncoding(2, 4, 4, 33, 1, -1, -1),
                new TripletEncoding(2, 4, 4, 33, 1, 1, -1),
                new TripletEncoding(2, 4, 4, 33, 1, -1, 1),
                new TripletEncoding(2, 4, 4, 33, 1, 1, 1),
                new TripletEncoding(2, 4, 4, 33, 17, -1, -1),
                new TripletEncoding(2, 4, 4, 33, 17, 1, -1),
                new TripletEncoding(2, 4, 4, 33, 17, -1, 1),
                new TripletEncoding(2, 4, 4, 33, 17, 1, 1),
                new TripletEncoding(2, 4, 4, 33, 33, -1, -1),
                new TripletEncoding(2, 4, 4, 33, 33, 1, -1),
                new TripletEncoding(2, 4, 4, 33, 33, -1, 1),
                new TripletEncoding(2, 4, 4, 33, 33, 1, 1),
                new TripletEncoding(2, 4, 4, 33, 49, -1, -1),
                new TripletEncoding(2, 4, 4, 33, 49, 1, -1),
                new TripletEncoding(2, 4, 4, 33, 49, -1, 1),
                new TripletEncoding(2, 4, 4, 33, 49, 1, 1),
                new TripletEncoding(2, 4, 4, 49, 1, -1, -1),
                new TripletEncoding(2, 4, 4, 49, 1, 1, -1),
                new TripletEncoding(2, 4, 4, 49, 1, -1, 1),
                new TripletEncoding(2, 4, 4, 49, 1, 1, 1),
                new TripletEncoding(2, 4, 4, 49, 17, -1, -1),
                new TripletEncoding(2, 4, 4, 49, 17, 1, -1),
                new TripletEncoding(2, 4, 4, 49, 17, -1, 1),
                new TripletEncoding(2, 4, 4, 49, 17, 1, 1),
                new TripletEncoding(2, 4, 4, 49, 33, -1, -1),
                new TripletEncoding(2, 4, 4, 49, 33, 1, -1),
                new TripletEncoding(2, 4, 4, 49, 33, -1, 1),
                new TripletEncoding(2, 4, 4, 49, 33, 1, 1),
                new TripletEncoding(2, 4, 4, 49, 49, -1, -1),
                new TripletEncoding(2, 4, 4, 49, 49, 1, -1),
                new TripletEncoding(2, 4, 4, 49, 49, -1, 1),
                new TripletEncoding(2, 4, 4, 49, 49, 1, 1),
                new TripletEncoding(3, 8, 8, 1, 1, -1, -1),
                new TripletEncoding(3, 8, 8, 1, 1, 1, -1),
                new TripletEncoding(3, 8, 8, 1, 1, -1, 1),
                new TripletEncoding(3, 8, 8, 1, 1, 1, 1),
                new TripletEncoding(3, 8, 8, 1, 257, -1, -1),
                new TripletEncoding(3, 8, 8, 1, 257, 1, -1),
                new TripletEncoding(3, 8, 8, 1, 257, -1, 1),
                new TripletEncoding(3, 8, 8, 1, 257, 1, 1),
                new TripletEncoding(3, 8, 8, 1, 513, -1, -1),
                new TripletEncoding(3, 8, 8, 1, 513, 1, -1),
                new TripletEncoding(3, 8, 8, 1, 513, -1, 1),
                new TripletEncoding(3, 8, 8, 1, 513, 1, 1),
                new TripletEncoding(3, 8, 8, 257, 1, -1, -1),
                new TripletEncoding(3, 8, 8, 257, 1, 1, -1),
                new TripletEncoding(3, 8, 8, 257, 1, -1, 1),
                new TripletEncoding(3, 8, 8, 257, 1, 1, 1),
                new TripletEncoding(3, 8, 8, 257, 257, -1, -1),
                new TripletEncoding(3, 8, 8, 257, 257, 1, -1),
                new TripletEncoding(3, 8, 8, 257, 257, -1, 1),
                new TripletEncoding(3, 8, 8, 257, 257, 1, 1),
                new TripletEncoding(3, 8, 8, 257, 513, -1, -1),
                new TripletEncoding(3, 8, 8, 257, 513, 1, -1),
                new TripletEncoding(3, 8, 8, 257, 513, -1, 1),
                new TripletEncoding(3, 8, 8, 257, 513, 1, 1),
                new TripletEncoding(3, 8, 8, 513, 1, -1, -1),
                new TripletEncoding(3, 8, 8, 513, 1, 1, -1),
                new TripletEncoding(3, 8, 8, 513, 1, -1, 1),
                new TripletEncoding(3, 8, 8, 513, 1, 1, 1),
                new TripletEncoding(3, 8, 8, 513, 257, -1, -1),
                new TripletEncoding(3, 8, 8, 513, 257, 1, -1),
                new TripletEncoding(3, 8, 8, 513, 257, -1, 1),
                new TripletEncoding(3, 8, 8, 513, 257, 1, 1),
                new TripletEncoding(3, 8, 8, 513, 513, -1, -1),
                new TripletEncoding(3, 8, 8, 513, 513, 1, -1),
                new TripletEncoding(3, 8, 8, 513, 513, -1, 1),
                new TripletEncoding(3, 8, 8, 513, 513, 1, 1),
                new TripletEncoding(4, 12, 12, 0, 0, -1, -1),
                new TripletEncoding(4, 12, 12, 0, 0, 1, -1),
                new TripletEncoding(4, 12, 12, 0, 0, -1, 1),
                new TripletEncoding(4, 12, 12, 0, 0, 1, 1),
                new TripletEncoding(5, 16, 16, 0, 0, -1, -1),
                new TripletEncoding(5, 16, 16, 0, 0, 1, -1),
                new TripletEncoding(5, 16, 16, 0, 0, -1, 1),
                new TripletEncoding(5, 16, 16, 0, 0, 1, 1),
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="TripletEncoding"/> class.
        /// </summary>
        /// <param name="byteCount">Byte count.</param>
        /// <param name="xbits">Number of bits used to represent X coordinate value (X bits).</param>
        /// <param name="ybits">Number of bits used to represent Y coordinate value (Y bits).</param>
        /// <param name="deltaX">An additional incremental amount to be added to X bits value (delta X).</param>
        /// <param name="deltaY">An additional incremental amount to be added to Y bits value (delta Y).</param>
        /// <param name="xsign">The sign of X coordinate value (X sign).</param>
        /// <param name="ysign">The sign of Y coordinate value (Y sign).</param>
        internal TripletEncoding(byte byteCount, byte xbits, byte ybits, ushort deltaX, ushort deltaY, sbyte xsign, sbyte ysign)
        {
            ByteCount = byteCount;
            Xbits = xbits;
            Ybits = ybits;
            DeltaX = deltaX;
            DeltaY = deltaY;
            Xsign = xsign;
            Ysign = ysign;
        }

        /// <summary>
        /// Byte count.
        /// </summary>
        public byte ByteCount { get; }

        /// <summary>
        /// Number of bits used to represent X coordinate value (X bits).
        /// </summary>
        public byte Xbits { get; }

        /// <summary>
        /// Number of bits used to represent Y coordinate value (Y bits).
        /// </summary>
        public byte Ybits { get; }

        /// <summary>
        /// An additional incremental amount to be added to X bits value (delta X).
        /// </summary>
        public ushort DeltaX { get; }

        /// <summary>
        /// An additional incremental amount to be added to Y bits value (delta Y).
        /// </summary>
        public ushort DeltaY { get; }

        /// <summary>
        /// The sign of X coordinate value (X sign).
        /// </summary>
        public sbyte Xsign { get; }

        /// <summary>
        /// The sign of Y coordinate value (Y sign).
        /// </summary>
        public sbyte Ysign { get; }

        /// <summary>
        /// Translate X.
        /// </summary>
        /// <param name="coordinates">The coordinates byte array.</param>
        /// <returns>X coordinate.</returns>
        public int TranslateX(byte[] coordinates)
        {
            switch (Xbits)
            {
                case 0:
                    return 0;
                case 4:
                    return ((coordinates[0] >> 4) + DeltaX) * Xsign;
                case 8:
                    return (coordinates[0] + DeltaX) * Xsign;
                case 12:
                    return ((coordinates[0] << 4) | (coordinates[1] >> 4) + DeltaX) * Xsign;
                case 16:
                    return (((coordinates[0] << 8) | coordinates[1]) + DeltaX) * Xsign;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Translate Y.
        /// </summary>
        /// <param name="coordinates">The coordinates byte array.</param>
        /// <returns>Y coordinate.</returns>
        public int TranslateY(byte[] coordinates)
        {
            switch (Ybits)
            {
                case 0:
                    return 0;
                case 4:
                    return ((coordinates[0] & 0xF) + DeltaY) * Ysign;
                case 8:
                    return Xbits == 0 ? (coordinates[0] + DeltaY) * Ysign : (coordinates[1] + DeltaY) * Ysign;
                case 12:
                    return ((((coordinates[1] & 0xF) << 8) | coordinates[2]) + DeltaY) * Ysign;
                case 16:
                    return (((coordinates[2] << 8) | coordinates[3]) + DeltaY) * Ysign;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
