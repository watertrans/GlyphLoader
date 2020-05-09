// <copyright file="PathSegmentCollection.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;
using System.Text;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader.Geometry
{
    /// <summary>
    /// Represents a collection of <see cref="PathSegment"/> objects that can be individually accessed by index.
    /// </summary>
    public class PathSegmentCollection : List<PathSegment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathSegmentCollection"/> class.
        /// </summary>
        public PathSegmentCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSegmentCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection of <see cref="PathSegment"/> objects that make up the <see cref="PathSegmentCollection"/>.</param>
        public PathSegmentCollection(IEnumerable<PathSegment> collection)
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
            foreach (var segment in this)
            {
                sb.Append(segment.ToString(x, y, roundDigits));
            }
            return sb.ToString();
        }
    }
}
