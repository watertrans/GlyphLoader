// <copyright file="LineSegment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Creates a line between two points in a <see cref="PathFigure"/>.
    /// </summary>
    public sealed class LineSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment"/> class.
        /// </summary>
        public LineSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment"/> class.
        /// </summary>
        /// <param name="point">The end point of this <see cref="LineSegment"/>.</param>
        /// <param name="isStroked">true to stroke this LineSegment; otherwise, false.</param>
        public LineSegment(Point point, bool isStroked)
        {
            Point = point;
            IsStroked = isStroked;
        }

        /// <summary>
        /// Gets or sets the end point of the line segment.
        /// </summary>
        public Point Point { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0, 0, Constants.RoundDigits);
        }

        /// <inheritdoc />
        public override string ToString(double x, double y, int roundDigits)
        {
            return "L" + Math.Round(Point.X + x, roundDigits) + "," + Math.Round(Point.Y + y, roundDigits);
        }
    }
}
