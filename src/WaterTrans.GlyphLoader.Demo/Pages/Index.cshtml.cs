using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace WaterTrans.GlyphLoader.Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITypefaces _typefaces;

        public IndexModel(ILogger<IndexModel> logger, ITypefaces typefaces)
        {
            _logger = logger;
            _typefaces = typefaces;
        }

        public List<SelectListItem> FontFiles { get; set; }
        public string GlyphCounts { get; set; }

        public void OnGet()
        {
            FontFiles = _typefaces.FontFiles.Select(x => new SelectListItem { Value = x, Text = x }).ToList();
            var glyphCounts = new Dictionary<string, int>();
            foreach (var font in _typefaces.FontFiles)
            {
                glyphCounts[font] = _typefaces[font].GlyphCount;
            }
            GlyphCounts = JsonSerializer.Serialize(glyphCounts);
        }
    }
}
