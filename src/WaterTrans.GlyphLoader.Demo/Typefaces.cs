using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace WaterTrans.GlyphLoader.Demo
{
    public interface ITypefaces
    {
        bool ContainsKey(string key);
        Typeface this[string key] { get; }
        string[] FontFiles { get; }
    }

    public class Typefaces : ITypefaces
    {
        private Dictionary<string, Typeface>  _typefaces = new Dictionary<string, Typeface>();

        public Typefaces()
        {
            foreach (string fontFile in FontFiles)
            {
                string fontPath = Path.Combine(AppContext.BaseDirectory, fontFile);
                using (var fontStream = File.OpenRead(fontPath))
                {
                    _typefaces[fontFile] = new Typeface(fontStream);
                }
            }
        }

        public Typeface this[string key] => _typefaces[key];

        public string[] FontFiles => new string[] {
            "NotoSerifJP-Regular.otf",
            "NotoSansJP-Regular.otf",
            "Roboto-Regular.ttf",
            "RobotoMono-Regular.ttf",
            "Lora-VariableFont_wght.ttf",
            "Font Awesome 5 Free-Solid-900.otf",
            "Font Awesome 5 Free-Regular-400.otf",
            "Font Awesome 5 Brands-Regular-400.otf",
            "Roboto-Regular.woff2",
            "RobotoMono-Regular.woff2",
        };

        public bool ContainsKey(string key)
        {
            return _typefaces.ContainsKey(key);
        }
    }
}
