// <copyright file="NameEncoding.cs" company="WaterTrans">
// © 2020 WaterTrans and Contributors
// </copyright>

using System;
using System.Text;

namespace WaterTrans.GlyphLoader.Internal.NAME
{
    /// <summary>
    /// NAME NameRecord name string encoding.
    /// </summary>
    internal class NameEncoding
    {
        /// <summary>
        /// Returns an encoding for the specified NameRecord.
        /// </summary>
        /// <param name="record">The <see cref="NameRecord"/>.</param>
        /// <returns>The encoding that is associated with the specified code page.</returns>
        internal static Encoding GetEncoding(NameRecord record)
        {
            try
            {
                switch (record.PlatformID)
                {
                    case (ushort)PlatformID.Unicode:
                        return Encoding.BigEndianUnicode;
                    case (ushort)PlatformID.Macintosh:
                        switch (record.EncodingID)
                        {
                            case 0: return Encoding.ASCII;
                            case 1: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-japanese"); // return Encoding.GetEncoding("x-mac-japanese");
                            case 2: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-chinesetrad"); // return Encoding.GetEncoding("x-mac-chinesetrad");
                            case 3: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-korean"); // return Encoding.GetEncoding("x-mac-korean");
                            case 4: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-arabic"); // return Encoding.GetEncoding("x-mac-arabic");
                            case 5: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-hebrew"); // return Encoding.GetEncoding("x-mac-hebrew");
                            case 6: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-greek"); // return Encoding.GetEncoding("x-mac-greek");
                            case 7: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-cyrillic"); // return Encoding.GetEncoding("x-mac-cyrillic");
                            // case 8: return Encoding.GetEncoding("");
                            case 9: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-de"); // return Encoding.GetEncoding("x-iscii-de");
                            // case 10: return Encoding.GetEncoding("");
                            case 11: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-gu"); // return Encoding.GetEncoding("x-iscii-gu");
                            case 12: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-or"); // return Encoding.GetEncoding("x-iscii-or");
                            case 13: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-be"); // return Encoding.GetEncoding("x-iscii-be");
                            case 14: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-ta"); // return Encoding.GetEncoding("x-iscii-ta");
                            case 15: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-te"); // return Encoding.GetEncoding("x-iscii-te");
                            case 16: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-ka"); // return Encoding.GetEncoding("x-iscii-ka");
                            case 17: return CodePagesEncodingProvider.Instance.GetEncoding("x-iscii-ma"); // return Encoding.GetEncoding("x-iscii-ma");
                            // case 18: return Encoding.GetEncoding("");
                            case 19: return CodePagesEncodingProvider.Instance.GetEncoding("Windows-1252"); // return Encoding.GetEncoding("Windows-1252");
                            // case 20: return Encoding.GetEncoding("");
                            case 21: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-thai"); // return Encoding.GetEncoding("x-mac-thai");
                            // case 22: return Encoding.GetEncoding("");
                            // case 23: return Encoding.GetEncoding("");
                            // case 24: return Encoding.GetEncoding("");
                            case 25: return CodePagesEncodingProvider.Instance.GetEncoding("x-mac-chinesesimp"); // return Encoding.GetEncoding("x-mac-chinesesimp");
                            // case 26: return Encoding.GetEncoding("");
                            // case 27: return Encoding.GetEncoding("");
                            // case 28: return Encoding.GetEncoding("");
                            // case 29: return Encoding.GetEncoding("");
                            case 30: return CodePagesEncodingProvider.Instance.GetEncoding("windows-1258"); // return Encoding.GetEncoding("windows-1258");
                            // case 31: return Encoding.GetEncoding("");
                            // case 32: return Encoding.GetEncoding("");
                        }

                        break;
                    case (ushort)PlatformID.ISO:
                        switch (record.EncodingID)
                        {
                            case 0: return Encoding.ASCII;
                            case 1: return Encoding.BigEndianUnicode;
                            case 2: return Encoding.GetEncoding(1252);
                        }

                        break;
                    case (ushort)PlatformID.Microsoft:
                        switch (record.EncodingID)
                        {
                            case 1: return Encoding.BigEndianUnicode;
                            case 2: return Encoding.GetEncoding("shift_jis");
                            case 3: return Encoding.GetEncoding("gb2312");
                            case 4: return Encoding.GetEncoding("big5");
                            case 5: return Encoding.GetEncoding("x-cp20949");
                            case 6: return Encoding.GetEncoding("Johab");
                            case 10: return Encoding.BigEndianUnicode;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Encoding.ASCII;
            }

            return Encoding.ASCII;
        }
    }
}
