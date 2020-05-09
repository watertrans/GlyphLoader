// <copyright file="PathFigureCollection.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using System.Text;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a collection of <see cref="PathFigure"/> objects that collectively make up the geometry of a <see cref="PathGeometry"/>.
    /// </summary>
    public class PathFigureCollection : List<PathFigure>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathFigureCollection"/> class.
        /// </summary>
        public PathFigureCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathFigureCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection of <see cref="PathFigure"/> objects which collectively make up the geometry of the Path.</param>
        public PathFigureCollection(IEnumerable<PathFigure> collection)
        {
            AddRange(collection);
        }

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
            foreach (var figure in this)
            {
                sb.Append(figure.ToString(x, y, roundDigits));
            }
            return sb.ToString();
        }
    }
}
