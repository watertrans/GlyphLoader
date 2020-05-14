// <copyright file="CharString.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Internal.OpenType.CFF
{
    /// <summary>
    /// The Type 2 Charstring Format.
    /// </summary>
    internal class CharString
    {
        private readonly IndexDataOfSubroutines _globalSubroutines;
        private readonly int _globalSubroutinesBias;
        private readonly IndexDataOfSubroutines _localSubroutines;
        private readonly int _localSubroutinesBias;
        private int _stemCount;
        private bool _haveWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharString"/> class.
        /// </summary>
        /// <param name="globalSubroutines">The Global Subrs INDEX.</param>
        /// <param name="localSubroutines">The Local Subrs INDEX.</param>
        internal CharString(IndexDataOfSubroutines globalSubroutines, IndexDataOfSubroutines localSubroutines)
        {
            _globalSubroutines = globalSubroutines;
            _localSubroutines = localSubroutines;
            _globalSubroutinesBias = CalcSubroutineBias(globalSubroutines.Count);
            _localSubroutinesBias = localSubroutines != null ? CalcSubroutineBias(localSubroutines.Count) : 0;
        }

        /// <summary>The list of expression.</summary>
        public List<CharStringExpression> Expressions { get; } = new List<CharStringExpression>();

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>Gets a minimum x for coordinate data.</summary>
        public short XMin { get; private set; } = short.MaxValue;

        /// <summary>Gets a minimum y for coordinate data.</summary>
        public short YMin { get; private set; } = short.MaxValue;

        /// <summary>Gets a maximum x for coordinate data.</summary>
        public short XMax { get; private set; } = short.MinValue;

        /// <summary>Gets a maximum y for coordinate data.</summary>
        public short YMax { get; private set; } = short.MinValue;

        /// <summary>
        /// Parse Charstring.
        /// </summary>
        /// <param name="data">The byte array of The Type 2 Charstring.</param>
        /// <param name="operand">The stack of operand.</param>
        public void Parse(byte[] data, Stack<int> operand)
        {
            int index = 0;

            if (operand == null)
            {
                _stemCount = 0;
                _haveWidth = false;
                Expressions.Clear();
                operand = new Stack<int>();
            }

            while (index < data.Length)
            {
                int advance = 1;
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
                else if (data[index] == 255)
                {
                    operand.Push(data[index + 1] << 24 | data[index + 2] << 16 | data[index + 3] << 8 | data[index + 4]);
                    advance = 5;
                }
                else if (data[index] == 28)
                {
                    operand.Push((data[index + 1] << 24 | data[index + 2] << 16) >> 16);
                    advance = 3;
                }
                else if (data[index] == 1 || data[index] == 3 || data[index] == 18 || data[index] == 23)
                {
                    var stem = operand.Reverse().ToList();
                    if (operand.Count % 2 != 0)
                    {
                        Width = (int)stem[0];
                        stem.RemoveAt(0);
                        _haveWidth = true;
                    }
                    Expressions.Add(new CharStringExpression(data[index], stem.ToArray()));
                    operand.Clear();
                    _stemCount += stem.Count / 2;
                }
                else if (data[index] == 10)
                {
                    if (operand.Count > 0)
                    {
                        var subrs = operand.Pop();
                        Parse(_localSubroutines.Objects[subrs + _localSubroutinesBias], operand);
                    }
                }
                else if (data[index] == 11)
                {
                    return;
                }
                else if (data[index] == 12)
                {
                    Expressions.Add(new CharStringExpression(data[index + 1] + 0x0c00, operand.Reverse().ToArray()));
                    operand.Clear();
                    advance = 2;
                }
                else if (data[index] == 19 || data[index] == 20)
                {
                    _stemCount += operand.Count / 2;
                    Expressions.Add(new CharStringExpression(data[index], operand.Reverse().ToArray()));
                    operand.Clear();
                    advance += (int)Math.Ceiling((double)_stemCount / 8);
                }
                else if (data[index] == 29)
                {
                    if (operand.Count > 0)
                    {
                        var subrs = operand.Pop();
                        Parse(_globalSubroutines.Objects[subrs + _globalSubroutinesBias], operand);
                    }
                }
                else if (data[index] == 4 || data[index] == 22)
                {
                    if (!_haveWidth && operand.Count > 1)
                    {
                        var removeWidth = operand.Reverse().ToList();
                        Width = (int)removeWidth[0];
                        removeWidth.RemoveAt(0);
                        _haveWidth = true;

                        Expressions.Add(new CharStringExpression(data[index], removeWidth.ToArray()));
                        operand.Clear();
                    }
                    else
                    {
                        Expressions.Add(new CharStringExpression(data[index], operand.Reverse().ToArray()));
                        operand.Clear();
                    }
                }
                else if (data[index] == 14)
                {
                    if (!_haveWidth && operand.Count > 0)
                    {
                        var removeWidth = operand.Reverse().ToList();
                        Width = (int)removeWidth[0];
                        removeWidth.RemoveAt(0);
                        _haveWidth = true;

                        Expressions.Add(new CharStringExpression(data[index], removeWidth.ToArray()));
                        operand.Clear();
                    }
                    else
                    {
                        Expressions.Add(new CharStringExpression(data[index], operand.Reverse().ToArray()));
                        operand.Clear();
                    }
                }
                else if (data[index] == 21)
                {
                    if (!_haveWidth && operand.Count > 2)
                    {
                        var removeWidth = operand.Reverse().ToList();
                        Width = (int)removeWidth[0];
                        removeWidth.RemoveAt(0);
                        _haveWidth = true;

                        Expressions.Add(new CharStringExpression(data[index], removeWidth.ToArray()));
                        operand.Clear();
                    }
                    else
                    {
                        Expressions.Add(new CharStringExpression(data[index], operand.Reverse().ToArray()));
                        operand.Clear();
                    }
                }
                else if (data[index] >= 0 && data[index] <= 31)
                {
                    Expressions.Add(new CharStringExpression(data[index], operand.Reverse().ToArray()));
                    operand.Clear();
                }
                index += advance;
            }
        }

        /// <summary>
        /// Calculate glyph metrics.
        /// </summary>
        public void CalcMetrics()
        {
            int x = 0;
            int y = 0;

            foreach (var expression in Expressions)
            {
                if (expression.OpCode == 0x0004) // vmoveto
                {
                    y += expression.Operands[0];
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0005) // rlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        x += expression.Operands[i];
                        y += expression.Operands[i + 1];
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x0006) // hlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        x += expression.Operands[i];
                        SetXY(x, y);

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        y += expression.Operands[i + 1];
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x0007) // vlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        y += expression.Operands[i];
                        SetXY(x, y);

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        x += expression.Operands[i + 1];
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x0008) // rrcurveto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 6)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y + expression.Operands[i + 1];
                        int c2x = c1x + expression.Operands[i + 2];
                        int c2y = c1y + expression.Operands[i + 3];
                        x = c2x + expression.Operands[i + 4];
                        y = c2y + expression.Operands[i + 5];

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x0015) // rmoveto
                {
                    x += expression.Operands[0];
                    y += expression.Operands[1];
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0016) // hmoveto
                {
                    x += expression.Operands[0];
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0018) // rcurveline
                {
                    for (int i = 0; i < expression.Operands.Length - 2; i += 6)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y + expression.Operands[i + 1];
                        int c2x = c1x + expression.Operands[i + 2];
                        int c2y = c1y + expression.Operands[i + 3];
                        x = c2x + expression.Operands[i + 4];
                        y = c2y + expression.Operands[i + 5];

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);
                    }

                    x += expression.Operands[expression.Operands.Length - 2];
                    y += expression.Operands[expression.Operands.Length - 1];
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0019) // rlinecurve
                {
                    for (int i = 0; i < expression.Operands.Length - 6; i += 2)
                    {
                        x += expression.Operands[i];
                        y += expression.Operands[i + 1];
                        SetXY(x, y);
                    }

                    int c1x = x + expression.Operands[expression.Operands.Length - 6];
                    int c1y = y + expression.Operands[expression.Operands.Length - 5];
                    int c2x = c1x + expression.Operands[expression.Operands.Length - 4];
                    int c2y = c1y + expression.Operands[expression.Operands.Length - 3];
                    x = c2x + expression.Operands[expression.Operands.Length - 2];
                    y = c2y + expression.Operands[expression.Operands.Length - 1];

                    SetXY(c1x, c1y);
                    SetXY(c2x, c2y);
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x001a) // vvcurveto
                {
                    int start = 0;
                    if ((expression.Operands.Length % 2) != 0)
                    {
                        x += expression.Operands[0];
                        start = 1;
                    }

                    for (int i = start; i < expression.Operands.Length; i += 4)
                    {
                        int c1x = x;
                        int c1y = y + expression.Operands[i];
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x;
                        y = c2y + expression.Operands[i + 3];

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x001b) // hhcurveto
                {
                    int start = 0;
                    if ((expression.Operands.Length % 2) != 0)
                    {
                        y += expression.Operands[0];
                        start = 1;
                    }

                    for (int i = start; i < expression.Operands.Length; i += 4)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y;
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y;

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);
                    }
                }
                else if (expression.OpCode == 0x001e) // vhcurveto
                {
                    int i = 0;
                    while (i < expression.Operands.Length)
                    {
                        int c1x = x;
                        int c1y = y + expression.Operands[i];
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        c1x = x + expression.Operands[i];
                        c1y = y;
                        c2x = c1x + expression.Operands[i + 1];
                        c2y = c1y + expression.Operands[i + 2];
                        x = c2x + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);
                        y = c2y + expression.Operands[i + 3];

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }
                    }
                }
                else if (expression.OpCode == 0x001f) // hvcurveto
                {
                    int i = 0;
                    while (i < expression.Operands.Length)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y;
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);
                        y = c2y + expression.Operands[i + 3];

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        c1x = x;
                        c1y = y + expression.Operands[i];
                        c2x = c1x + expression.Operands[i + 1];
                        c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);

                        SetXY(c1x, c1y);
                        SetXY(c2x, c2y);
                        SetXY(x, y);

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }
                    }
                }
                else if (expression.OpCode == 0x0c22) // hflex
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y;
                    int c2x = c1x + expression.Operands[1];
                    int c2y = c1y + expression.Operands[2];
                    int c3x = c2x + expression.Operands[3];
                    int c3y = c2y;

                    SetXY(c1x, c1y);
                    SetXY(c2x, c2y);
                    SetXY(c3x, c3y);

                    int c4x = c3x + expression.Operands[4];
                    int c4y = c2y;
                    int c5x = c4x + expression.Operands[5];
                    int c5y = y;
                    x = c5x + expression.Operands[6];

                    SetXY(c4x, c4y);
                    SetXY(c5x, c5y);
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0c23) // flex
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y + expression.Operands[5];

                    SetXY(c1x, c1y);
                    SetXY(c2x, c2y);
                    SetXY(c3x, c3y);

                    int c4x = c3x + expression.Operands[6];
                    int c4y = c3y + expression.Operands[7];
                    int c5x = c4x + expression.Operands[8];
                    int c5y = c4y + expression.Operands[9];
                    x = c5x + expression.Operands[10];
                    y = c5y + expression.Operands[11];

                    SetXY(c4x, c4y);
                    SetXY(c5x, c5y);
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0c24) // hflex1
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y;

                    SetXY(c1x, c1y);
                    SetXY(c2x, c2y);
                    SetXY(c3x, c3y);

                    int c4x = c3x + expression.Operands[5];
                    int c4y = c2y;
                    int c5x = c4x + expression.Operands[6];
                    int c5y = c4y + expression.Operands[7];
                    x = c5x + expression.Operands[8];

                    SetXY(c4x, c4y);
                    SetXY(c5x, c5y);
                    SetXY(x, y);
                }
                else if (expression.OpCode == 0x0c25) // flex1
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y + expression.Operands[5];

                    SetXY(c1x, c1y);
                    SetXY(c2x, c2y);
                    SetXY(c3x, c3y);

                    int c4x = c3x + expression.Operands[6];
                    int c4y = c3y + expression.Operands[7];
                    int c5x = c4x + expression.Operands[8];
                    int c5y = c4y + expression.Operands[9];

                    if (Math.Abs(c5x - x) > Math.Abs(c5y - y))
                    {
                        x = c5x + expression.Operands[10];
                    }
                    else
                    {
                        y = c5y + expression.Operands[10];
                    }

                    SetXY(c4x, c4y);
                    SetXY(c5x, c5y);
                    SetXY(x, y);
                }
            }

            if (XMin == short.MaxValue)
            {
                XMin = 0;
            }

            if (YMin == short.MaxValue)
            {
                YMin = 0;
            }

            if (XMax == short.MinValue)
            {
                XMax = 0;
            }

            if (YMax == short.MinValue)
            {
                YMax = 0;
            }
        }

        /// <summary>
        /// Converts to glyph data to <see cref="PathGeometry"/>.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>Returns the <see cref="PathGeometry"/>.</returns>
        public PathGeometry ConvertToPathGeometry(double scale)
        {
            int x = 0;
            int y = 0;

            var result = new PathGeometry();
            result.FillRule = FillRule.Nonzero;

            PathFigure figure = null;

            foreach (var expression in Expressions)
            {
                if (expression.OpCode == 0x0004) // vmoveto
                {
                    y += expression.Operands[0];
                    figure = new PathFigure();
                    figure.IsClosed = true;
                    figure.StartPoint = new Point(x * scale, -y * scale);
                    result.Figures.Add(figure);
                }
                else if (expression.OpCode == 0x0005) // rlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        x += expression.Operands[i];
                        y += expression.Operands[i + 1];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));
                    }
                }
                else if (expression.OpCode == 0x0006) // hlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        x += expression.Operands[i];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        y += expression.Operands[i + 1];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));
                    }
                }
                else if (expression.OpCode == 0x0007) // vlineto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 2)
                    {
                        y += expression.Operands[i];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        x += expression.Operands[i + 1];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));
                    }
                }
                else if (expression.OpCode == 0x0008) // rrcurveto
                {
                    for (int i = 0; i < expression.Operands.Length; i += 6)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y + expression.Operands[i + 1];
                        int c2x = c1x + expression.Operands[i + 2];
                        int c2y = c1y + expression.Operands[i + 3];
                        x = c2x + expression.Operands[i + 4];
                        y = c2y + expression.Operands[i + 5];

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));
                    }
                }
                else if (expression.OpCode == 0x0015) // rmoveto
                {
                    x += expression.Operands[0];
                    y += expression.Operands[1];
                    figure = new PathFigure();
                    figure.IsClosed = true;
                    figure.StartPoint = new Point(x * scale, -y * scale);
                    result.Figures.Add(figure);
                }
                else if (expression.OpCode == 0x0016) // hmoveto
                {
                    x += expression.Operands[0];
                    figure = new PathFigure();
                    figure.IsClosed = true;
                    figure.StartPoint = new Point(x * scale, -y * scale);
                    result.Figures.Add(figure);
                }
                else if (expression.OpCode == 0x0018) // rcurveline
                {
                    for (int i = 0; i < expression.Operands.Length - 2; i += 6)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y + expression.Operands[i + 1];
                        int c2x = c1x + expression.Operands[i + 2];
                        int c2y = c1y + expression.Operands[i + 3];
                        x = c2x + expression.Operands[i + 4];
                        y = c2y + expression.Operands[i + 5];

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));
                    }

                    x += expression.Operands[expression.Operands.Length - 2];
                    y += expression.Operands[expression.Operands.Length - 1];
                    figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));
                }
                else if (expression.OpCode == 0x0019) // rlinecurve
                {
                    for (int i = 0; i < expression.Operands.Length - 6; i += 2)
                    {
                        x += expression.Operands[i];
                        y += expression.Operands[i + 1];
                        figure.Segments.Add(new LineSegment(new Point(x * scale, -y * scale), true));
                    }

                    int c1x = x + expression.Operands[expression.Operands.Length - 6];
                    int c1y = y + expression.Operands[expression.Operands.Length - 5];
                    int c2x = c1x + expression.Operands[expression.Operands.Length - 4];
                    int c2y = c1y + expression.Operands[expression.Operands.Length - 3];
                    x = c2x + expression.Operands[expression.Operands.Length - 2];
                    y = c2y + expression.Operands[expression.Operands.Length - 1];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c1x * scale, -c1y * scale),
                        new Point(c2x * scale, -c2y * scale),
                        new Point(x * scale, -y * scale),
                        true));
                }
                else if (expression.OpCode == 0x001a) // vvcurveto
                {
                    int start = 0;
                    if ((expression.Operands.Length % 2) != 0)
                    {
                        x += expression.Operands[0];
                        start = 1;
                    }

                    for (int i = start; i < expression.Operands.Length; i += 4)
                    {
                        int c1x = x;
                        int c1y = y + expression.Operands[i];
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x;
                        y = c2y + expression.Operands[i + 3];

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));
                    }
                }
                else if (expression.OpCode == 0x001b) // hhcurveto
                {
                    int start = 0;
                    if ((expression.Operands.Length % 2) != 0)
                    {
                        y += expression.Operands[0];
                        start = 1;
                    }

                    for (int i = start; i < expression.Operands.Length; i += 4)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y;
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y;

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));
                    }
                }
                else if (expression.OpCode == 0x001e) // vhcurveto
                {
                    int i = 0;
                    while (i < expression.Operands.Length)
                    {
                        int c1x = x;
                        int c1y = y + expression.Operands[i];
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        c1x = x + expression.Operands[i];
                        c1y = y;
                        c2x = c1x + expression.Operands[i + 1];
                        c2y = c1y + expression.Operands[i + 2];
                        x = c2x + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);
                        y = c2y + expression.Operands[i + 3];

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }
                    }
                }
                else if (expression.OpCode == 0x001f) // hvcurveto
                {
                    int i = 0;
                    while (i < expression.Operands.Length)
                    {
                        int c1x = x + expression.Operands[i];
                        int c1y = y;
                        int c2x = c1x + expression.Operands[i + 1];
                        int c2y = c1y + expression.Operands[i + 2];
                        x = c2x + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);
                        y = c2y + expression.Operands[i + 3];

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }

                        c1x = x;
                        c1y = y + expression.Operands[i];
                        c2x = c1x + expression.Operands[i + 1];
                        c2y = c1y + expression.Operands[i + 2];
                        x = c2x + expression.Operands[i + 3];
                        y = c2y + (expression.Operands.Length == i + 5 ? expression.Operands[i + 4] : 0);

                        figure.Segments.Add(new BezierSegment(
                            new Point(c1x * scale, -c1y * scale),
                            new Point(c2x * scale, -c2y * scale),
                            new Point(x * scale, -y * scale),
                            true));

                        i += 4;

                        if (i + 1 >= expression.Operands.Length)
                        {
                            break;
                        }
                    }
                }
                else if (expression.OpCode == 0x0c22) // hflex
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y;
                    int c2x = c1x + expression.Operands[1];
                    int c2y = c1y + expression.Operands[2];
                    int c3x = c2x + expression.Operands[3];
                    int c3y = c2y;

                    figure.Segments.Add(new BezierSegment(
                        new Point(c1x * scale, -c1y * scale),
                        new Point(c2x * scale, -c2y * scale),
                        new Point(c3x * scale, -c3y * scale),
                        true));

                    int c4x = c3x + expression.Operands[4];
                    int c4y = c2y;
                    int c5x = c4x + expression.Operands[5];
                    int c5y = y;
                    x = c5x + expression.Operands[6];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c4x * scale, -c4y * scale),
                        new Point(c5x * scale, -c5y * scale),
                        new Point(x * scale, -y * scale),
                        true));
                }
                else if (expression.OpCode == 0x0c23) // flex
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y + expression.Operands[5];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c1x * scale, -c1y * scale),
                        new Point(c2x * scale, -c2y * scale),
                        new Point(c3x * scale, -c3y * scale),
                        true));

                    int c4x = c3x + expression.Operands[6];
                    int c4y = c3y + expression.Operands[7];
                    int c5x = c4x + expression.Operands[8];
                    int c5y = c4y + expression.Operands[9];
                    x = c5x + expression.Operands[10];
                    y = c5y + expression.Operands[11];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c4x * scale, -c4y * scale),
                        new Point(c5x * scale, -c5y * scale),
                        new Point(x * scale, -y * scale),
                        true));
                }
                else if (expression.OpCode == 0x0c24) // hflex1
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y;

                    figure.Segments.Add(new BezierSegment(
                        new Point(c1x * scale, -c1y * scale),
                        new Point(c2x * scale, -c2y * scale),
                        new Point(c3x * scale, -c3y * scale),
                        true));

                    int c4x = c3x + expression.Operands[5];
                    int c4y = c2y;
                    int c5x = c4x + expression.Operands[6];
                    int c5y = c4y + expression.Operands[7];
                    x = c5x + expression.Operands[8];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c4x * scale, -c4y * scale),
                        new Point(c5x * scale, -c5y * scale),
                        new Point(x * scale, -y * scale),
                        true));
                }
                else if (expression.OpCode == 0x0c25) // flex1
                {
                    int c1x = x + expression.Operands[0];
                    int c1y = y + expression.Operands[1];
                    int c2x = c1x + expression.Operands[2];
                    int c2y = c1y + expression.Operands[3];
                    int c3x = c2x + expression.Operands[4];
                    int c3y = c2y + expression.Operands[5];

                    figure.Segments.Add(new BezierSegment(
                        new Point(c1x * scale, -c1y * scale),
                        new Point(c2x * scale, -c2y * scale),
                        new Point(c3x * scale, -c3y * scale),
                        true));

                    int c4x = c3x + expression.Operands[6];
                    int c4y = c3y + expression.Operands[7];
                    int c5x = c4x + expression.Operands[8];
                    int c5y = c4y + expression.Operands[9];

                    if (Math.Abs(c5x - x) > Math.Abs(c5y - y))
                    {
                        x = c5x + expression.Operands[10];
                    }
                    else
                    {
                        y = c5y + expression.Operands[10];
                    }

                    figure.Segments.Add(new BezierSegment(
                        new Point(c4x * scale, -c4y * scale),
                        new Point(c5x * scale, -c5y * scale),
                        new Point(x * scale, -y * scale),
                        true));
                }
            }

            return result;
        }

        private void SetXY(int x, int y)
        {
            if (x < XMin)
            {
                XMin = (short)x;
            }

            if (x > XMax)
            {
                XMax = (short)x;
            }

            if (y < YMin)
            {
                YMin = (short)y;
            }

            if (y > YMax)
            {
                YMax = (short)y;
            }
        }

        private int CalcSubroutineBias(int count)
        {
            int bias;
            if (count < 1240)
            {
                bias = 107;
            }
            else if (count < 33900)
            {
                bias = 1131;
            }
            else
            {
                bias = 32768;
            }

            return bias;
        }
    }
}
