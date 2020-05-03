// <copyright file="GlyphMetricsDictionary.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.SFNT
{
    /// <summary>
    /// The glyph metrics dictionary.
    /// </summary>
    /// <typeparam name="TValue">The type to internal dictionary.</typeparam>
    internal class GlyphMetricsDictionary<TValue> : IDictionary<ushort, double>
    {
        private readonly Dictionary<ushort, TValue> _internalDictionary;
        private readonly ushort _unitsPerEm;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphMetricsDictionary{TValue}"/> class.
        /// </summary>
        /// <param name="internalDictionary">The internal dictionary.</param>
        /// <param name="unitsPerEm">The units per em in ‘head’ table.</param>
        internal GlyphMetricsDictionary(Dictionary<ushort, TValue> internalDictionary, ushort unitsPerEm)
        {
            _internalDictionary = internalDictionary;
            _unitsPerEm = unitsPerEm;
        }

        /// <inheritdoc />
        public ICollection<ushort> Keys
        {
            get { return _internalDictionary.Keys; }
        }

        /// <inheritdoc />
        public ICollection<double> Values
        {
            get { throw new NotSupportedException(); }
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _internalDictionary.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc />
        public double this[ushort key]
        {
            get { return (double)Convert.ChangeType(_internalDictionary[key], typeof(double)) / _unitsPerEm; }
            set { throw new NotSupportedException(); }
        }

        /// <inheritdoc />
        public void Add(ushort key, double value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool ContainsKey(ushort key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(ushort key)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool TryGetValue(ushort key, out double value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<ushort, double> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<ushort, double> item)
        {
            return ContainsKey(item.Key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<ushort, double>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length || (arrayIndex + Count) > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            for (ushort i = 0; i < Count; ++i)
            {
                array[arrayIndex + i] = new KeyValuePair<ushort, double>(i, this[i]);
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<ushort, double> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<ushort, double>> GetEnumerator()
        {
            for (ushort i = 0; i < Count; ++i)
            {
                yield return new KeyValuePair<ushort, double>(i, this[i]);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<ushort, double>>)this).GetEnumerator();
        }
    }
}
