// <copyright file="Point.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents an x- and y-coordinate pair in two-dimensional space.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The x-coordinate of the new <see cref="Point"/> structure.</param>
        /// <param name="y">The y-coordinate of the new <see cref="Point"/> structure.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X-coordinate value of this <see cref="Point"/> structure.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate value of this <see cref="Point"/> structure.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Scales this Point by the specified amount.
        /// </summary>
        /// <param name="scale">The amount to scale.</param>
        /// <returns>Returns a translated value.</returns>
        public Point Scale(double scale)
        {
            return new Point(X * scale, Y * scale);
        }

        /// <summary>
        /// Transforms this Point by two by two transformation.
        /// </summary>
        /// <param name="xScale">The x-scale.</param>
        /// <param name="yScale">The y-scale.</param>
        /// <param name="scale01">The scale01.</param>
        /// <param name="scale10">The scale10.</param>
        /// <returns>Returns a transformed value.</returns>
        public Point TransformTwoByTwo(double xScale, double yScale, double scale01, double scale10)
        {
            return new Point((xScale * X) + (scale01 * Y), (scale10 * X) + (yScale * Y));
        }

        /// <summary>
        /// Translates this Point by the specified amount.
        /// </summary>
        /// <param name="x">The amount to offset the x-coordinate.</param>
        /// <param name="y">The amount to offset the y-coordinate.</param>
        /// <returns>Returns a translated value.</returns>
        public Point Offset(double x, double y)
        {
            return new Point(X + x, Y + y);
        }
    }
}
