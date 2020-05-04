﻿// <copyright file="Typeface.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using WaterTrans.GlyphLoader.Geometry;
using WaterTrans.GlyphLoader.Internal;
using WaterTrans.GlyphLoader.Internal.AAT;
using WaterTrans.GlyphLoader.Internal.OpenType.CFF;
using WaterTrans.GlyphLoader.Internal.SFNT;

namespace WaterTrans.GlyphLoader
{
    /// <summary>
    /// Main class for WaterTrans.GlyphLoader.
    /// </summary>
    public class Typeface
    {
        private readonly Dictionary<ushort, GlyphData> _glyphDataCache = new Dictionary<ushort, GlyphData>();
        private readonly Dictionary<ushort, CharString> _charStringCache = new Dictionary<ushort, CharString>();
        private readonly Dictionary<string, TableDirectory> _tableDirectories = new Dictionary<string, TableDirectory>(StringComparer.OrdinalIgnoreCase);
        private IDictionary<ushort, double> _alternativeAdvancedHeights;
        private IDictionary<ushort, double> _cffAdvancedWidths;
        private IDictionary<ushort, double> _designUnitsAdvanceWidths;
        private IDictionary<ushort, double> _designUnitsLeftSideBearings;
        private IDictionary<ushort, double> _designUnitsAdvanceHeights;
        private IDictionary<ushort, double> _designUnitsTopSideBearings;
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
                else if ((_tableOfOS2.Selection & 128) > 0) // USE_TYPO_METRICS
                {
                    return (double)(_tableOfOS2.TypoAscender - _tableOfOS2.TypoDescender) / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)(_tableOfOS2.WinAscent + _tableOfOS2.WinDescent) / _tableOfHEAD.UnitsPerEm;
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
                if ((_tableOfOS2.Selection & 128) > 0) // USE_TYPO_METRICS
                {
                    return (double)_tableOfOS2.TypoAscender / _tableOfHEAD.UnitsPerEm;
                }
                else
                {
                    return (double)_tableOfOS2.WinAscent / _tableOfHEAD.UnitsPerEm;
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
            get
            {
                if (_tableOfCFF == null)
                {
                    return _designUnitsAdvanceWidths;
                }
                else
                {
                    return _cffAdvancedWidths;
                }
            }
        }

        /// <summary>
        /// Gets the advance heights for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> AdvanceHeights
        {
            get
            {
                if (_tableOfVMTX == null)
                {
                    return _alternativeAdvancedHeights;
                }
                else
                {
                    return _designUnitsAdvanceHeights;
                }
            }
        }

        /// <summary>
        /// Gets the distance from the leading end of the advance vector to the left edge of the black box for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> LeftSideBearings
        {
            get { return _designUnitsLeftSideBearings; }
        }

        /// <summary>
        /// Gets the distance from the top end of the vertical advance vector to the top edge of the black box for the glyphs represented by the Typeface object.
        /// </summary>
        public IDictionary<ushort, double> TopSideBearings
        {
            get { return _designUnitsTopSideBearings; }
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
            _designUnitsAdvanceWidths = new GlyphMetricsDictionary<ushort>(_tableOfHMTX.AdvanceWidths, _tableOfHEAD.UnitsPerEm);
            _designUnitsLeftSideBearings = new GlyphMetricsDictionary<short>(_tableOfHMTX.LeftSideBearings, _tableOfHEAD.UnitsPerEm);
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
                var internalDictionary = new Dictionary<ushort, short>();
                short height;
                if (_tableOfOS2 == null)
                {
                    height = (short)(_tableOfHHEA.Ascender - _tableOfHHEA.Descender);
                }
                else
                {
                    height = (short)(_tableOfOS2.TypoAscender - _tableOfOS2.TypoDescender);
                }

                for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
                {
                    internalDictionary[i] = height;
                }
                _alternativeAdvancedHeights = new GlyphMetricsDictionary<short>(internalDictionary, _tableOfHEAD.UnitsPerEm);
                return;
            }

            reader.Position = _tableDirectories[TableNames.VMTX].Offset;
            _tableOfVMTX = new TableOfVMTX(reader, _tableOfVHEA.NumberOfVMetrics, _tableOfMAXP.NumGlyphs);
            _designUnitsAdvanceHeights = new GlyphMetricsDictionary<ushort>(_tableOfVMTX.AdvanceHeights, _tableOfHEAD.UnitsPerEm);
            _designUnitsTopSideBearings = new GlyphMetricsDictionary<short>(_tableOfVMTX.TopSideBearings, _tableOfHEAD.UnitsPerEm);
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
            ReadCFFAdvancedWidths();
        }

        private void ReadGLYF(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GLYF))
            {
                return;
            }

            for (ushort i = 0; i < _tableOfMAXP.NumGlyphs; i++)
            {
                ReadGLYF(reader, i);
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

        private void ReadCFFAdvancedWidths()
        {
            var internalDictionary = new Dictionary<ushort, int>();
            for (ushort i = 0; i < _tableOfCFF.CharStrings.Count; i++)
            {
                var charString = ReadCFFCharString(i);
                internalDictionary[i] = charString.Width;
            }
            _cffAdvancedWidths = new GlyphMetricsDictionary<int>(internalDictionary, _tableOfHEAD.UnitsPerEm);
        }

        private GlyphData ReadGLYF(TypefaceReader reader, ushort glyphIndex)
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
                    _glyphDataCache[result.Components[i].GlyphIndex] = ReadGLYF(reader, result.Components[i].GlyphIndex);
                }
            }

            if (result.Components.Count > 0)
            {
                result.Compound(_glyphDataCache);
            }

            return result;
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
