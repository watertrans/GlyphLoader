// <copyright file="GlyphComponent.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.AAT
{
    /// <summary>
    /// The Composite glyphs in the font in the TrueType outline format.
    /// </summary>
    internal sealed class GlyphComponent
    {
        private const ushort ARG_1_AND_2_ARE_WORDS    = 0x0001;
        private const ushort ARGS_ARE_XY_VALUES       = 0x0002;
        private const ushort WE_HAVE_A_SCALE          = 0x0008;
        private const ushort WE_HAVE_AN_X_AND_Y_SCALE = 0x0040;
        private const ushort WE_HAVE_A_TWO_BY_TWO     = 0x0080;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphComponent"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        /// <param name="flags">The component flag.</param>
        internal GlyphComponent(TypefaceReader reader, ushort flags)
        {
            Flags = flags;
            GlyphIndex = reader.ReadUInt16();

            if ((flags & ARG_1_AND_2_ARE_WORDS) > 0)
            {
                if ((flags & ARGS_ARE_XY_VALUES) > 0)
                {
                    // Values are offset
                    XOffsetOrPointNumber = reader.ReadInt16();
                    YOffsetOrPointNumber = reader.ReadInt16();
                }
                else
                {
                    // Values are point number
                    XOffsetOrPointNumber = reader.ReadUInt16();
                    YOffsetOrPointNumber = reader.ReadUInt16();
                }
            }
            else
            {
                // The arguments are bytes
                if ((flags & ARGS_ARE_XY_VALUES) > 0)
                {
                    // Values are offset
                    XOffsetOrPointNumber = reader.ReadSByte();
                    YOffsetOrPointNumber = reader.ReadSByte();
                }
                else
                {
                    // Values are point number
                    XOffsetOrPointNumber = reader.ReadByte();
                    YOffsetOrPointNumber = reader.ReadByte();
                }
            }

            if ((flags & WE_HAVE_A_SCALE) > 0)
            {
                // We have a scale
                XScale = YScale = reader.ReadF2Dot14();
            }
            else if ((flags & WE_HAVE_AN_X_AND_Y_SCALE) > 0)
            {
                // We have an X / Y scale
                XScale = reader.ReadF2Dot14();
                YScale = reader.ReadF2Dot14();
            }
            else if ((flags & WE_HAVE_A_TWO_BY_TWO) > 0)
            {
                // We have a 2x2 transformation
                XScale = reader.ReadF2Dot14();
                Scale01 = reader.ReadF2Dot14();
                Scale10 = reader.ReadF2Dot14();
                YScale = reader.ReadF2Dot14();
            }
        }

        /// <summary>Gets the component flag.</summary>
        public ushort Flags { get; }

        /// <summary>Gets the glyph index of component.</summary>
        public ushort GlyphIndex { get; }

        /// <summary>Gets the x-offset for component or point number; type depends on bits 0 and 1 in component flags.</summary>
        public int XOffsetOrPointNumber { get; }

        /// <summary>Gets the y-offset for component or point number; type depends on bits 0 and 1 in component flags.</summary>
        public int YOffsetOrPointNumber { get; }

        /// <summary>Gets the x-scale for component.</summary>
        public float XScale { get; } = 1;

        /// <summary>Gets the y-scale for component.</summary>
        public float YScale { get; } = 1;

        /// <summary>Gets the 2x2 transformation.</summary>
        public float Scale01 { get; }

        /// <summary>Gets the 2x2 transformation.</summary>
        public float Scale10 { get; }
    }
}
