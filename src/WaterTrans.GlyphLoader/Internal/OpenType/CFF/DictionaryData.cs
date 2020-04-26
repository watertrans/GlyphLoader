// <copyright file="DictionaryData.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Compact FontFormat Specification DICT Data.
    /// </summary>
    internal class DictionaryData : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryData"/> class.
        /// </summary>
        /// <param name="data">The DICT Data.</param>
        /// <param name="strings">The Strings.</param>
        /// <param name="isPrivate">The value of Private DICT Data.</param>
        internal DictionaryData(byte[] data, List<string> strings, bool isPrivate)
        {
            uint index = 0;
            Stack<object> operand = new Stack<object>();

            while (index < data.Length)
            {
                uint advance = 1;
                if (data[index] >= 32 && data[index] <= 246)
                {
                    operand.Push(data[index] - 139);
                    advance = 1;
                }
                else if (data[index] >= 247 && data[index] <= 250)
                {
                    operand.Push(((data[index] - 247) * 256) + data[index + 1] + 108);
                    advance = 2;
                }
                else if (data[index] >= 251 && data[index] <= 254)
                {
                    operand.Push((-(data[index] - 251) * 256) - data[index + 1] - 108);
                    advance = 2;
                }
                else if (data[index] == 28)
                {
                    operand.Push(data[index + 1] << 8 | data[index + 2]);
                    advance = 3;
                }
                else if (data[index] == 29)
                {
                    operand.Push(data[index + 1] << 24 | data[index + 2] << 16 | data[index + 3] << 8 | data[index + 4]);
                    advance = 5;
                }
                else if (data[index] == 30)
                {
                    string nibble = string.Empty;
                    while ((data[index + advance] & 0x0f) != 0x0f)
                    {
                        int[] x = { data[index + advance] >> 4, data[index + advance] & 0x0f };
                        for (int i = 0; i < 2; i++)
                        {
                            nibble += ParseNibble(x[i]);
                        }
                        advance++;
                    }
                    nibble += ParseNibble(data[index + advance] >> 4);
                    operand.Push(float.Parse(nibble));
                    advance++;
                }
                else if (!isPrivate && data[index] >= 0 && data[index] <= 21)
                {
                    switch (data[index])
                    {
                        case 0x00:
                            this["version"] = strings[(int)operand.Pop()];
                            break;
                        case 0x01:
                            this["Notice"] = strings[(int)operand.Pop()];
                            break;
                        case 0x02:
                            this["FullName"] = strings[(int)operand.Pop()];
                            break;
                        case 0x03:
                            this["FamilyName"] = strings[(int)operand.Pop()];
                            break;
                        case 0x04:
                            this["Weight"] = strings[(int)operand.Pop()];
                            break;
                        case 0x05:
                            int fontBBox4 = (int)operand.Pop();
                            int fontBBox3 = (int)operand.Pop();
                            int fontBBox2 = (int)operand.Pop();
                            int fontBBox1 = (int)operand.Pop();
                            this["FontBBox"] = new int[] { fontBBox1, fontBBox2, fontBBox3, fontBBox4 };
                            break;
                        case 0x0d:
                            this["UniqueID"] = operand.Pop();
                            break;
                        case 0x0e:
                            this["XUID"] = operand.Reverse().ToArray();
                            operand.Clear();
                            break;
                        case 0x0f:
                            this["charset"] = (int)operand.Pop();
                            break;
                        case 0x10:
                            this["Encoding"] = (int)operand.Pop();
                            break;
                        case 0x11:
                            this["CharStrings"] = (int)operand.Pop();
                            break;
                        case 0x12:
                            int privateOffset = (int)operand.Pop();
                            int privateSize = (int)operand.Pop();
                            this["Private"] = new int[] { privateSize, privateOffset };
                            break;
                        case 0x0c:
                            switch (data[index + 1])
                            {
                                case 0x00:
                                    this["Copyright"] = strings[(int)operand.Pop()];
                                    break;
                                case 0x01:
                                    this["isFixedPitch"] = operand.Pop();
                                    break;
                                case 0x02:
                                    this["ItalicAngle"] = operand.Pop();
                                    break;
                                case 0x03:
                                    this["UnderlinePosition"] = operand.Pop();
                                    break;
                                case 0x04:
                                    this["UnderlineThickness"] = operand.Pop();
                                    break;
                                case 0x05:
                                    this["PaintType"] = operand.Pop();
                                    break;
                                case 0x06:
                                    this["CharstringType"] = (int)operand.Pop();
                                    break;
                                case 0x07:
                                    this["FontMatrix"] = operand.Reverse().ToArray();
                                    operand.Clear();
                                    break;
                                case 0x08:
                                    this["StrokeWidth"] = operand.Pop();
                                    break;
                                case 0x14:
                                    this["SyntheticBase"] = operand.Pop();
                                    break;
                                case 0x15:
                                    this["PostScript"] = strings[(int)operand.Pop()];
                                    break;
                                case 0x16:
                                    this["BaseFontName"] = strings[(int)operand.Pop()];
                                    break;
                                case 0x17:
                                    this["BaseFontBlend"] = operand.Reverse().ToArray();
                                    operand.Clear();
                                    break;
                                case 0x1e:
                                    int ros3 = (int)operand.Pop();
                                    int ros2 = (int)operand.Pop();
                                    int ros1 = (int)operand.Pop();
                                    this["ROS"] = strings[ros1] + " " + strings[ros2] + " " + ros3;
                                    break;
                                case 0x1f:
                                    this["CIDFontVersion"] = operand.Pop();
                                    break;
                                case 0x20:
                                    this["CIDFontRevision"] = operand.Pop();
                                    break;
                                case 0x21:
                                    this["CIDFontType"] = operand.Pop();
                                    break;
                                case 0x22:
                                    this["CIDCount"] = operand.Pop();
                                    break;
                                case 0x23:
                                    this["UIDBase"] = operand.Pop();
                                    break;
                                case 0x24:
                                    this["FDArray"] = operand.Pop();
                                    break;
                                case 0x25:
                                    this["FDSelect"] = operand.Pop();
                                    break;
                                case 0x26:
                                    this["FontName"] = strings[(int)operand.Pop()];
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("Invalid op:" + data[index + 1].ToString("x2") + " at pos " + index);
                            }
                            advance = 2;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Invalid op:" + data[index].ToString("x2") + " at pos " + index);
                    }
                }
                else if (isPrivate && data[index] >= 0 && data[index] <= 21)
                {
                    switch (data[index])
                    {
                        case 0x06:
                            this["BlueValues"] = operand.Reverse().ToArray();
                            operand.Clear();
                            break;
                        case 0x07:
                            this["OtherBlues"] = operand.Reverse().ToArray();
                            operand.Clear();
                            break;
                        case 0x08:
                            this["FamilyBlues"] = operand.Reverse().ToArray();
                            operand.Clear();
                            break;
                        case 0x09:
                            this["FamilyOtherBlues"] = operand.Reverse().ToArray();
                            operand.Clear();
                            break;
                        case 0x0a:
                            this["StdHW"] = operand.Pop();
                            break;
                        case 0x0b:
                            this["StdVW"] = operand.Pop();
                            break;
                        case 0x13:
                            this["Subrs"] = (int)operand.Pop();
                            break;
                        case 0x14:
                            this["defaultWidthX"] = operand.Pop();
                            break;
                        case 0x15:
                            this["nominalWidthX"] = operand.Pop();
                            break;
                        case 0x0c:
                            switch (data[index + 1])
                            {
                                case 0x09:
                                    this["BlueScale"] = operand.Pop();
                                    break;
                                case 0x0a:
                                    this["BlueShift"] = operand.Pop();
                                    break;
                                case 0x0b:
                                    this["BlueFuzz"] = operand.Pop();
                                    break;
                                case 0x0c:
                                    this["StemSnapH"] = operand.Reverse().ToArray();
                                    operand.Clear();
                                    break;
                                case 0x0d:
                                    this["StemSnapV"] = operand.Reverse().ToArray();
                                    operand.Clear();
                                    break;
                                case 0x0e:
                                    this["ForceBold"] = operand.Pop();
                                    break;
                                case 0x11:
                                    this["LanguageGroup"] = operand.Pop();
                                    break;
                                case 0x12:
                                    this["ExpansionFactor"] = operand.Pop();
                                    break;
                                case 0x13:
                                    this["initialRandomSeed"] = operand.Pop();
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("Invalid op:" + data[index + 1].ToString("x2") + " at pos " + index);
                            }
                            advance = 2;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Invalid op:" + data[index].ToString("x2") + " at pos " + index);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Reserved byte encountered:" + data[index].ToString("x2") + " at pos " + index);
                }
                index += advance;
            }
            if (index != data.Length)
            {
                throw new ArgumentOutOfRangeException("Cursor not equal to data length:" + index + "!=" + data.Length);
            }

            if (operand.Count != 0)
            {
                throw new ArgumentOutOfRangeException("Operand stack not empty:" + operand.Count);
            }
        }

        private string ParseNibble(int value)
        {
            string result = string.Empty;
            switch (value)
            {
                case 0x0a:
                    result += ".";
                    break;
                case 0x0b:
                    result += "E";
                    break;
                case 0x0c:
                    result += "E-";
                    break;
                case 0x0d:
                    throw new ArgumentOutOfRangeException(nameof(value));
                case 0x0e:
                    result += "-";
                    break;
                case 0x0f:
                    break;
                default:
                    result += value.ToString("d1");
                    break;
            }
            return result;
        }
    }
}
