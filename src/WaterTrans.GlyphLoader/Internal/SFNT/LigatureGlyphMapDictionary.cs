// <copyright file="LigatureGlyphMapDictionary.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.SFNT
{
    /// <summary>
    /// The ligature glyph map dictionary.
    /// </summary>
    internal class LigatureGlyphMapDictionary : IDictionary<ushort[], ushort>
    {
        private readonly Dictionary<ushort[], ushort> _internalDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="LigatureGlyphMapDictionary"/> class.
        /// </summary>
        /// <param name="internalDictionary">The internal dictionary.</param>
        internal LigatureGlyphMapDictionary(Dictionary<ushort[], ushort> internalDictionary)
        {
            _internalDictionary = internalDictionary;
        }

        /// <inheritdoc />
        public ICollection<ushort[]> Keys
        {
            get { return _internalDictionary.Keys; }
        }

        /// <inheritdoc />
        public ICollection<ushort> Values
        {
            get { return _internalDictionary.Values; }
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
        public ushort this[ushort[] key]
        {
            get { return _internalDictionary[key]; }
            set { throw new NotSupportedException(); }
        }

        /// <inheritdoc />
        public void Add(ushort[] key, ushort value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool ContainsKey(ushort[] key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(ushort[] key)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool TryGetValue(ushort[] key, out ushort value)
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
        public void Add(KeyValuePair<ushort[], ushort> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<ushort[], ushort> item)
        {
            return ContainsKey(item.Key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<ushort[], ushort>[] array, int arrayIndex)
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

            ushort i = 0;
            foreach (var item in this)
            {
                array[arrayIndex + i] = new KeyValuePair<ushort[], ushort>(item.Key, item.Value);
                i++;
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<ushort[], ushort> item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<ushort[], ushort>> GetEnumerator()
        {
            foreach (var item in this)
            {
                yield return new KeyValuePair<ushort[], ushort>(item.Key, item.Value);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<ushort[], ushort>>)this).GetEnumerator();
        }
    }
}
