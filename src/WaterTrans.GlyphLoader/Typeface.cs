// <copyright file="Typeface.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WaterTrans.GlyphLoader.Geometry;
using WaterTrans.GlyphLoader.Internal;
using WaterTrans.GlyphLoader.Internal.AAT;
using WaterTrans.GlyphLoader.Internal.OpenType;
using WaterTrans.GlyphLoader.Internal.OpenType.CFF;
using WaterTrans.GlyphLoader.Internal.SFNT;
using WaterTrans.GlyphLoader.OpenType;

namespace WaterTrans.GlyphLoader
{
    /// <summary>
    /// Main class for WaterTrans.GlyphLoader.
    /// </summary>
    public class Typeface
    {
        private readonly Dictionary<string, FeatureRecord> _gsubFeatures = new Dictionary<string, FeatureRecord>();
        private readonly Dictionary<string, FeatureRecord> _gposFeatures = new Dictionary<string, FeatureRecord>();
        private readonly Dictionary<ushort, GlyphData> _glyphDataCache = new Dictionary<ushort, GlyphData>();
        private readonly Dictionary<ushort, CharString> _charStringCache = new Dictionary<ushort, CharString>();
        private readonly Dictionary<string, TableDirectory> _tableDirectories = new Dictionary<string, TableDirectory>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort, ushort>> _singleSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort, ushort>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IDictionary<ushort[], ushort>> _ligatureSubstitutionMaps = new ConcurrentDictionary<string, IDictionary<ushort[], ushort>>(StringComparer.OrdinalIgnoreCase);
        private IDictionary<ushort, double> _designUnitsAdvanceWidths;
        private IDictionary<ushort, double> _designUnitsLeftSideBearings;
        private IDictionary<ushort, double> _designUnitsRightSideBearings;
        private IDictionary<ushort, double> _designUnitsAdvanceHeights;
        private IDictionary<ushort, double> _designUnitsTopSideBearings;
        private IDictionary<ushort, double> _designUnitsBottomSideBearings;
        private IDictionary<ushort, double> _designUnitsDistancesFromHorizontalBaselineToBlackBoxBottom;
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

            if (IsCollection(byteArray))
            {
                ReadCollectionTypeface(byteArray, index);
            }
            else
            {
                ReadTypeface(byteArray, 0);
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
                if (_tableOfOS2 == null)
                {
                    // TODO Determining the cap height by 'H' or 'O'? see https://developer.apple.com/fonts/TrueType-Reference-Manual/RM03/Chap3.html#a_whole
                    throw new NotImplementedException();
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
                if (_tableOfOS2 == null)
                {
                    // TODO Determining the x height by 'n' or 'o'? see https://developer.apple.com/fonts/TrueType-Reference-Manual/RM03/Chap3.html#a_whole
                    throw new NotImplementedException();
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
        public ReadOnlyDictionary<string, FeatureRecord> GSUBFeatures
        {
            get { return new ReadOnlyDictionary<string, FeatureRecord>(_gsubFeatures); }
        }

        /// <summary>
        /// Gets the GPOS feature list.
        /// </summary>
        public ReadOnlyDictionary<string, FeatureRecord> GPOSFeatures
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

            var ligatureSubstitutionMap = new Dictionary<ushort[], ushort>(new LigatureGlyphMapComparer());
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

        private bool IsCollection(byte[] byteArray)
        {
            bool result;
            var reader = new TypefaceReader(byteArray, 0);
            result = reader.ReadCharArray(4) == "ttcf";
            return result;
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
                    ReadTypeface(byteArray, ttcDirectoryOffset);
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

            reader.Position = _tableDirectories[TableNames.HMTX].Offset;
            _tableOfHMTX = new TableOfHMTX(reader, _tableOfHHEA.NumberOfHMetrics, _tableOfMAXP.NumGlyphs);
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
            if (!_tableDirectories.ContainsKey(TableNames.LOCA))
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

            for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
            {
                ReadGLYFGlyphData(reader, i);
            }
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

        private void ReadTypeface(byte[] byteArray, long position)
        {
            var reader = new TypefaceReader(byteArray, position);
            ReadDirectory(reader);
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
    }
}
