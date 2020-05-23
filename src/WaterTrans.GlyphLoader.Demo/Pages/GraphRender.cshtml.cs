using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WaterTrans.GlyphLoader.TestFonts;

namespace WaterTrans.GlyphLoader.Demo.Pages
{
    public class GraphRenderModel : PageModel
    {
        private readonly ILogger<GraphRenderModel> _logger;
        private readonly ITypefaces _typefaces;

        public GraphRenderModel(ILogger<GraphRenderModel> logger, ITypefaces typefaces)
        {
            _logger = logger;
            _typefaces = typefaces;
        }

        public string Graph { get; set; }

        public void OnGet(string font, ushort index = 0)
        {
            if (!_typefaces.ContainsKey(font))
            {
                font = "Roboto-Regular.ttf";
            }

            if (index < 0 || _typefaces[font].GlyphCount <= index)
            {
                index = 0;
            }

            var tf = _typefaces[font];
            var emsize = 200;
            var renderer = new GraphRenderer();
            var typefaceGeometry = tf.GetGlyphOutline(index, emsize);
            Graph = renderer.CreateGraph(
                typefaceGeometry,
                tf.Baseline,
                tf.AdvanceWidths[index],
                tf.AdvanceHeights[index],
                tf.LeftSideBearings[index],
                tf.RightSideBearings[index],
                tf.TopSideBearings[index],
                tf.BottomSideBearings[index],
                emsize);
        }
    }
}