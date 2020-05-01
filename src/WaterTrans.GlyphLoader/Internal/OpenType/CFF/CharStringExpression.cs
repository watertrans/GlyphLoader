// <copyright file="CharStringExpression.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System.Collections.Generic;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Type 2 Charstring Format expression.
    /// </summary>
    internal class CharStringExpression
    {
        /// <summary>
        /// The One-byte or Two-byte Type 2 Operators.
        /// </summary>
        public static readonly Dictionary<int, string> Operators =
            new Dictionary<int, string>
            {
                { 0x0001, "hstem" },
                { 0x0003, "vstem" },
                { 0x0004, "vmoveto" },
                { 0x0005, "rlineto" },
                { 0x0006, "hlineto" },
                { 0x0007, "vlineto" },
                { 0x0008, "rrcurveto" },
                { 0x000a, "callsubr" },
                { 0x000b, "return" },
                { 0x000e, "endchar" },
                { 0x0012, "hstemhm" },
                { 0x0013, "hintmask" },
                { 0x0014, "cntrmask" },
                { 0x0015, "rmoveto" },
                { 0x0016, "hmoveto" },
                { 0x0017, "vstemhm" },
                { 0x0018, "rcurveline" },
                { 0x0019, "rlinecurve" },
                { 0x001a, "vvcurveto" },
                { 0x001b, "hhcurveto" },
                { 0x001d, "callgsubr" },
                { 0x001e, "vhcurveto" },
                { 0x001f, "hvcurveto" },
                { 0x0c22, "hflex" },
                { 0x0c23, "flex" },
                { 0x0c24, "hflex1" },
                { 0x0c25, "flex1" },
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="CharStringExpression"/> class.
        /// </summary>
        /// <param name="opCode">The One-byte or Two-byte Type 2 operator code.</param>
        /// <param name="operands">The array of operand.</param>
        internal CharStringExpression(int opCode, int[] operands)
        {
            OpCode = opCode;
            Operator = Operators[opCode];
            Operands = operands;
        }

        /// <summary>The One-byte or Two-byte Type 2 operator code.</summary>
        public int OpCode { get; }

        /// <summary>The One-byte or Two-byte Type 2 operator.</summary>
        public string Operator { get; }

        /// <summary>The array of operand.</summary>
        public int[] Operands { get; }
    }
}
