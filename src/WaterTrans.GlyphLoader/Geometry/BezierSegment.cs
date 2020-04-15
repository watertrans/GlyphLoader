// <copyright file="BezierSegment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Text;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a cubic Bezier curve drawn between two points.
    /// </summary>
    public sealed class BezierSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BezierSegment"/> class.
        /// </summary>
        public BezierSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierSegment"/> class.
        /// </summary>
        /// <param name="point1">The first control point, which determines the beginning portion of the curve.</param>
        /// <param name="point2">The second control point, which determines the ending portion of the curve.</param>
        /// <param name="point3">The point to which the curve is drawn.</param>
        /// <param name="isStroked">true to stroke the curve when a Pen is used to render the segment; otherwise, false.</param>
        public BezierSegment(Point point1, Point point2, Point point3, bool isStroked)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            IsStroked = isStroked;
        }

        /// <summary>
        /// Gets or sets the first control point of the curve.
        /// </summary>
        public Point Point1 { get; set; }

        /// <summary>
        /// Gets or sets the second control point of the curve.
        /// </summary>
        public Point Point2 { get; set; }

        /// <summary>
        /// Gets or sets the end point of the curve.
        /// </summary>
        public Point Point3 { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0, 0, Constants.RoundDigits);
        }

        /// <inheritdoc />
        public override string ToString(double x, double y, int roundDigits)
        {
            var sb = new StringBuilder();
            sb.Append("C" + Math.Round(Point1.X + x, roundDigits) + "," + Math.Round(Point1.Y + y, roundDigits));
            sb.Append(" " + Math.Round(Point2.X + x, roundDigits) + "," + Math.Round(Point2.Y + y, roundDigits));
            sb.Append(" " + Math.Round(Point3.X + x, roundDigits) + "," + Math.Round(Point3.Y + y, roundDigits));
            return sb.ToString();
        }
    }
}
