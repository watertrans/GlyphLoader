// <copyright file="GlyphData.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The glyphs in the font in the TrueType outline format.
    /// </summary>
    internal sealed class GlyphData
    {
        // Simple Glyph
        private const byte ON_CURVE_POINT = 0x01;
        private const byte X_SHORT_VECTOR = 0x02;
        private const byte Y_SHORT_VECTOR = 0x04;
        private const byte REPEAT_FLAG = 0x08;
        private const byte X_IS_SAME_OR_POSITIVE = 0x10;
        private const byte Y_IS_SAME_OR_POSITIVE = 0x20;
        private const byte OVERLAP_SIMPLE = 0x40;

        // Composite Glyph
        private const ushort ARGS_ARE_XY_VALUES = 0x0002;
        private const ushort MORE_COMPONENTS = 0x0020;
        private const ushort WE_HAVE_INSTRUCTIONS = 0x0100;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphData"/> class.
        /// </summary>
        /// <param name="glyphIndex">The index of the glyph.</param>
        internal GlyphData(ushort glyphIndex)
        {
            GlyphIndex = glyphIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphData"/> class.
        /// </summary>
        /// <param name="glyphData">The <see cref="GlyphData"/>.</param>
        internal GlyphData(GlyphData glyphData)
        {
            GlyphIndex = glyphData.GlyphIndex;
            NumberOfContours = glyphData.NumberOfContours;
            XMin = glyphData.XMin;
            YMin = glyphData.YMin;
            XMax = glyphData.XMax;
            YMax = glyphData.YMax;
            EndPtsOfContours.AddRange(glyphData.EndPtsOfContours);
            HasInstructions = glyphData.HasInstructions;
            InstructionLength = glyphData.InstructionLength;
            Instructions.AddRange(glyphData.Instructions);
            NumberOfCoordinates = glyphData.NumberOfCoordinates;
            Flags.AddRange(glyphData.Flags);
            XCoordinates.AddRange(glyphData.XCoordinates);
            YCoordinates.AddRange(glyphData.YCoordinates);
            Components.AddRange(glyphData.Components);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphData"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="glyphIndex">The index of the glyph.</param>
        internal GlyphData(TypefaceReader reader, ushort glyphIndex)
        {
            GlyphIndex = glyphIndex;
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
                ushort flags = 0;
                bool moreComponents = true;
                while (moreComponents)
                {
                    flags = reader.ReadUInt16();
                    Components.Add(new GlyphComponent(reader, flags));
                    moreComponents = (flags & MORE_COMPONENTS) > 0;
                }
                if ((flags & WE_HAVE_INSTRUCTIONS) > 0)
                {
                    HasInstructions = true;
                    InstructionLength = reader.ReadUInt16();

                    for (int i = 0; i < InstructionLength; i++)
                    {
                        Instructions.Add(reader.ReadByte());
                    }
                }
            }
        }

        /// <summary>Gets a index of the glyph.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets a number of contours. If the number of contours is greater than or equal to zero, this is a simple glyph. If negative, this is a composite glyph — the value -1 should be used for composite glyphs.</summary>
        public short NumberOfContours { get; internal set; }

        /// <summary>Gets a minimum x for coordinate data.</summary>
        public short XMin { get; internal set; }

        /// <summary>Gets a minimum y for coordinate data.</summary>
        public short YMin { get; internal set; }

        /// <summary>Gets a maximum x for coordinate data.</summary>
        public short XMax { get; internal set; }

        /// <summary>Gets a maximum y for coordinate data.</summary>
        public short YMax { get; internal set; }

        /// <summary>Gets an array of point indices for the last point of each contour, in increasing numeric order.</summary>
        public List<ushort> EndPtsOfContours { get; } = new List<ushort>();

        /// <summary>Gets a value that indicates whether this glyph has instructions.</summary>
        public bool HasInstructions { get; internal set; }

        /// <summary>Gets the total number of bytes for instructions. If instructionLength is zero, no instructions are present for this glyph, and this field is followed directly by the flags field.</summary>
        public ushort InstructionLength { get; internal set; }

        /// <summary>Gets an array of instruction byte code for the glyph.</summary>
        public List<byte> Instructions { get; } = new List<byte>();

        /// <summary>Gets a number of coordinates.</summary>
        public ushort NumberOfCoordinates { get; internal set; }

        /// <summary>Gets an array of flag elements.</summary>
        public List<byte> Flags { get; } = new List<byte>();

        /// <summary>Gets an array of x coordinate.</summary>
        public List<short> XCoordinates { get; } = new List<short>();

        /// <summary>Gets an array of y coordinate.</summary>
        public List<short> YCoordinates { get; } = new List<short>();

        /// <summary>Gets the composite glyph components.</summary>
        public List<GlyphComponent> Components { get; } = new List<GlyphComponent>();

        /// <summary>
        /// Converts to glyph data to <see cref="PathGeometry"/>.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>Returns the <see cref="PathGeometry"/>.</returns>
        public PathGeometry ConvertToPathGeometry(double scale)
        {
            var result = new PathGeometry();
            result.FillRule = FillRule.Nonzero;

            for (int i = 0; i < NumberOfContours; i++)
            {
                var flags = GetContoursRange<byte>(Flags, i);
                var xCoordinates = GetContoursRange<short>(XCoordinates, i);
                var yCoordinates = GetContoursRange<short>(YCoordinates, i);

                var figure = new PathFigure();
                figure.IsClosed = true;

                bool lastCurve = (flags[flags.Count - 1] & ON_CURVE_POINT) > 0;
                bool fistCurve = (flags[0] & ON_CURVE_POINT) > 0;
                Point lastPoint = new Point(xCoordinates[flags.Count - 1], -yCoordinates[flags.Count - 1]).Scale(scale);
                Point fistPoint = new Point(xCoordinates[0], -yCoordinates[0]).Scale(scale);

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
                    nextPoint = new Point(xCoordinates[nextIndex], -yCoordinates[nextIndex]).Scale(scale);

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

        /// <summary>
        /// Compounds this composite glyph components.
        /// </summary>
        /// <param name="glyphCache">The cache of GlyphData.</param>
        public void Compound(IDictionary<ushort, GlyphData> glyphCache)
        {
            foreach (var component in Components)
            {
                // Clone glyph data
                var glyphData = new GlyphData(glyphCache[component.GlyphIndex]);

                if ((component.Flags & ARGS_ARE_XY_VALUES) > 0)
                {
                    // Values are offset
                    for (int i = 0; i < glyphData.NumberOfCoordinates; i++)
                    {
                        glyphData.XCoordinates[i] = (short)(Math.Truncate(component.XScale * glyphData.XCoordinates[i])
                            + Math.Truncate(component.Scale01 * glyphData.YCoordinates[i])
                            + component.XOffsetOrPointNumber);

                        glyphData.YCoordinates[i] = (short)(Math.Truncate(component.Scale10 * glyphData.XCoordinates[i])
                            + Math.Truncate(component.YScale * glyphData.YCoordinates[i])
                            + component.YOffsetOrPointNumber);
                    }
                }
                else
                {
                    // Values are point number
                    if (component.XOffsetOrPointNumber > XCoordinates.Count - 1 ||
                        component.YOffsetOrPointNumber > glyphData.NumberOfCoordinates - 1)
                    {
                        throw new IndexOutOfRangeException("Matched points out of range in " + GlyphIndex);
                    }

                    var firstPointX = XCoordinates[component.XOffsetOrPointNumber];
                    var firstPointY = YCoordinates[component.XOffsetOrPointNumber];
                    var secondPointX = (short)(Math.Truncate(component.XScale * glyphData.XCoordinates[component.YOffsetOrPointNumber])
                                     + Math.Truncate(component.Scale01 * glyphData.YCoordinates[component.YOffsetOrPointNumber]));
                    var secondPointY = (short)(Math.Truncate(component.Scale10 * glyphData.XCoordinates[component.YOffsetOrPointNumber])
                                     + Math.Truncate(component.YScale * glyphData.YCoordinates[component.YOffsetOrPointNumber]));

                    for (int i = 0; i < glyphData.NumberOfCoordinates; i++)
                    {
                        glyphData.XCoordinates[i] = (short)(Math.Truncate(component.XScale * glyphData.XCoordinates[i])
                            + Math.Truncate(component.Scale01 * glyphData.YCoordinates[i])
                            + firstPointX - secondPointX);

                        glyphData.YCoordinates[i] = (short)(Math.Truncate(component.Scale10 * glyphData.XCoordinates[i])
                            + Math.Truncate(component.YScale * glyphData.YCoordinates[i])
                            + firstPointY - secondPointY);
                    }
                }

                for (int i = 0; i < glyphData.NumberOfCoordinates; i++)
                {
                    XMin = glyphData.XCoordinates[i] < XMin ? glyphData.XCoordinates[i] : XMin;
                    XMax = XMax < glyphData.XCoordinates[i] ? glyphData.XCoordinates[i] : XMax;
                    YMin = glyphData.YCoordinates[i] < YMin ? glyphData.YCoordinates[i] : YMin;
                    YMax = YMax < glyphData.YCoordinates[i] ? glyphData.YCoordinates[i] : YMax;
                }

                EndPtsOfContours.AddRange(glyphData.EndPtsOfContours.Select(x => (ushort)(x + NumberOfCoordinates)));
                NumberOfContours = (short)EndPtsOfContours.Count;
                Flags.AddRange(glyphData.Flags);
                XCoordinates.AddRange(glyphData.XCoordinates);
                YCoordinates.AddRange(glyphData.YCoordinates);
                NumberOfCoordinates += glyphData.NumberOfCoordinates;
            }

            Components.Clear();
        }

        private List<T> GetContoursRange<T>(List<T> list, int index)
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
