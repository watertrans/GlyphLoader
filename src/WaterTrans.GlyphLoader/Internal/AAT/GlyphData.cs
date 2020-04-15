// <copyright file="GlyphData.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The glyphs in the font in the TrueType outline format.
    /// </summary>
    internal sealed class GlyphData
    {
        private const byte ON_CURVE_POINT        = 0x01;
        private const byte X_SHORT_VECTOR        = 0x02;
        private const byte Y_SHORT_VECTOR        = 0x04;
        private const byte REPEAT_FLAG           = 0x08;
        private const byte X_IS_SAME_OR_POSITIVE = 0x10;
        private const byte Y_IS_SAME_OR_POSITIVE = 0x20;
        private const byte OVERLAP_SIMPLE        = 0x40;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphData"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal GlyphData(TypefaceReader reader)
        {
            NumberOfContours = reader.ReadInt16();
            XMin = reader.ReadInt16();
            YMin = reader.ReadInt16();
            XMax = reader.ReadInt16();
            YMax = reader.ReadInt16();

            if (NumberOfContours == 0)
            {
                return;
            }

            if (NumberOfContours > 0)
            {
                // Simple Glyph
                for (int i = 0; i < NumberOfContours; i++)
                {
                    EndPtsOfContours.Add(reader.ReadUInt16());
                }

                InstructionLength = reader.ReadUInt16();

                for (int i = 0; i < InstructionLength; i++)
                {
                    Instructions.Add(reader.ReadByte());
                }

                NumberOfCoordinates = (ushort)(EndPtsOfContours[EndPtsOfContours.Count - 1] + 1);

                for (int i = 0; i < NumberOfCoordinates; i++)
                {
                    byte flag = reader.ReadByte();
                    Flags.Add(flag);

                    if ((flag & REPEAT_FLAG) > 0)
                    {
                        int repeatCount = reader.ReadByte();
                        for (int j = 0; j < repeatCount; j++)
                        {
                            Flags.Add(flag);
                            i++;
                        }
                    }
                }

                short x = 0;

                for (var i = 0; i < NumberOfCoordinates; i++)
                {
                    byte flag = Flags[i];
                    if ((flag & X_SHORT_VECTOR) > 0)
                    {
                        if ((flag & X_IS_SAME_OR_POSITIVE) > 0)
                        {
                            x += reader.ReadByte();
                        }
                        else
                        {
                            x -= reader.ReadByte();
                        }
                    }
                    else if ((flag & X_IS_SAME_OR_POSITIVE) > 0)
                    {
                        // x is same.
                    }
                    else
                    {
                        x += reader.ReadInt16();
                    }

                    XCoordinates.Add(x);
                }

                short y = 0;

                for (var i = 0; i < NumberOfCoordinates; i++)
                {
                    byte flag = Flags[i];
                    if ((flag & Y_SHORT_VECTOR) > 0)
                    {
                        if ((flag & Y_IS_SAME_OR_POSITIVE) > 0)
                        {
                            y += reader.ReadByte();
                        }
                        else
                        {
                            y -= reader.ReadByte();
                        }
                    }
                    else if ((flag & Y_IS_SAME_OR_POSITIVE) > 0)
                    {
                        // y is same.
                    }
                    else
                    {
                        y += reader.ReadInt16();
                    }

                    YCoordinates.Add(y);
                }
            }
            else
            {
                // Composite Glyph
            }
        }

        /// <summary>Gets a number of contours. If the number of contours is greater than or equal to zero, this is a simple glyph. If negative, this is a composite glyph — the value -1 should be used for composite glyphs.</summary>
        public short NumberOfContours { get; }

        /// <summary>Gets a minimum x for coordinate data.</summary>
        public short XMin { get; }

        /// <summary>Gets a minimum y for coordinate data.</summary>
        public short YMin { get; }

        /// <summary>Gets a maximum x for coordinate data.</summary>
        public short XMax { get; }

        /// <summary>Gets a maximum y for coordinate data.</summary>
        public short YMax { get; }

        /// <summary>Gets an array of point indices for the last point of each contour, in increasing numeric order.</summary>
        public List<ushort> EndPtsOfContours { get; } = new List<ushort>();

        /// <summary>Gets the total number of bytes for instructions. If instructionLength is zero, no instructions are present for this glyph, and this field is followed directly by the flags field.</summary>
        public ushort InstructionLength { get; }

        /// <summary>Gets an array of instruction byte code for the glyph.</summary>
        public List<byte> Instructions { get; } = new List<byte>();

        /// <summary>Gets a number of coordinates.</summary>
        public ushort NumberOfCoordinates { get; }

        /// <summary>Gets an array of flag elements.</summary>
        public List<byte> Flags { get; } = new List<byte>();

        /// <summary>Gets an array of x coordinate.</summary>
        public List<short> XCoordinates { get; } = new List<short>();

        /// <summary>Gets an array of y coordinate.</summary>
        public List<short> YCoordinates { get; } = new List<short>();

        /// <summary>
        /// Convert to glyph data to <see cref="PathGeometry"/>.
        /// </summary>
        /// <param name="scale">The value by which to scale.</param>
        /// <returns>Returns the <see cref="PathGeometry"/>.</returns>
        public PathGeometry ConvertToPathGeometry(double scale)
        {
            var result = new PathGeometry();
            result.FillRule = FillRule.Nonzero;

            for (int i = 0; i < NumberOfContours; i++)
            {
                var flags = SplitContours<byte>(Flags, i);
                var xCoordinates = SplitContours<short>(XCoordinates, i);
                var yCoordinates = SplitContours<short>(YCoordinates, i);

                var figure = new PathFigure();
                figure.IsClosed = true;

                bool lastCurve = (flags[flags.Count - 1] & ON_CURVE_POINT) > 0;
                bool fistCurve = (flags[0] & ON_CURVE_POINT) > 0;
                Point lastPoint = new Point(xCoordinates[flags.Count - 1] * scale, -yCoordinates[flags.Count - 1] * scale);
                Point fistPoint = new Point(xCoordinates[0] * scale, -yCoordinates[0] * scale);

                if (lastCurve)
                {
                    figure.StartPoint = lastPoint;
                }
                else
                {
                    if (fistCurve)
                    {
                        figure.StartPoint = fistPoint;
                    }
                    else
                    {
                        figure.StartPoint = new Point((lastPoint.X + fistPoint.X) * 0.5, (lastPoint.Y + fistPoint.Y) * 0.5);
                    }
                }

                bool currentCurve;
                bool nextCurve = fistCurve;
                Point currentPoint;
                Point nextPoint = fistPoint;

                for (int j = 0; j < flags.Count; j++)
                {
                    int nextIndex = (j + 1) % flags.Count;
                    currentCurve = nextCurve;
                    currentPoint = nextPoint;
                    nextCurve = (flags[nextIndex] & ON_CURVE_POINT) > 0;
                    nextPoint = new Point(xCoordinates[nextIndex] * scale, -yCoordinates[nextIndex] * scale);

                    if (currentCurve)
                    {
                        figure.Segments.Add(new LineSegment(currentPoint, true));
                    }
                    else
                    {
                        if (!nextCurve)
                        {
                            Point middlePoint = new Point((currentPoint.X + nextPoint.X) * 0.5, (currentPoint.Y + nextPoint.Y) * 0.5);
                            figure.Segments.Add(new QuadraticBezierSegment(currentPoint, middlePoint, true));
                        }
                        else
                        {
                            figure.Segments.Add(new QuadraticBezierSegment(currentPoint, nextPoint, true));
                        }
                    }
                }

                result.Figures.Add(figure);
            }

            return result;
        }

        private List<T> SplitContours<T>(List<T> list, int index)
        {
            int rangeIndex = 0;
            for (int i = 0; i < EndPtsOfContours.Count; i++)
            {
                if (i == index)
                {
                    return list.GetRange(rangeIndex, (EndPtsOfContours[i] - rangeIndex) + 1);
                }
                rangeIndex = EndPtsOfContours[i] + 1;
            }
            return list;
        }
    }
}
