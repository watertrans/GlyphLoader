// <copyright file="PathGeometry.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a complex shape that may be composed of arcs, curves, ellipses, lines, and rectangles.
    /// </summary>
    public sealed class PathGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathGeometry"/> class.
        /// </summary>
        public PathGeometry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathGeometry"/> class.
        /// </summary>
        /// <param name="figures">The Figures of the <see cref="PathGeometry"/> which describes the contents of the Path.</param>
        public PathGeometry(IEnumerable<PathFigure> figures)
        {
            Figures.AddRange(figures);
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="PathFigure"/> objects that describe the path's contents.
        /// </summary>
        public PathFigureCollection Figures { get; set; } = new PathFigureCollection();

        /// <summary>
        /// Gets or sets a value that determines how the intersecting areas contained in this <see cref="PathGeometry"/> are combined.
        /// </summary>
        public FillRule FillRule { get; set; } = FillRule.EvenOdd;

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
        /// <param name="roundDigits">Round by the given number of digits.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(double x, double y, int roundDigits)
        {
            return (FillRule == FillRule.Nonzero ? "F1" : "F0") + Figures.ToString(x, y, roundDigits);
        }
    }
}
