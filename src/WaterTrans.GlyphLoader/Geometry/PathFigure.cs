// <copyright file="PathFigure.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a complex shape that may be composed of arcs, curves, ellipses, lines, and rectangles.
    /// </summary>
    public sealed class PathFigure
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathFigure"/> class.
        /// </summary>
        public PathFigure()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFigure"/> class.
        /// </summary>
        /// <param name="start">The StartPoint for the <see cref="PathFigure"/>.</param>
        /// <param name="segments">The Segments for the <see cref="PathFigure"/>.</param>
        /// <param name="closed">The IsClosed for the <see cref="PathFigure"/>.</param>
        public PathFigure(Point start, IEnumerable<PathSegment> segments, bool closed)
        {
            StartPoint = start;
            Segments.AddRange(segments);
            IsClosed = closed;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether this figures first and last segments are connected.
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Gets or sets whether the contained area of this <see cref="PathFigure"/> is to be used for hit-testing, rendering, and clipping.
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Gets or sets the collection of segments that define the shape of this <see cref="PathFigure"/> object.
        /// </summary>
        public PathSegmentCollection Segments { get; set; } = new PathSegmentCollection();

        /// <summary>
        /// Gets or sets the <see cref="Point"/> where the <see cref="PathFigure"/> begins.
        /// </summary>
        public Point StartPoint { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0, 0, Constants.RoundDigits);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="x">X coordinates offset.</param>
        /// <param name="y">Y coordinates offset.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(double x, double y)
        {
            return ToString(x, y, Constants.RoundDigits);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="x">X coordinates offset.</param>
        /// <param name="y">Y coordinates offset.</param>
        /// <param name="roundDigits">Round by the given number of digits.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(double x, double y, int roundDigits)
        {
            var sb = new StringBuilder();
            sb.Append("M" + Math.Round(StartPoint.X + x, roundDigits) + "," + Math.Round(StartPoint.Y + y, roundDigits));
            sb.Append(Segments.ToString(x, y, roundDigits));
            sb.Append("z ");
            return sb.ToString();
        }
    }
}
