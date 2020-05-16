// <copyright file="TableOfCFF.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using WaterTrans.GlyphLoader.Internal.OpenType.CFF;

namespace WaterTrans.GlyphLoader.Internal
{
    /// <summary>
    /// Table of CFF.
    /// </summary>
    internal sealed class TableOfCFF
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfCFF"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="TypefaceReader"/>.</param>
        internal TableOfCFF(TypefaceReader reader)
        {
            var startPosition = reader.Position;
            TableVersionNumberMajor = reader.ReadByte();
            TableVersionNumberMinor = reader.ReadByte();
            HeaderSize = reader.ReadByte();
            Offset = reader.ReadByte();

            reader.Position = startPosition + HeaderSize;
            NameIndex = new IndexDataOfName(reader);

            if (NameIndex.Objects.Count != 1)
            {
                // https://docs.microsoft.com/ja-jp/typography/opentype/otspec182/cff
                // The Name INDEX in the CFF data must contain only one entry; that is, there must be only one font in the CFF FontSet.
                throw new NotSupportedException("The Name INDEX in the CFF data must contain only one entry.");
            }

            TopDictionaryIndex = new IndexDataOfTopDictionary(reader);

            if (TopDictionaryIndex.Objects.Count != 1)
            {
                // https://wwwimages2.adobe.com/content/dam/acom/en/devnet/font/pdfs/5176.CFF.pdf
                // This contains the top-level DICTs of all the fonts in the FontSet stored in an INDEX structure.
                // Objects contained within this INDEX correspond to those in the Name INDEX in both order and number.
                throw new NotSupportedException("The Top DICT INDEX in the CFF data must contain only one entry.");
            }

            StringIndex = new IndexDataOfString(reader);
            GlobalSubroutines = new IndexDataOfSubroutines(reader);
            TopDictionary = new DictionaryData(TopDictionaryIndex.Objects[0], StringIndex.Strings, false);

            if (TopDictionary.ContainsKey("CharstringType") && (int)TopDictionary["CharstringType"] != 2)
            {
                // https://docs.microsoft.com/ja-jp/typography/opentype/otspec182/cff
                // The CFF Top DICT must specify a CharstringType value of 2.
                throw new NotSupportedException("The CFF Top DICT must specify a CharstringType value of 2.");
            }

            if (TopDictionary.ContainsKey("Private"))
            {
                // TODO Not tested. Please provide the font file.
                int privateSize = ((int[])TopDictionary["Private"])[0];
                int privateOffset = ((int[])TopDictionary["Private"])[1];
                reader.Position = startPosition + privateOffset;
                TopPrivateDictionary = new DictionaryData(reader.ReadBytes(privateSize), StringIndex.Strings, true);

                if (TopPrivateDictionary.ContainsKey("Subrs"))
                {
                    int localSubrsOffset = (int)TopPrivateDictionary["Subrs"];
                    reader.Position = startPosition + privateOffset + localSubrsOffset;
                    TopLocalSubroutines = new IndexDataOfSubroutines(reader);
                }
            }

            reader.Position = startPosition + (int)TopDictionary["CharStrings"];
            CharStrings = new IndexDataOfCharStrings(reader);

            // The CIDFonts require the additional Top DICT operators.
            if (TopDictionary.ContainsKey("ROS") && TopDictionary.ContainsKey("FDSelect") && TopDictionary.ContainsKey("FDArray"))
            {
                reader.Position = startPosition + (int)TopDictionary["FDArray"];
                FontDictionaryIndex = new IndexDataOfFontDictionary(reader);

                for (int i = 0; i < FontDictionaryIndex.Objects.Count; i++)
                {
                    FontDictionaries.Add(new DictionaryData(FontDictionaryIndex.Objects[i], StringIndex.Strings, false));
                }

                for (int i = 0; i < FontDictionaryIndex.Objects.Count; i++)
                {
                    if (FontDictionaries[i].ContainsKey("Private"))
                    {
                        int privateSize = ((int[])FontDictionaries[i]["Private"])[0];
                        int privateOffset = ((int[])FontDictionaries[i]["Private"])[1];
                        reader.Position = startPosition + privateOffset;
                        FontPrivateDictionaries.Add(new DictionaryData(reader.ReadBytes(privateSize), StringIndex.Strings, true));

                        if (FontPrivateDictionaries[i].ContainsKey("Subrs"))
                        {
                            int localSubrsOffset = (int)FontPrivateDictionaries[i]["Subrs"];
                            reader.Position = startPosition + privateOffset + localSubrsOffset;
                            LocalSubroutinesList.Add(new IndexDataOfSubroutines(reader));
                        }
                        else
                        {
                            LocalSubroutinesList.Add(null);
                        }
                    }
                    else
                    {
                        FontPrivateDictionaries.Add(null);
                        LocalSubroutinesList.Add(null);
                    }
                }

                reader.Position = startPosition + (int)TopDictionary["FDSelect"];
                var format = reader.ReadByte();

                if (format == 0)
                {
                    for (ushort i = 0; i < CharStrings.Count; i++)
                    {
                        FDSelect[i] = reader.ReadByte();
                    }
                }
                else if (format == 3)
                {
                    var ranges = reader.ReadUInt16();
                    var first = reader.ReadUInt16();
                    for (var range = 0; range < ranges; range++)
                    {
                        var value = reader.ReadByte();
                        var next = reader.ReadUInt16();
                        for (ushort i = first; i < next; i++)
                        {
                            FDSelect[i] = value;
                        }
                        first = next;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (CharStrings.Count != FDSelect.Count)
                {
                    throw new NotSupportedException("The FDSelect in the CFF data must be the same as the count field in CharStrings INDEX.");
                }
            }
        }

        /// <summary>Gets a major table version.</summary>
        public byte TableVersionNumberMajor { get; }

        /// <summary>Gets a minor table version.</summary>
        public byte TableVersionNumberMinor { get; }

        /// <summary>Gets a header size.</summary>
        public byte HeaderSize  { get; }

        /// <summary>Gets an absolute offset size.</summary>
        public byte Offset { get; }

        /// <summary>Gets the Name INDEX.</summary>
        public IndexDataOfName NameIndex { get; }

        /// <summary>Gets the Top DICT INDEX.</summary>
        public IndexDataOfTopDictionary TopDictionaryIndex { get; }

        /// <summary>Gets the String INDEX.</summary>
        public IndexDataOfString StringIndex { get; }

        /// <summary>Gets the Global Subrs INDEX.</summary>
        public IndexDataOfSubroutines GlobalSubroutines { get; }

        /// <summary>Gets the CharStrings INDEX.</summary>
        public IndexDataOfCharStrings CharStrings { get; }

        /// <summary>Gets the Font DICT INDEX.</summary>
        public IndexDataOfFontDictionary FontDictionaryIndex { get; }

        /// <summary>Gets the Top DICT Data.</summary>
        public DictionaryData TopDictionary { get; }

        /// <summary>Gets the Private DICT Data of Top DICT.</summary>
        public DictionaryData TopPrivateDictionary { get; }

        /// <summary>Gets the Local Subrs INDEX of Top DICT.</summary>
        public IndexDataOfSubroutines TopLocalSubroutines { get; }

        /// <summary>Gets a list of the Font DICT Data.</summary>
        public List<DictionaryData> FontDictionaries { get; } = new List<DictionaryData>();

        /// <summary>Gets a list of the Private DICT Data of Font DICT.</summary>
        public List<DictionaryData> FontPrivateDictionaries { get; } = new List<DictionaryData>();

        /// <summary>Gets a list of the Local Subrs INDEX.</summary>
        public List<IndexDataOfSubroutines> LocalSubroutinesList { get; } = new List<IndexDataOfSubroutines>();

        /// <summary>Gets the Font DICT INDEX by glyph index.</summary>
        public Dictionary<ushort, int> FDSelect { get; } = new Dictionary<ushort, int>();

        /// <summary>
        /// Parse to glyph data to <see cref="CharString"/>.
        /// </summary>
        /// <param name="glyphIndex">The index of the glyph.</param>
        /// <param name="width">The advance width in 'htmx' table.</param>
        /// <returns>Returns the <see cref="CharString"/>.</returns>
        public CharString ParseCharString(ushort glyphIndex, ushort width)
        {
            IndexDataOfSubroutines subrs;
            int defaultWidthX;
            int nominalWidthX;

            if (FDSelect.Count > 0)
            {
                var fdIndex = FDSelect[glyphIndex];
                var fdDict = FontPrivateDictionaries[fdIndex];
                subrs = LocalSubroutinesList[fdIndex];
                defaultWidthX = fdDict.ContainsKey("defaultWidthX") ? (int)fdDict["defaultWidthX"] : width;
                nominalWidthX = fdDict.ContainsKey("nominalWidthX") ? (int)fdDict["nominalWidthX"] : 0;
            }
            else
            {
                subrs = TopLocalSubroutines;
                defaultWidthX = TopPrivateDictionary.ContainsKey("defaultWidthX") ? (int)TopPrivateDictionary["defaultWidthX"] : width;
                nominalWidthX = TopPrivateDictionary.ContainsKey("nominalWidthX") ? (int)TopPrivateDictionary["nominalWidthX"] : 0;
            }

            var charstring = new CharString(GlobalSubroutines, subrs);
            charstring.Width = defaultWidthX;
            charstring.Parse(CharStrings.Objects[glyphIndex], null);

            if (charstring.Width != defaultWidthX)
            {
                charstring.Width += nominalWidthX;
            }

            charstring.CalcMetrics();

            return charstring;
        }
    }
}
