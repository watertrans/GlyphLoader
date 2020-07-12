// <copyright file="NameID.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

namespace WaterTrans.GlyphLoader.Internal.NAME
{
    /// <summary>The NAME NameRecord NameID type enumeration.</summary>
    internal enum NameID : ushort
    {
        /// <summary>Copyright notice.</summary>
        Copyright = 0,

        /// <summary>Font Family name.</summary>
        FontFamilyName = 1,

        /// <summary>Font Subfamily name.</summary>
        FontSubfamilyName = 2,

        /// <summary>Unique font identifier.</summary>
        UniqueFontIdentifier = 3,

        /// <summary>Full font name that reflects all family and relevant subfamily descriptors.</summary>
        FullFontName = 4,

        /// <summary>Version string.</summary>
        VersionString = 5,

        /// <summary>PostScript name for the font.</summary>
        PostScriptName = 6,

        /// <summary>Trademark.</summary>
        Trademark = 7,

        /// <summary>Manufacturer Name.</summary>
        ManufacturerName = 8,

        /// <summary>Designer.</summary>
        DesignerName = 9,

        /// <summary>Description.</summary>
        Description = 10,

        /// <summary>URL Vendor.</summary>
        VendorUrl = 11,

        /// <summary>URL Designer.</summary>
        DesignerUrl = 12,

        /// <summary>License Description.</summary>
        LicenseDescription = 13,

        /// <summary>License Info URL.</summary>
        LicenseInfoUrl = 14,

        /// <summary>Reserved.</summary>
        Reserved = 15,

        /// <summary>Typographic Family name.</summary>
        TypographicFamilyName = 16,

        /// <summary>Typographic Subfamily name.</summary>
        TypographicSubfamilyName = 17,

        /// <summary>Compatible Full.</summary>
        CompatibleFull = 18,

        /// <summary>Sample text.</summary>
        SampleText = 19,

        /// <summary>PostScript CID findfont name.</summary>
        PostScriptCIDFindFontName = 20,

        /// <summary>WWS Family Name.</summary>
        WWSFamilyName = 21,

        /// <summary>WWS Subfamily Name.</summary>
        WWSSubfamilyName = 22,

        /// <summary>Light Background Palette.</summary>
        LightBackgroundPalette = 23,

        /// <summary>Dark Background Palette.</summary>
        DarkBackgroundPalette = 24,

        /// <summary>Variations PostScript Name Prefix.</summary>
        VariationsPostScriptNamePrefix = 25,
    }
}
