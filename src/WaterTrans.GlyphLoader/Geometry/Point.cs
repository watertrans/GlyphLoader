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
    }
}
