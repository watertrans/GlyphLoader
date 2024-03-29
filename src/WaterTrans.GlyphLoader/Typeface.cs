﻿// <copyright file="Typeface.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Brotli;
using WaterTrans.GlyphLoader.Geometry;
using WaterTrans.GlyphLoader.Internal;
using WaterTrans.GlyphLoader.Internal.AAT;
using WaterTrans.GlyphLoader.Internal.NAME;
using WaterTrans.GlyphLoader.Internal.OpenType.CFF;
using WaterTrans.GlyphLoader.Internal.SFNT;
using WaterTrans.GlyphLoader.Internal.WOFF2;
using WaterTrans.GlyphLoader.OpenType;

namespace WaterTrans.GlyphLoader
{
    /// <summary>
    /// Main class for GlyphLoader.
    /// </summary>
    public class Typeface
    {
        private readonly Dictionary<CultureInfo, string> _copyrights = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _fontFamilyNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _fontSubfamilyNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _fullFontNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _trademarks = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _versionStrings = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _manufacturerNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _designerNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _descriptions = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _vendorUrls = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _designerUrls = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _licenseDescriptions = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _preferredFontFamilyNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _preferredFontSubfamilyNames = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<CultureInfo, string> _sampleTexts = new Dictionary<CultureInfo, string>();
        private readonly Dictionary<string, FeatureRecord> _gsubFeatures = new Dictionary<string, FeatureRecord>();
        private readonly Dictionary<string, FeatureRecord> _gposFeatures = new Dictionary<string, FeatureRecord>();
        private readonly Dictionary<ushort, GlyphData> _glyphDataCache = new Dictionary<ushort, GlyphData>();
        private readonly Dictionary<ushort, CharString> _charStringCache = new Dictionary<ushort, CharString>();
        private readonly Dictionary<string, TableDirectory> _tableDirectories = new Dictionary<string, TableDirectory>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, WOFF2TableDirectory> _woff2TableDirectories = new Dictionary<string, WOFF2TableDirectory>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort, ushort>> _singleSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort, ushort>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort[], ushort>> _ligatureSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort[], ushort>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort, ushort[]>> _multipleSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort, ushort[]>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort, ushort[]>> _alternateSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort, ushort[]>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort, Adjustment>> _singleAdjustmentMaps = new ConcurrentDictionary<string, IDictionary<ushort, Adjustment>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort[], PairAdjustment>> _pairAdjustmentMaps = new ConcurrentDictionary<string, IDictionary<ushort[], PairAdjustment>>(StringComparer.OrdinalIgnoreCase);
        private IDictionary<ushort, short> _topSideBearings;
        private IDictionary<ushort, double> _designUnitsAdvanceWidths;
        private IDictionary<ushort, double> _designUnitsLeftSideBearings;
        private IDictionary<ushort, double> _designUnitsRightSideBearings;
        private IDictionary<ushort, double> _designUnitsAdvanceHeights;
        private IDictionary<ushort, double> _designUnitsTopSideBearings;
        private IDictionary<ushort, double> _designUnitsBottomSideBearings;
        private IDictionary<ushort, double> _designUnitsDistancesFromHorizontalBaselineToBlackBoxBottom;
        private WOFF2Header _woff2Header;
        private TableOfCMAP _tableOfCMAP;
        private TableOfMAXP _tableOfMAXP;
        private TableOfHEAD _tableOfHEAD;
        private TableOfHHEA _tableOfHHEA;
        private TableOfHMTX _tableOfHMTX;
        private TableOfOS2  _tableOfOS2;
        private TableOfPOST _tableOfPOST;
        private TableOfLOCA _tableOfLOCA;
        private TableOfVHEA _tableOfVHEA;
        private TableOfVMTX _tableOfVMTX;
        private TableOfMORT _tableOfMORT;
        private TableOfGSUB _tableOfGSUB;
        private TableOfGPOS _tableOfGPOS;
        private TableOfCFF  _tableOfCFF;
        private TableOfNAME _tableOfNAME;

        /// <summary>
        /// Initializes a new instance of the <see cref="Typeface"/> class.
        /// </summary>
        /// <param name="stream">The font file stream.</param>
        public Typeface(Stream stream)
            : this(stream, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Typeface"/> class.
        /// </summary>
        /// <param name="stream">The font file stream.</param>
        /// <param name="index">TrueType collections or OpenType collections index(Zero-based numbering).</param>
        public Typeface(Stream stream, int index)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("The stream does not support reading.");
            }

            byte[] byteArray;

            if (stream is MemoryStream)
            {
                byteArray = ((MemoryStream)stream).ToArray();
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    byteArray = memoryStream.ToArray();
                }
            }

            if (IsWOFF2(byteArray))
            {
                byteArray = ReadWOFF2(byteArray, 0);
                ReadTypeface(byteArray, 0, true);
                return;
            }

            if (IsCollection(byteArray))
            {
                ReadCollectionTypeface(byteArray, index);
            }
            else
            {
                ReadTypeface(byteArray, 0, false);
            }
        }

        /// <summary>
        /// Gets whether to perform checksum calculation.
        /// </summary>
        public bool SkipChecksum { get; } = true;  // (Support in future versions)

        /// <summary>
        /// Gets the copyright information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> Copyrights
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_copyrights);
            }
        }

        /// <summary>
        /// Gets the description information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> Descriptions
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_descriptions);
            }
        }

        /// <summary>
        /// Gets the designer information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> DesignerNames
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_designerNames);
            }
        }

        /// <summary>
        /// Gets the designer URL information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> DesignerUrls
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_designerUrls);
            }
        }

        /// <summary>
        /// Gets the face name for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> FaceNames
        {
            get
            {
                // TODO Need to investigate specifications
                return new ReadOnlyDictionary<CultureInfo, string>(_preferredFontSubfamilyNames);
            }
        }

        /// <summary>
        /// Gets the family name for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> FamilyNames
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_preferredFontFamilyNames);
            }
        }

        /// <summary>
        /// Gets the font license description information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> LicenseDescriptions
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_licenseDescriptions);
            }
        }

        /// <summary>
        /// Gets the font manufacturer information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> ManufacturerNames
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_manufacturerNames);
            }
        }

        /// <summary>
        /// Gets the sample text information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> SampleTexts
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_sampleTexts);
            }
        }

        /// <summary>
        /// Gets the trademark notice information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> Trademarks
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_trademarks);
            }
        }

        /// <summary>
        /// Gets the vendor URL information for the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> VendorUrls
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_vendorUrls);
            }
        }

        /// <summary>
        /// Gets the version string information for the Typeface object interpreted from the font's 'NAME' table.
        /// </summary>
        public IDictionary<CultureInfo, string> VersionStrings
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_versionStrings);
            }
        }

        /// <summary>
        /// Gets the Win32 face name for the font represented by the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> Win32FaceNames
        {
            get
            {
                // TODO Need to investigate specifications
                return new ReadOnlyDictionary<CultureInfo, string>(_fontSubfamilyNames);
            }
        }

        /// <summary>
        /// Gets the Win32 family name for the font represented by the Typeface object.
        /// </summary>
        public IDictionary<CultureInfo, string> Win32FamilyNames
        {
            get
            {
                return new ReadOnlyDictionary<CultureInfo, string>(_fontFamilyNames);
            }
        }

        /// <summary>
        /// Gets height of character cell relative to em size.
        /// </summary>
        public double Height
        {
            get
            {
                if (_tableOfOS2 == null)
                {
                    return (double)(_tableOfHHEA.Ascender - _tableOfHHEA.Descender) / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)(_tableOfOS2.TypoAscender - _tableOfOS2.TypoDescender) / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets distance from cell top to English baseline relative to em size.
        /// </summary>
        public double Baseline
        {
            get
            {
                if (_tableOfOS2 == null)
                {
                    return (double)_tableOfHHEA.Ascender / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)_tableOfOS2.TypoAscender / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets distance from baseline to top of English capital relative to em size.
        /// </summary>
        public double CapsHeight
        {
            get
            {
                // the value may be set equal to the top of the unscaled and unhinted glyph bounding
                // box of the glyph encoded at U+0048 (LATIN CAPITAL LETTER H).
                // If no glyph is encoded in this position the field should be set to 0.
                ushort glyphIndex = CharacterToGlyphMap[0x48];

                if (_tableOfOS2 == null)
                {
                    return (double)(_tableOfHHEA.Ascender - _topSideBearings[glyphIndex]) / _tableOfHEAD.UnitsPerEm;
                }
                else if (_tableOfOS2.CapHeight == 0)
                {
                    return (double)(_tableOfOS2.TypoAscender - _topSideBearings[glyphIndex]) / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)_tableOfOS2.CapHeight / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets western x-height relative to em size.
        /// </summary>
        public double XHeight
        {
            get
            {
                // the value may be set equal to the top of the unscaled and unhinted glyph bounding
                // box of the glyph encoded at U+0078 (LATIN SMALL LETTER X).
                // If no glyph is encoded in this position the field should be set to 0.
                ushort glyphIndex = CharacterToGlyphMap[0x78];

                if (_tableOfOS2 == null)
                {
                    return (double)(_tableOfHHEA.Ascender - _topSideBearings[glyphIndex]) / _tableOfHEAD.UnitsPerEm;
                }
                else if (_tableOfOS2.XHeight == 0)
                {
                    return (double)(_tableOfOS2.TypoAscender - _topSideBearings[glyphIndex]) / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)_tableOfOS2.XHeight / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates the distance from the baseline to the strikethrough for the typeface.
        /// </summary>
        public double StrikethroughPosition
        {
            get
            {
                if (_tableOfOS2 == null)
                {
                    // TODO What should I do?
                    throw new NotImplementedException();
                }
                else
                {
                    return (double)_tableOfOS2.StrikeoutPosition / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates the thickness of the strikethrough relative to the font em size.
        /// </summary>
        public double StrikethroughThickness
        {
            get
            {
                if (_tableOfOS2 == null)
                {
                    // TODO What should I do?
                    throw new NotImplementedException();
                }
                else
                {
                    return (double)_tableOfOS2.StrikeoutSize / _tableOfHEAD.UnitsPerEm;
                }
            }
        }

        /// <summary>
        /// Gets the position of the underline in the Typeface.
        /// </summary>
        public double UnderlinePosition
        {
            get { return (double)_tableOfPOST.UnderlinePosition / _tableOfHEAD.UnitsPerEm; }
        }

        /// <summary>
        /// Gets the thickness of the underline relative to em size.
        /// </summary>
        public double UnderlineThickness
        {
            get { return (double)_tableOfPOST.UnderlineThickness / _tableOfHEAD.UnitsPerEm; }
        }

        /// <summary>
        /// Gets the advance widths for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> AdvanceWidths
        {
            get { return _designUnitsAdvanceWidths; }
        }

        /// <summary>
        /// Gets the advance heights for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> AdvanceHeights
        {
            get { return _designUnitsAdvanceHeights; }
        }

        /// <summary>
        /// Gets the distance from the leading end of the advance vector to the left edge of the black box for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> LeftSideBearings
        {
            get { return _designUnitsLeftSideBearings; }
        }

        /// <summary>
        /// Gets the distance from the right edge of the black box to the right end of the advance vector for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> RightSideBearings
        {
            get { return _designUnitsRightSideBearings; }
        }

        /// <summary>
        /// Gets the distance from the top end of the vertical advance vector to the top edge of the black box for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> TopSideBearings
        {
            get { return _designUnitsTopSideBearings; }
        }

        /// <summary>
        /// Gets the distance from bottom edge of the black box to the bottom end of the advance vector for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> BottomSideBearings
        {
            get { return _designUnitsBottomSideBearings; }
        }

        /// <summary>
        /// Gets the offset value from the horizontal Western baseline to the bottom of the glyph black box for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> DistancesFromHorizontalBaselineToBlackBoxBottom
        {
            get { return _designUnitsDistancesFromHorizontalBaselineToBlackBoxBottom; }
        }

        /// <summary>
        /// Gets the number of glyphs for the Typeface object.
        /// </summary>
        public int GlyphCount
        {
            get { return _tableOfMAXP.NumGlyphs; }
        }

        /// <summary>
        /// Gets the nominal mapping of a Unicode code point to a glyph index as defined by the font 'CMAP' table.
        /// </summary>
        public IDictionary<int, ushort> CharacterToGlyphMap
        {
            get { return _tableOfCMAP.GlyphMap; }
        }

        /// <summary>
        /// Gets the GSUB feature list.
        /// </summary>
        public IDictionary<string, FeatureRecord> GSUBFeatures
        {
            get { return new ReadOnlyDictionary<string, FeatureRecord>(_gsubFeatures); }
        }

        /// <summary>
        /// Gets the GPOS feature list.
        /// </summary>
        public IDictionary<string, FeatureRecord> GPOSFeatures
        {
            get { return new ReadOnlyDictionary<string, FeatureRecord>(_gposFeatures); }
        }

        /// <summary>
        /// Gets sfnt Major Version.
        /// </summary>
        internal ushort SfntVersionMajor { get; private set; }

        /// <summary>
        /// Gets sfnt Minor Version.
        /// </summary>
        internal ushort SfntVersionMinor { get; private set; }

        /// <summary>
        /// Gets number of tables.
        /// </summary>
        internal ushort NumTables { get; private set; }

        /// <summary>
        /// Gets (Maximum power of 2 &lt;= numTables) x 16.
        /// </summary>
        internal ushort SearchRange { get; private set; }

        /// <summary>
        /// Gets log2(maximum power of 2 &gt;= numTables).
        /// </summary>
        internal ushort EntrySelector { get; private set; }

        /// <summary>
        /// Gets numTables x 16-searchRange.
        /// </summary>
        internal ushort RangeShift { get; private set; }

        /// <summary>
        /// Gets the single substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="id">The feture record id by GSUBFeatures method result.</param>
        /// <returns>The single substitution map.</returns>
        public IDictionary<ushort, ushort> GetSingleSubstitutionMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            var record = _gsubFeatures[id];
            if (_singleSubstitutionMaps.ContainsKey(id))
            {
                return _singleSubstitutionMaps[id];
            }

            var singleSubstitutionMap = new Dictionary<ushort, ushort>();
            foreach (ushort lookupIndex in _tableOfGSUB.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var ssb in _tableOfGSUB.LookupList[lookupIndex].SingleSubstitutionList)
                {
                    singleSubstitutionMap[ssb.GlyphIndex] = ssb.SubstitutionGlyphIndex;
                }
            }
            return _singleSubstitutionMaps.GetOrAdd(id, new GlyphMapDictionary(singleSubstitutionMap));
        }

        /// <summary>
        /// Gets the single substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The single substitution map.</returns>
        public IDictionary<ushort, ushort> GetSingleSubstitutionMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gsubFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph substitution.");
            }

            return GetSingleSubstitutionMap(id);
        }

        /// <summary>
        /// Gets the ligature substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="id">The feture record id by GSUBFeatures method result.</param>
        /// <returns>The ligature substitution map.</returns>
        public IDictionary<ushort[], ushort> GetLigatureSubstitutionMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            var record = _gsubFeatures[id];
            if (_ligatureSubstitutionMaps.ContainsKey(id))
            {
                return _ligatureSubstitutionMaps[id];
            }

            var ligatureSubstitutionMap = new Dictionary<ushort[], ushort>(new GlyphIndexArrayComparer());
            foreach (ushort lookupIndex in _tableOfGSUB.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var ssb in _tableOfGSUB.LookupList[lookupIndex].LigatureSubstitutionList)
                {
                    ligatureSubstitutionMap[ssb.GlyphIndex] = ssb.SubstitutionGlyphIndex;
                }
            }
            return _ligatureSubstitutionMaps.GetOrAdd(id, new LigatureGlyphMapDictionary(ligatureSubstitutionMap));
        }

        /// <summary>
        /// Gets the ligature substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The ligature substitution map.</returns>
        public IDictionary<ushort[], ushort> GetLigatureSubstitutionMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gsubFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph substitution.");
            }

            return GetLigatureSubstitutionMap(id);
        }

        /// <summary>
        /// Gets the multiple substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="id">The feture record id by GSUBFeatures method result.</param>
        /// <returns>The multiple substitution map.</returns>
        public IDictionary<ushort, ushort[]> GetMultipleSubstitutionMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            var record = _gsubFeatures[id];
            if (_multipleSubstitutionMaps.ContainsKey(id))
            {
                return _multipleSubstitutionMaps[id];
            }

            var multipleSubstitutionMap = new Dictionary<ushort, ushort[]>();
            foreach (ushort lookupIndex in _tableOfGSUB.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var ssb in _tableOfGSUB.LookupList[lookupIndex].MultipleSubstitutionList)
                {
                    multipleSubstitutionMap[ssb.GlyphIndex] = ssb.SubstitutionGlyphIndex;
                }
            }
            return _multipleSubstitutionMaps.GetOrAdd(id, new MultipleGlyphMapDictionary(multipleSubstitutionMap));
        }

        /// <summary>
        /// Gets the multiple substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The multiple substitution map.</returns>
        public IDictionary<ushort, ushort[]> GetMultipleSubstitutionMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gsubFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph substitution.");
            }

            return GetMultipleSubstitutionMap(id);
        }

        /// <summary>
        /// Gets the alternate substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="id">The feture record id by GSUBFeatures method result.</param>
        /// <returns>The alternate substitution map.</returns>
        public IDictionary<ushort, ushort[]> GetAlternateSubstitutionMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            var record = _gsubFeatures[id];
            if (_alternateSubstitutionMaps.ContainsKey(id))
            {
                return _alternateSubstitutionMaps[id];
            }

            var alternateSubstitutionMap = new Dictionary<ushort, ushort[]>();
            foreach (ushort lookupIndex in _tableOfGSUB.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var ssb in _tableOfGSUB.LookupList[lookupIndex].AlternateSubstitutionList)
                {
                    alternateSubstitutionMap[ssb.GlyphIndex] = ssb.SubstitutionGlyphIndex;
                }
            }
            return _alternateSubstitutionMaps.GetOrAdd(id, new MultipleGlyphMapDictionary(alternateSubstitutionMap));
        }

        /// <summary>
        /// Gets the alternate substitution map by the font 'GSUB' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The alternate substitution map.</returns>
        public IDictionary<ushort, ushort[]> GetAlternateSubstitutionMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                throw new NotSupportedException("This font file does not contain the 'GSUB' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gsubFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph substitution.");
            }

            return GetAlternateSubstitutionMap(id);
        }

        /// <summary>
        /// Gets the single adjustment map by the font 'GPOS' table.
        /// </summary>
        /// <param name="id">The feture record id by GPOSFeatures method result.</param>
        /// <returns>The single adjustment map.</returns>
        public IDictionary<ushort, Adjustment> GetSingleAdjustmentMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                throw new NotSupportedException("This font file does not contain the 'GPOS' table.");
            }

            var record = _gposFeatures[id];
            if (_singleAdjustmentMaps.ContainsKey(id))
            {
                return _singleAdjustmentMaps[id];
            }

            var singleAdjustment = new Dictionary<ushort, Adjustment>();
            foreach (ushort lookupIndex in _tableOfGPOS.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var item in _tableOfGPOS.LookupList[lookupIndex].SingleAdjustmentList)
                {
                    singleAdjustment[item.GlyphIndex] = new Adjustment(
                        item.ValueRecord.XPlacement,
                        item.ValueRecord.YPlacement,
                        item.ValueRecord.XAdvance,
                        item.ValueRecord.YAdvance,
                        _tableOfHEAD.UnitsPerEm);
                }
            }
            return _singleAdjustmentMaps.GetOrAdd(id, new ReadOnlyDictionary<ushort, Adjustment>(singleAdjustment));
        }

        /// <summary>
        /// Gets the single adjustment map by the font 'GPOS' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The single adjustment map.</returns>
        public IDictionary<ushort, Adjustment> GetSingleAdjustmentMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                throw new NotSupportedException("This font file does not contain the 'GPOS' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gposFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph positioning.");
            }

            return GetSingleAdjustmentMap(id);
        }

        /// <summary>
        /// Gets the pair adjustment map by the font 'GPOS' table.
        /// </summary>
        /// <param name="id">The feture record id by GPOSFeatures method result.</param>
        /// <returns>The pair adjustment map.</returns>
        public IDictionary<ushort[], PairAdjustment> GetPairAdjustmentMap(string id)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                throw new NotSupportedException("This font file does not contain the 'GPOS' table.");
            }

            var record = _gposFeatures[id];
            if (_pairAdjustmentMaps.ContainsKey(id))
            {
                return _pairAdjustmentMaps[id];
            }

            var pairAdjustment = new Dictionary<ushort[], PairAdjustment>(new GlyphIndexArrayComparer());
            foreach (ushort lookupIndex in _tableOfGPOS.FeatureList[record.FeatureIndex].LookupListIndex)
            {
                foreach (var item in _tableOfGPOS.LookupList[lookupIndex].PairAdjustmentList)
                {
                    var key = new ushort[] { item.FirstGlyphIndex, item.SecondGlyphIndex };
                    pairAdjustment[key] = new PairAdjustment(
                        new Adjustment(
                            item.FirstValueRecord.XPlacement,
                            item.FirstValueRecord.YPlacement,
                            item.FirstValueRecord.XAdvance,
                            item.FirstValueRecord.YAdvance,
                            _tableOfHEAD.UnitsPerEm),
                        new Adjustment(
                            item.SecondValueRecord.XPlacement,
                            item.SecondValueRecord.YPlacement,
                            item.SecondValueRecord.XAdvance,
                            item.SecondValueRecord.YAdvance,
                            _tableOfHEAD.UnitsPerEm));
                }
            }
            return _pairAdjustmentMaps.GetOrAdd(id, new ReadOnlyDictionary<ushort[], PairAdjustment>(pairAdjustment));
        }

        /// <summary>
        /// Gets the pair adjustment map by the font 'GPOS' table.
        /// </summary>
        /// <param name="scriptTag">The OpenType script identification tag.</param>
        /// <param name="langSysTag">The OpenType language system identification tag.</param>
        /// <param name="featureTag">The OpenType feature identification tag.</param>
        /// <returns>The pair adjustment map.</returns>
        public IDictionary<ushort[], PairAdjustment> GetPairAdjustmentMap(string scriptTag, string langSysTag, string featureTag)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                throw new NotSupportedException("This font file does not contain the 'GPOS' table.");
            }

            string id = scriptTag + "." + langSysTag + "." + featureTag;
            if (!_gposFeatures.ContainsKey(id))
            {
                throw new NotSupportedException("This font file does not contain the argument glyph positioning.");
            }

            return GetPairAdjustmentMap(id);
        }

        /// <summary>
        /// Returns a Geometry value describing the path for a single glyph in the font.
        /// </summary>
        /// <param name="glyphIndex">The index of the glyph to get the outline for.</param>
        /// <param name="renderingEmSize">The font size in drawing surface units.</param>
        /// <returns>A <see cref="PathGeometry"/> value that represents the path of the glyph.</returns>
        public PathGeometry GetGlyphOutline(ushort glyphIndex, double renderingEmSize)
        {
            double scale = (double)renderingEmSize / _tableOfHEAD.UnitsPerEm;
            if (_tableDirectories.ContainsKey(TableNames.GLYF))
            {
                var glyph = GetGlyphData(glyphIndex);
                return glyph.ConvertToPathGeometry(scale);
            }
            else if  (_tableDirectories.ContainsKey(TableNames.CFF))
            {
                var charString = GetCharString(glyphIndex);
                return charString.ConvertToPathGeometry(scale);
            }

            // TODO not supported format
            return new PathGeometry();
        }

        private GlyphData GetGlyphData(ushort glyphIndex)
        {
            if (!_glyphDataCache.ContainsKey(glyphIndex))
            {
                return _glyphDataCache[0];
            }
            return _glyphDataCache[glyphIndex];
        }

        private CharString GetCharString(ushort glyphIndex)
        {
            if (!_charStringCache.ContainsKey(glyphIndex))
            {
                return _charStringCache[0];
            }
            return _charStringCache[glyphIndex];
        }

        private bool IsWOFF2(byte[] byteArray)
        {
            bool result;
            var reader = new TypefaceReader(byteArray, 0);
            result = reader.ReadCharArray(4) == "wOF2";
            return result;
        }

        private bool IsCollection(byte[] byteArray)
        {
            bool result;
            var reader = new TypefaceReader(byteArray, 0);
            result = reader.ReadCharArray(4) == "ttcf";
            return result;
        }

        private byte[] ReadWOFF2(byte[] byteArray, long position)
        {
            var reader = new TypefaceReader(byteArray, position);
            _woff2Header = new WOFF2Header(reader);

            if (_woff2Header.Flavor == 0x74746366)
            {
                // TODO Collection directory format
                throw new NotSupportedException();
            }

            uint offset = 0;
            for (int i = 0; i < _woff2Header.NumTables; i++)
            {
                var table = new WOFF2TableDirectory(reader, offset);
                offset += table.Length;
                _woff2TableDirectories[table.Tag] = table;
            }

            foreach (string name in TableNames.RequiedTables)
            {
                if (!_woff2TableDirectories.ContainsKey(name))
                {
                    throw new NotSupportedException("This font file does not contain the required tables.");
                }
            }

            foreach (var item in _woff2TableDirectories)
            {
                _tableDirectories[item.Value.Tag] = new TableDirectory(item.Value.Tag, 0, item.Value.Offset, item.Value.Length);
            }

            return DecodeBrotli(byteArray, (int)reader.Position, (int)_woff2Header.TotalCompressedSize);
        }

        private byte[] DecodeBrotli(byte[] input, int index, int count)
        {
            using (var inputStream = new MemoryStream(input, index, count))
            using (var bs = new BrotliStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                bs.CopyTo(outputStream);
                outputStream.Seek(0, System.IO.SeekOrigin.Begin);
                return outputStream.ToArray();
            }
        }

        private void ReadCollectionTypeface(byte[] byteArray, int index)
        {
            var reader = new TypefaceReader(byteArray, 0);
            string ttctag = reader.ReadCharArray(4);
            ushort ttcVersionMajor = reader.ReadUInt16();
            ushort ttcVersionMinor = reader.ReadUInt16();
            uint ttcDirectoryCount = reader.ReadUInt32();

            for (int i = 0; i <= Convert.ToInt32(ttcDirectoryCount - 1); i++)
            {
                uint ttcDirectoryOffset = reader.ReadUInt32();
                if (i == index)
                {
                    ReadTypeface(byteArray, ttcDirectoryOffset, false);
                    return;
                }
            }
            throw new ArgumentOutOfRangeException("Font index out of range.");
        }

        private void ReadCMAP(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.CMAP))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.CMAP].Offset;
            _tableOfCMAP = new TableOfCMAP(reader);
        }

        private void ReadMAXP(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.MAXP))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.MAXP].Offset;
            _tableOfMAXP = new TableOfMAXP(reader);
        }

        private void ReadHEAD(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HEAD))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.HEAD].Offset;
            _tableOfHEAD = new TableOfHEAD(reader);
        }

        private void ReadHHEA(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HHEA))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.HHEA].Offset;
            _tableOfHHEA = new TableOfHHEA(reader);
        }

        private void ReadHMTX(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HMTX))
            {
                return;
            }

            if (_woff2Header != null && _woff2TableDirectories[TableNames.HMTX].PreprocessingTransformationVersion == 1)
            {
                // TODO Not tested. Please provide the font file.
                reader.Position = _tableDirectories[TableNames.HMTX].Offset;
                _tableOfHMTX = new TableOfHMTX(reader, _tableOfHHEA.NumberOfHMetrics, _tableOfMAXP.NumGlyphs, true);
            }
            else
            {
                reader.Position = _tableDirectories[TableNames.HMTX].Offset;
                _tableOfHMTX = new TableOfHMTX(reader, _tableOfHHEA.NumberOfHMetrics, _tableOfMAXP.NumGlyphs, false);
            }
        }

        private void ReadOS2(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.OS2))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.OS2].Offset;
            _tableOfOS2 = new TableOfOS2(reader);
        }

        private void ReadPOST(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.POST))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.POST].Offset;
            _tableOfPOST = new TableOfPOST(reader);
        }

        private void ReadLOCA(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.LOCA) || _tableDirectories[TableNames.LOCA].Length == 0)
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.LOCA].Offset;
            _tableOfLOCA = new TableOfLOCA(reader, _tableOfMAXP.NumGlyphs, _tableOfHEAD.IndexToLocFormat);
        }

        private void ReadVHEA(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.VHEA))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.VHEA].Offset;
            _tableOfVHEA = new TableOfVHEA(reader);
        }

        private void ReadVMTX(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.VMTX))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.VMTX].Offset;
            _tableOfVMTX = new TableOfVMTX(reader, _tableOfVHEA.NumberOfVMetrics, _tableOfMAXP.NumGlyphs);
        }

        private void ReadMORT(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.MORT))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.MORT].Offset;
            _tableOfMORT = new TableOfMORT(reader);
        }

        private void ReadGSUB(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.GSUB].Offset;
            _tableOfGSUB = new TableOfGSUB(reader);
        }

        private void ReadGPOS(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.GPOS].Offset;
            _tableOfGPOS = new TableOfGPOS(reader);
        }

        private void ReadCFF(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.CFF))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.CFF].Offset;
            _tableOfCFF = new TableOfCFF(reader);

            for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
            {
                ReadCFFCharString(i);
            }
        }

        private void ReadGLYF(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GLYF))
            {
                return;
            }

            if (_woff2Header != null && _woff2TableDirectories[TableNames.GLYF].PreprocessingTransformationVersion == 0)
            {
                reader.Position = _tableDirectories[TableNames.GLYF].Offset;
                ReadGLYFTransformedGlyphData(reader);
            }
            else
            {
                for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
                {
                    ReadGLYFGlyphData(reader, i);
                }
            }
        }

        private void ReadNAME(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.NAME))
            {
                return;
            }

            reader.Position = _tableDirectories[TableNames.NAME].Offset;
            _tableOfNAME = new TableOfNAME(reader);

            foreach (var record in _tableOfNAME.NameRecords)
            {
                switch (record.PlatformID)
                {
                    case (ushort)WaterTrans.GlyphLoader.Internal.NAME.PlatformID.Unicode:
                        break;
                    case (ushort)WaterTrans.GlyphLoader.Internal.NAME.PlatformID.Macintosh:
                        break;
                    case (ushort)WaterTrans.GlyphLoader.Internal.NAME.PlatformID.ISO:
                        break;
                    case (ushort)WaterTrans.GlyphLoader.Internal.NAME.PlatformID.Microsoft:
                        switch (record.NameID)
                        {
                            case (ushort)NameID.Copyright:
                                _copyrights[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.FontFamilyName:
                                _fontFamilyNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.FontSubfamilyName:
                                _fontSubfamilyNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.FullFontName:
                                _fullFontNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.VersionString:
                                _versionStrings[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.Trademark:
                                _trademarks[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.ManufacturerName:
                                _manufacturerNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.DesignerName:
                                _designerNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.Description:
                                _descriptions[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.VendorUrl:
                                _vendorUrls[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.DesignerUrl:
                                _designerUrls[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.LicenseDescription:
                                _licenseDescriptions[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.TypographicFamilyName:
                                _preferredFontFamilyNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.TypographicSubfamilyName:
                                _preferredFontSubfamilyNames[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                            case (ushort)NameID.SampleText:
                                _sampleTexts[new CultureInfo(record.LanguageID)] = record.NameString;
                                break;
                        }
                        break;
                }
            }

            _fontFamilyNames.ToList().ForEach(x =>
            {
                if (!_preferredFontFamilyNames.ContainsKey(x.Key))
                {
                    _preferredFontFamilyNames[x.Key] = x.Value;
                }
            });

            _fontSubfamilyNames.ToList().ForEach(x =>
            {
                if (!_preferredFontSubfamilyNames.ContainsKey(x.Key))
                {
                    _preferredFontSubfamilyNames[x.Key] = x.Value;
                }
            });
        }

        private CharString ReadCFFCharString(ushort glyphIndex)
        {
            CharString result = null;

            if (_charStringCache.ContainsKey(glyphIndex))
            {
                return _charStringCache[glyphIndex];
            }

            result = _tableOfCFF.ParseCharString(glyphIndex, _tableOfHMTX.AdvanceWidths[glyphIndex]);
            _charStringCache[glyphIndex] = result;
            return result;
        }

        private void ReadGLYFTransformedGlyphData(TypefaceReader reader)
        {
            var version = reader.ReadFixed();
            var numGlyphs = reader.ReadUInt16();
            var indexFormat = reader.ReadUInt16();
            var nContourStreamSize = reader.ReadUInt32();
            var nPointsStreamSize = reader.ReadUInt32();
            var flagStreamSize = reader.ReadUInt32();
            var glyphStreamSize = reader.ReadUInt32();
            var compositeStreamSize = reader.ReadUInt32();
            var bboxStreamSize = reader.ReadUInt32();
            var instructionStreamSize = reader.ReadUInt32();

            var nContourStreamPosition = reader.Position;
            var nPointsStreamPosition = nContourStreamPosition + nContourStreamSize;
            var flagStreamPosition = nPointsStreamPosition + nPointsStreamSize;
            var glyphStreamPosition = flagStreamPosition + flagStreamSize;
            var compositeStreamPosition = glyphStreamPosition + glyphStreamSize;
            var bboxStreamPosition = compositeStreamPosition + compositeStreamSize;
            var instructionStreamPosition = bboxStreamPosition + bboxStreamSize;

            // Stream of Int16 values representing number of contours for each glyph record
            var nContourStream = new List<short>();
            var contourCount = 0;
            var compositeGlyphs = new List<ushort>();
            for (ushort i = 0; i < numGlyphs; i++)
            {
                var contour = reader.ReadInt16();
                nContourStream.Add(contour);
                if (contour > 0)
                {
                    contourCount += contour;
                }
                else if (contour < 0)
                {
                    compositeGlyphs.Add(i);
                }
            }

            if (reader.Position != nPointsStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for nContourStreamSize.");
            }

            // Stream of values representing number of outline points for each contour in glyph records
            var nPointsStream = new List<ushort>();
            for (ushort i = 0; i < contourCount; i++)
            {
                nPointsStream.Add(reader.Read255UInt16());
            }

            if (reader.Position != flagStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for nPointsStreamSize.");
            }

            // Stream of UInt8 values representing flag values for each outline point
            var flagStream = new List<byte>();
            for (ushort i = 0; i < flagStreamSize; i++)
            {
                flagStream.Add(reader.ReadByte());
            }

            if (reader.Position != glyphStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for flagStreamSize.");
            }

            // Stream of bytes representing component flag values and associated composite glyph data
            reader.Position = compositeStreamPosition;
            const ushort MORE_COMPONENTS = 0x0020;
            const ushort WE_HAVE_INSTRUCTIONS = 0x0100;
            for (ushort i = 0; i < compositeGlyphs.Count; i++)
            {
                var glyphIndex = compositeGlyphs[i];
                var glyph = new GlyphData(glyphIndex);
                glyph.NumberOfContours = nContourStream[glyphIndex];
                ushort flags = 0;
                bool moreComponents = true;
                while (moreComponents)
                {
                    flags = reader.ReadUInt16();
                    glyph.Components.Add(new GlyphComponent(reader, flags));
                    moreComponents = (flags & MORE_COMPONENTS) > 0;
                }
                if ((flags & WE_HAVE_INSTRUCTIONS) > 0)
                {
                    glyph.HasInstructions = true;
                }
                _glyphDataCache[glyphIndex] = glyph;
            }

            if (reader.Position != bboxStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for compositeStreamSize.");
            }

            // Stream of bytes representing point coordinate values using variable length encoding format
            reader.Position = glyphStreamPosition;
            int nPointsIndex = 0;
            int flagIndex = 0;
            for (ushort i = 0; i < numGlyphs; i++)
            {
                var endPtsOfContours = new List<ushort>();
                var xCoordinates = new List<short>();
                var yCoordinates = new List<short>();
                var flags = new List<byte>();
                ushort instructionLength = 0;
                int x = 0;
                int y = 0;
                ushort nPoints = 0;

                if (nContourStream[i] == 0)
                {
                    _glyphDataCache[i] = new GlyphData(i);
                }
                else if (nContourStream[i] > 0)
                {
                    // Simple Glyph
                    for (int j = 0; j < nContourStream[i]; j++)
                    {
                        nPoints += nPointsStream[nPointsIndex + j];
                        endPtsOfContours.Add((ushort)(nPoints - 1));
                    }
                    nPointsIndex += nContourStream[i];

                    for (int j = 0; j < nPoints; j++)
                    {
                        byte flag = flagStream[flagIndex + j];
                        int tripletIndex = flag & 0x7F;
                        var triplet = TripletEncoding.Items[tripletIndex];
                        byte[] coordinates = reader.ReadBytes(triplet.ByteCount - 1);

                        x += triplet.TranslateX(coordinates);
                        y += triplet.TranslateY(coordinates);

                        xCoordinates.Add((short)x);
                        yCoordinates.Add((short)y);
                        flags.Add((byte)~(flag >> 7));
                    }
                    flagIndex += nPoints;
                    instructionLength = reader.Read255UInt16();

                    var glyph = new GlyphData(i);
                    glyph.NumberOfContours = nContourStream[i];
                    glyph.EndPtsOfContours.AddRange(endPtsOfContours);
                    glyph.InstructionLength = instructionLength;
                    glyph.NumberOfCoordinates = nPoints;
                    glyph.Flags.AddRange(flags);
                    glyph.XCoordinates.AddRange(xCoordinates);
                    glyph.YCoordinates.AddRange(yCoordinates);
                    _glyphDataCache[i] = glyph;
                }
                else
                {
                    // Composite Glyph
                    if (_glyphDataCache[i].HasInstructions)
                    {
                        _glyphDataCache[i].InstructionLength = reader.Read255UInt16();
                    }
                }
            }

            if (reader.Position != compositeStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for glyphStreamSize.");
            }

            // Bitmap (a numGlyphs-long bit array) indicating explicit bounding boxes
            reader.Position = bboxStreamPosition;
            int bitmapCount = (int)(4 * Math.Floor((double)((numGlyphs + 31) / 32)));
            int bitIndex = 7;
            byte[] bboxBitmap = reader.ReadBytes(bitmapCount);
            for (ushort i = 0; i < numGlyphs; i++)
            {
                var glyph = _glyphDataCache[i];
                int bboxBitmapIndex = i / 8;
                int hasBbox = (bboxBitmap[bboxBitmapIndex] >> bitIndex) & 0x1;

                if (hasBbox == 1)
                {
                    glyph.XMin = reader.ReadInt16();
                    glyph.YMin = reader.ReadInt16();
                    glyph.XMax = reader.ReadInt16();
                    glyph.YMax = reader.ReadInt16();
                }
                else
                {
                    if (glyph.NumberOfContours < 0)
                    {
                        throw new NotSupportedException("For a composite glyph, an encoder MUST always set the corresponding bboxBitmap flag and record the original bounding box values in the bboxStream.");
                    }
                }

                if (glyph.NumberOfContours == 0 && (glyph.XMin != 0 || glyph.YMin != 0 || glyph.XMax != 0 || glyph.YMax != 0))
                {
                    throw new NotSupportedException("For glyphs records that contain zero contours, MUST the glyph bounding box values are all zeros.");
                }

                bitIndex = bitIndex > 0 ? bitIndex - 1 : 7;
            }

            if (reader.Position != instructionStreamPosition)
            {
                throw new NotSupportedException("Incorrect value for bboxStreamSize.");
            }

            // Stream of UInt8 values representing a set of instructions for each corresponding glyph
            for (ushort i = 0; i < numGlyphs; i++)
            {
                var glyph = _glyphDataCache[i];
                if (glyph.InstructionLength > 0)
                {
                    glyph.Instructions.AddRange(reader.ReadBytes(glyph.InstructionLength));
                }
            }

            if (reader.Position != instructionStreamPosition + instructionStreamSize)
            {
                throw new NotSupportedException("Incorrect value for instructionStreamSize.");
            }

            for (ushort i = 0; i < numGlyphs; i++)
            {
                CompoundGLYFTransformedGlyphData(i);
            }

            return;
        }

        private void CompoundGLYFTransformedGlyphData(ushort glyphIndex)
        {
            var glyph = _glyphDataCache[glyphIndex];
            if (glyph.Components.Count == 0)
            {
                return;
            }

            for (int i = 0; i < glyph.Components.Count; i++)
            {
                CompoundGLYFTransformedGlyphData(glyph.Components[i].GlyphIndex);
            }

            glyph.Compound(_glyphDataCache);
        }

        private GlyphData ReadGLYFGlyphData(TypefaceReader reader, ushort glyphIndex)
        {
            GlyphData result = null;

            if (_glyphDataCache.ContainsKey(glyphIndex))
            {
                return _glyphDataCache[glyphIndex];
            }

            if (_tableOfLOCA.Offsets[glyphIndex] == uint.MaxValue)
            {
                // No glyph data
                result = new GlyphData(glyphIndex);
            }
            else
            {
                reader.Position = _tableDirectories[TableNames.GLYF].Offset + _tableOfLOCA.Offsets[glyphIndex];
                result = new GlyphData(reader, glyphIndex);
            }

            _glyphDataCache[glyphIndex] = result;

            for (int i = 0; i < result.Components.Count; i++)
            {
                if (!_glyphDataCache.ContainsKey(result.Components[i].GlyphIndex))
                {
                    _glyphDataCache[result.Components[i].GlyphIndex] = ReadGLYFGlyphData(reader, result.Components[i].GlyphIndex);
                }
            }

            if (result.Components.Count > 0)
            {
                result.Compound(_glyphDataCache);
            }

            return result;
        }

        private void CalcGlyphMetrics()
        {
            _designUnitsAdvanceWidths = new GlyphMetricsDictionary<ushort>(_tableOfHMTX.AdvanceWidths, _tableOfHEAD.UnitsPerEm);
            _designUnitsLeftSideBearings = new GlyphMetricsDictionary<short>(_tableOfHMTX.LeftSideBearings, _tableOfHEAD.UnitsPerEm);

            if (_tableDirectories.ContainsKey(TableNames.CFF))
            {
                CalcCFFGlyphMetrics();
            }

            if (_tableDirectories.ContainsKey(TableNames.GLYF))
            {
                CalcGLYFGlyphMetrics();
            }
        }

        private void CalcGLYFGlyphMetrics()
        {
            short ascender;
            short descender;
            if (_tableOfOS2 == null)
            {
                ascender = _tableOfHHEA.Ascender;
                descender = _tableOfHHEA.Descender;
            }
            else
            {
                ascender = _tableOfOS2.TypoAscender;
                descender = _tableOfOS2.TypoDescender;
            }

            var glyfAdvancedHeights = new Dictionary<ushort, short>();
            var glyfLeftSideBearings = new Dictionary<ushort, short>();
            var glyfRightSideBearings = new Dictionary<ushort, short>();
            var glyfTopSideBearings = new Dictionary<ushort, short>();
            var glyfBottomSideBearings = new Dictionary<ushort, short>();
            var glyfDistancesFromHorizontalBaselineToBlackBoxBottom = new Dictionary<ushort, short>();

            for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
            {
                var glyph = GetGlyphData(i);
                glyfAdvancedHeights[i] = (short)(ascender - descender);
                glyfLeftSideBearings[i] = (short)(glyph.XCoordinates.Count > 0 ? glyph.XCoordinates.Min() : glyph.XMin);
                glyfRightSideBearings[i] = (short)(_tableOfHMTX.AdvanceWidths[i] -
                    ((glyph.XCoordinates.Count > 0 ? glyph.XCoordinates.Min() : glyph.XMin) +
                    (glyph.XCoordinates.Count > 0 ? glyph.XCoordinates.Max() : glyph.XMax) -
                    (glyph.XCoordinates.Count > 0 ? glyph.XCoordinates.Min() : glyph.XMin)));
                glyfTopSideBearings[i] = (short)(ascender -
                    (glyph.YCoordinates.Count > 0 ? glyph.YCoordinates.Max() : glyph.YMax));
                glyfBottomSideBearings[i] = (short)(Math.Abs(descender) +
                     (glyph.YCoordinates.Count > 0 ? glyph.YCoordinates.Min() : glyph.YMin));
                glyfDistancesFromHorizontalBaselineToBlackBoxBottom[i] = (short)-(glyph.YCoordinates.Count > 0 ? glyph.YCoordinates.Min() : glyph.YMin);
            }

            _topSideBearings = glyfTopSideBearings;
            _designUnitsAdvanceHeights = new GlyphMetricsDictionary<short>(glyfAdvancedHeights, _tableOfHEAD.UnitsPerEm);
            _designUnitsLeftSideBearings = new GlyphMetricsDictionary<short>(glyfLeftSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsRightSideBearings = new GlyphMetricsDictionary<short>(glyfRightSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsTopSideBearings = new GlyphMetricsDictionary<short>(glyfTopSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsBottomSideBearings = new GlyphMetricsDictionary<short>(glyfBottomSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsDistancesFromHorizontalBaselineToBlackBoxBottom = new GlyphMetricsDictionary<short>(glyfDistancesFromHorizontalBaselineToBlackBoxBottom, _tableOfHEAD.UnitsPerEm);
        }

        private void CalcCFFGlyphMetrics()
        {
            short ascender;
            short descender;
            if (_tableOfOS2 == null)
            {
                ascender = _tableOfHHEA.Ascender;
                descender = _tableOfHHEA.Descender;
            }
            else
            {
                ascender = _tableOfOS2.TypoAscender;
                descender = _tableOfOS2.TypoDescender;
            }

            var cffAdvancedWidths = new Dictionary<ushort, short>();
            var cffAdvancedHeights = new Dictionary<ushort, short>();
            var cffLeftSideBearings = new Dictionary<ushort, short>();
            var cffRightSideBearings = new Dictionary<ushort, short>();
            var cffTopSideBearings = new Dictionary<ushort, short>();
            var cffBottomSideBearings = new Dictionary<ushort, short>();
            var cffDistancesFromHorizontalBaselineToBlackBoxBottom = new Dictionary<ushort, short>();

            for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
            {
                var charString = GetCharString(i);
                cffAdvancedWidths[i] = (short)charString.Width;
                cffAdvancedHeights[i] = (short)(ascender - descender);
                cffLeftSideBearings[i] = charString.XMin;
                cffRightSideBearings[i] = (short)(charString.Width - (charString.XMin + charString.XMax - charString.XMin));
                cffTopSideBearings[i] = (short)(ascender - charString.YMax);
                cffBottomSideBearings[i] = (short)(Math.Abs(descender) + charString.YMin);
                cffDistancesFromHorizontalBaselineToBlackBoxBottom[i] = (short)-charString.YMin;
            }

            _topSideBearings = cffTopSideBearings;
            _designUnitsAdvanceWidths = new GlyphMetricsDictionary<short>(cffAdvancedWidths, _tableOfHEAD.UnitsPerEm);
            _designUnitsAdvanceHeights = new GlyphMetricsDictionary<short>(cffAdvancedHeights, _tableOfHEAD.UnitsPerEm);
            _designUnitsLeftSideBearings = new GlyphMetricsDictionary<short>(cffLeftSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsRightSideBearings = new GlyphMetricsDictionary<short>(cffRightSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsTopSideBearings = new GlyphMetricsDictionary<short>(cffTopSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsBottomSideBearings = new GlyphMetricsDictionary<short>(cffBottomSideBearings, _tableOfHEAD.UnitsPerEm);
            _designUnitsDistancesFromHorizontalBaselineToBlackBoxBottom = new GlyphMetricsDictionary<short>(cffDistancesFromHorizontalBaselineToBlackBoxBottom, _tableOfHEAD.UnitsPerEm);
        }

        private void BuildGSUBFeatureRecord()
        {
            if (_tableOfGSUB == null)
            {
                return;
            }

            foreach (var st in _tableOfGSUB.ScriptList)
            {
                foreach (var ls in st.LanguageSystemTables)
                {
                    foreach (var featureIndex in ls.FeatureIndexList)
                    {
                        var featureRecord = new FeatureRecord(st.Tag, ls.Tag, _tableOfGSUB.FeatureList[featureIndex].Tag, featureIndex);
                        _gsubFeatures[featureRecord.Id] = featureRecord;
                    }
                }
            }
        }

        private void BuildGPOSFeatureRecord()
        {
            if (_tableOfGPOS == null)
            {
                return;
            }

            foreach (var st in _tableOfGPOS.ScriptList)
            {
                foreach (var ls in st.LanguageSystemTables)
                {
                    foreach (var featureIndex in ls.FeatureIndexList)
                    {
                        var featureRecord = new FeatureRecord(st.Tag, ls.Tag, _tableOfGPOS.FeatureList[featureIndex].Tag, featureIndex);
                        _gposFeatures[featureRecord.Id] = featureRecord;
                    }
                }
            }
        }

        private void ReadTypeface(byte[] byteArray, long position, bool isWOFF2)
        {
            var reader = new TypefaceReader(byteArray, position);

            if (!isWOFF2)
            {
                ReadDirectory(reader);
                if (!SkipChecksum)
                {
                    CalcAllTableChecksum(byteArray);
                }
            }
            ReadCMAP(reader);
            ReadMAXP(reader);
            ReadHEAD(reader);
            ReadHHEA(reader);
            ReadHMTX(reader);
            ReadOS2(reader);
            ReadPOST(reader);
            ReadLOCA(reader);
            ReadVHEA(reader);
            ReadVMTX(reader);
            ReadMORT(reader);
            ReadGSUB(reader);
            ReadGPOS(reader);
            ReadCFF(reader);
            ReadGLYF(reader);
            ReadNAME(reader);
            CalcGlyphMetrics();
            BuildGSUBFeatureRecord();
            BuildGPOSFeatureRecord();
        }

        private void ReadDirectory(TypefaceReader reader)
        {
            SfntVersionMajor = reader.ReadUInt16();
            SfntVersionMinor = reader.ReadUInt16();
            NumTables = reader.ReadUInt16();
            SearchRange = reader.ReadUInt16();
            EntrySelector = reader.ReadUInt16();
            RangeShift = reader.ReadUInt16();

            for (int i = 1; i <= NumTables; i++)
            {
                string tableName = reader.ReadCharArray(4);
                var td = new TableDirectory(
                    tableName,
                    reader.ReadUInt32(),
                    reader.ReadUInt32(),
                    reader.ReadUInt32());

                _tableDirectories.Add(tableName, td);
            }

            foreach (string name in TableNames.RequiedTables)
            {
                if (!_tableDirectories.ContainsKey(name))
                {
                    throw new NotSupportedException("This font file does not contain the required tables.");
                }
            }
        }

        private void CalcAllTableChecksum(byte[] byteArray)
        {
            foreach (var item in _tableDirectories)
            {
                if (item.Key == TableNames.HEAD)
                {
                    continue;
                }

                var table = _tableDirectories[item.Key];
                var checksum = CalcTableChecksum(byteArray, table.Offset, table.Length);

                if (checksum != table.CheckSum)
                {
                    throw new NotSupportedException($"The checksums in the '{item.Key}' table did not match.");
                }
            }
        }

        private uint CalcTableChecksum(byte[] byteArray, long position, uint numberOfBytesInTable)
        {
            var reader = new TypefaceReader(byteArray, position);
            uint sum = 0;
            uint nLongs = (numberOfBytesInTable + 3) / 4;
            while (nLongs-- > 0)
            {
                sum += reader.ReadUInt32();
            }
            return sum;
        }
    }
}
