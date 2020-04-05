// <copyright file="TableOfMORT.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using WaterTrans.GlyphLoader.Internal.AAT;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of MORT.
    /// </summary>
    internal sealed class TableOfMORT
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfMORT"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfMORT(TypefaceReader reader)
        {
            TableVersionNumberMajor = reader.ReadUInt16();
            TableVersionNumberMinor = reader.ReadUInt16();
            NChains = reader.ReadUInt32();

            for (int i = 1; i <= NChains; i++)
            {
                Chains.Add(new Chain(reader));
            }
        }

        /// <summary>Gets metamorphosis chains.</summary>
        /// <remarks>Metamorphosis chains.</remarks>
        public List<Chain> Chains { get; } = new List<Chain>();

        /// <summary>Gets TableVersionNumberMajor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets TableVersionNumberMinor.</summary>
        /// <remarks>0x00010000 for version 1.0.</remarks>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets NChains.</summary>
        /// <remarks>Number of metamorphosis chains contained in this table.</remarks>
        public uint NChains { get; }
    }
}
