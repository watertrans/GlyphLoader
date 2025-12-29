// <copyright file="QuadraticBezierSegment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Globalization;
using System.Text;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Creates a quadratic Bezier curve between two points in a <see cref="PathFigure"/>.
    /// </summary>
    public sealed class QuadraticBezierSegment : PathSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierSegment"/> class.
        /// </summary>
        public QuadraticBezierSegment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierSegment"/> class.
        /// </summary>
        /// <param name="point1">The control point of this <see cref="QuadraticBezierSegment"/>.</param>
        /// <param name="point2">The end point of this <see cref="QuadraticBezierSegment"/>.</param>
        /// <param name="isStroked">true if this <see cref="QuadraticBezierSegment"/> is to be stroked; otherwise, false.</param>
        public QuadraticBezierSegment(Point point1, Point point2, bool isStroked)
        {
            Point1 = point1;
            Point2 = point2;
            IsStroked = isStroked;
        }

        /// <summary>
        /// Gets or sets the control <see cref="Point"/> of the curve.
        /// </summary>
        public Point Point1 { get; set; }

        /// <summary>
        /// Gets or sets the end <see cref="Point"/> of this <see cref="QuadraticBezierSegment"/>.
        /// </summary>
        public Point Point2 { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0, 0, Constants.RoundDigits);
        }

        /// <inheritdoc />
        public override string ToString(double x, double y)
        {
            return ToString(x, y, Constants.RoundDigits);
        }

        /// <inheritdoc />
        public override string ToString(double x, double y, int roundDigits)
        {
            var sb = new StringBuilder();
            sb.Append("Q" + Math.Round(Point1.X + x, roundDigits).ToString(CultureInfo.InvariantCulture) + "," + Math.Round(Point1.Y + y, roundDigits).ToString(CultureInfo.InvariantCulture));
            sb.Append(" " + Math.Round(Point2.X + x, roundDigits).ToString(CultureInfo.InvariantCulture) + "," + Math.Round(Point2.Y + y, roundDigits).ToString(CultureInfo.InvariantCulture));
            return sb.ToString();
        }
    }
}
