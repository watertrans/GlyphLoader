// <copyright file="PathSegment.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a segment of a <see cref="PathFigure"/> object.
    /// </summary>
    public abstract class PathSegment
    {
        /// <summary>
        /// Gets or sets a value that indicates whether the segment is stroked.
        /// </summary>
        public bool IsStroked { get; set; }

        /// <inheritdoc />
        public abstract override string ToString();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="x">X coordinates offset.</param>
        /// <param name="y">Y coordinates offset.</param>
        /// <returns>A string that represents the current object.</returns>
        public abstract string ToString(double x, double y);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="x">X coordinates offset.</param>
        /// <param name="y">Y coordinates offset.</param>
        /// <param name="roundDigits">Round by the given number of digits.</param>
        /// <returns>A string that represents the current object.</returns>
        public abstract string ToString(double x, double y, int roundDigits);
    }
}
