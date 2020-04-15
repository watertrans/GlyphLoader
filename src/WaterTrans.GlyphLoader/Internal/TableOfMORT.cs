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

        /// <summary>Gets the metamorphosis chains.</summary>
        public List<Chain> Chains { get; } = new List<Chain>();

        /// <summary>Gets a major table version.</summary>
        public ushort TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public ushort TableVersionNumberMinor { get; }

        /// <summary>Gets a number of metamorphosis chains contained in this table.</summary>
        public uint NChains { get; }
    }
}
