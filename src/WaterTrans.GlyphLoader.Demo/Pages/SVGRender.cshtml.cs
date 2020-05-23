using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.Demo.Pages
{
    public class Glyph
    {
        public Glyph(double x, double y, PathGeometry glyphOutline)
        {
            X = x;
            Y = y;
            GlyphOutline = glyphOutline;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public PathGeometry GlyphOutline { get; set; }
    }

    public class SVGRenderModel : PageModel
    {
        private readonly ILogger<SVGRenderModel> _logger;
        private readonly ITypefaces _typefaces;

        public SVGRenderModel(ILogger<SVGRenderModel> logger, ITypefaces typefaces)
        {
            _logger = logger;
            _typefaces = typefaces;
        }

        public double Width { get; set; }
        public double Height { get; set; }
        public List<Glyph> Glyphs { get; set; } = new List<Glyph>();

        public void OnGet(string font, string text, double emsize = 50)
        {
            if (!_typefaces.ContainsKey(font))
            {
                font = "Roboto-Regular.ttf";
            }

            if (string.IsNullOrEmpty(text))
            {
                text = "The quick brown fox jumps over the lazy dog";
            }

            var tf = _typefaces[font];

            double x = emsize * 0.2;
            double y = 0;
            foreach (char c in text)
            {
                ushort glyphIndex = 0;
                if (tf.CharacterToGlyphMap.ContainsKey((int)c))
                {
                    glyphIndex = tf.CharacterToGlyphMap[(int)c];
                }
                double advanceWidth = tf.AdvanceWidths[glyphIndex] * emsize;
                double baseline = tf.Baseline * emsize;
                Glyphs.Add(new Glyph(x, y + baseline, tf.GetGlyphOutline(glyphIndex, emsize)));
                x += advanceWidth;
            }

            Width = x + (emsize * 0.2);
            Height = emsize + (emsize * 0.3);
        }
    }
}