# GlyphLoader
GlyphLoader is a .NET Standerd library for TrueType, OpenType font.  
It is written in C#, designed to be small, efficient and portable while capable of producing high-quality glyph images.  
In WebAssembly environment, it can be used for application development using glyph outline information.  

## Features

- It provides the similar function as WPF GlyphTypeface class.
- The output glyph outline can be easily converted to SVG tags.
- It also supports vertical writing in Japanese.

## Demo

![AjaxDemo](./docs/img/demo/ajaxrendering.gif)

![GraphDemo](./docs/img/demo/glyphcontuours.gif)

## Release Notes

### 1.0-beta3

- Add support for GPOS single adjustment, pair adjustment

### 1.0-beta2

- Add support for GSUB ligature, alternate, multiple substitution

### 1.0-beta

- Add support for vertical writing in Japanese

### 1.0-alpha

- Add support for TrueType glyph outline in glyf table
- Add support for OpenType glyph outline in CFF table

## Supported Platforms
This library is compiled for .NET Standard 2.0. Supports following platforms:

- .NET Core (2.0+)
- .NET Framework (4.6.1+)
- WebAssembly (1.0+)
- Mono (5.4+)
- Xamarin.iOS (10.14+)
- Xamarin.Mac (3.8+)
- Xamarin.Android (8.0+)

## Supported Font File Format
Supports following font file format:

- TrueType(TTF) and TrueType collections(TTC)
- OpenType(OTF) and OpenType collections(OTC)
- Web Open Font Format 2(WOFF2) [comming soon]

## Limitations
GlyphLoader has the following limitations:

- CFF2 table will be supported in future version
- Variable fonts will be supported in future version

## Make sure to read
Font files are inherently dangerous. You shouldn't read font files uploaded by someone via the internet.
The GlyphLoader expects to load your own safe font file with a clear source.

## License
MIT

## Getting Started

The following is a horizontal writing sample:

```cs
string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "NotoSerifJP-Regular.otf");
Typeface tf;
using (var fontStream = System.IO.File.OpenRead(fontPath))
{
    // Initialize stream only
    tf = new Typeface(fontStream);
}

var svg = new System.Text.StringBuilder();
double unit = 100;
double x = 20;
double y = 20;
string japaneseText = "以心伝心";

svg.AppendLine("<svg width='440' height='140' viewBox='0 0 440 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

foreach (char c in japaneseText)
{
    // Get glyph index
    ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

    // Get glyph outline
    var geometry = tf.GetGlyphOutline(glyphIndex, unit);

    // Get advanced width
    double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

    // Get advanced height
    double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

    // Get baseline
    double baseline = tf.Baseline * unit;

    // Convert to path mini-language
    string miniLanguage = geometry.Figures.ToString(x, y + baseline);

    svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
    x += advanceWidth;
}

svg.AppendLine("</svg>");
System.Diagnostics.Trace.WriteLine(svg.ToString());
/* Result
<svg width='440' height='140' viewBox='0 0 440 140' xmlns='http://www.w3.org/2000/svg' version='1.1'>
<path d='M57.1,41.2C63.2,46.9 71.4,56.2 73.9,63.3C81.8,68.4 86.2,52 58.1,40.4z M71.1,82.5C61.8,85.9 52.5,89.3 44.5,92.1L43.5,32.9C46.1,32.5 46.9,31.4 47,30L37,29.2L38,94.3C31.8,96.4 26.6,98.1 23.3,98.9L27.6,107.4C28.6,107 29.5,105.9 29.7,104.7C48,96.4 61.6,89.2 71.6,84z M98.5,29.4C97.6,67 93.2,93.3 46.9,113.8L48,115.7C68.8,108.2 81.9,99.3 90.3,88.9C97.8,96.3 106.5,106.4 109.6,114.2C117.9,119.5 121.7,102.5 91.9,86.8C102.7,72 104.6,54.3 105.6,33.6C107.8,33.3 109,32.2 109.3,30.7z ' fill='black' stroke='black' stroke-width='0' />
<path d='M136.5,57.2C136.7,70.6 131.1,82.5 125.2,87.1C123.4,89 122.5,91.5 123.9,93.3C125.6,95.3 129.6,94 132.5,90.9C136.9,86.2 142.4,74.8 138.3,57.2z M152.8,26.5L152.2,28.1C165.1,33.7 174,41.9 177.3,47.2C185.2,51.4 189.5,33.5 152.8,26.5z M195.6,56L194.6,57.1C205.5,66.4 209.1,81.1 209.4,90.2C216.7,98.7 225.3,72.3 195.6,56z M151,46.9L151,104.4C151,110.8 153.6,112.5 162.9,112.5L176.7,112.5C196.2,112.5 200,111.4 200,107.9C200,106.5 199.3,105.7 196.9,104.9L196.7,86.7L195.4,86.7C193.9,94.9 192.5,102.1 191.6,104.1C191.1,105.2 190.6,105.7 189.2,105.9C187.3,106.1 182.8,106.1 176.8,106.1L163.7,106.1C158.4,106.1 157.6,105.3 157.6,103L157.6,50.7C159.8,50.3 160.8,49.3 161,48z ' fill='black' stroke='black' stroke-width='0' />
<path d='M307.2,38.6C308.7,38.6 309.6,38.1 309.9,37.1C306.6,33.9 301.1,29.6 301.1,29.6L296.4,35.7L257.2,35.7L257.9,38.6z M246.5,24.2C241.4,43.2 232.7,62.2 224,74.3L225.4,75.3C229.8,71 234,65.8 237.9,59.9L237.9,115.7L239.1,115.7C241.5,115.7 244.3,114.1 244.4,113.5L244.4,54C246.2,53.7 247.1,53 247.4,52.1L243.4,50.6C247.1,44 250.3,36.8 253,29.3C255.2,29.4 256.4,28.6 256.9,27.5z M303.6,63.1L249,63.1L249.7,66.1L273.3,66.1C270.5,77.2 265.5,93.4 261.9,103C256.5,103.5 252,103.8 248.8,103.9L252.9,113C253.9,112.8 254.9,112.1 255.5,111C276.6,107 292,103.8 303.8,101.1C305.9,105.3 307.5,109.4 308.3,113C316.2,119.6 321.1,100.5 292,79.9L290.6,80.7C294.5,85.7 299.1,92.4 302.7,99.1C288.8,100.6 275.5,101.9 265,102.8C270.3,93.3 277.2,78 281.9,66.1L314.8,66.1C316,66.1 317,65.6 317.3,64.5C314,61.4 308.5,57.1 308.5,57.1z ' fill='black' stroke='black' stroke-width='0' />
<path d='M336.5,57.2C336.7,70.6 331.1,82.5 325.2,87.1C323.4,89 322.5,91.5 323.9,93.3C325.6,95.3 329.6,94 332.5,90.9C336.9,86.2 342.4,74.8 338.3,57.2z M352.8,26.5L352.2,28.1C365.1,33.7 374,41.9 377.3,47.2C385.2,51.4 389.5,33.5 352.8,26.5z M395.6,56L394.6,57.1C405.5,66.4 409.1,81.1 409.4,90.2C416.7,98.7 425.3,72.3 395.6,56z M351,46.9L351,104.4C351,110.8 353.6,112.5 362.9,112.5L376.7,112.5C396.2,112.5 400,111.4 400,107.9C400,106.5 399.3,105.7 396.9,104.9L396.7,86.7L395.4,86.7C393.9,94.9 392.5,102.1 391.6,104.1C391.1,105.2 390.6,105.7 389.2,105.9C387.3,106.1 382.8,106.1 376.8,106.1L363.7,106.1C358.4,106.1 357.6,105.3 357.6,103L357.6,50.7C359.8,50.3 360.8,49.3 361,48z ' fill='black' stroke='black' stroke-width='0' />
</svg>
*/
```
![Example](./docs/examples/ishindenshin.svg)

The following is a vertical writing sample:

```cs
string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "NotoSansJP-Regular.otf");
Typeface tf;
using (var fontStream = System.IO.File.OpenRead(fontPath))
{
    // Initialize stream only
    tf = new Typeface(fontStream);
}

// Get vertical glyph map
var vertMap = tf.GetSingleSubstitutionMap("DFLT", "DFLT", "vert");

var svg = new System.Text.StringBuilder();
double unit = 100;
double x = 20;
double y = 20;
string japaneseText = "【風林火山】";

svg.AppendLine("<svg width='140' height='640' viewBox='0 0 140 640' xmlns='http://www.w3.org/2000/svg' version='1.1'>");

foreach (char c in japaneseText)
{
    // Get glyph index
    ushort glyphIndex = tf.CharacterToGlyphMap[(int)c];

    // Get vertical glyph index
    glyphIndex = vertMap.ContainsKey(glyphIndex) ? vertMap[glyphIndex] : glyphIndex;

    // Get glyph outline
    var geometry = tf.GetGlyphOutline(glyphIndex, unit);

    // Get advanced width
    double advanceWidth = tf.AdvanceWidths[glyphIndex] * unit;

    // Get advanced height
    double advanceHeight = tf.AdvanceHeights[glyphIndex] * unit;

    // Get baseline
    double baseline = tf.Baseline * unit;

    // Convert to path mini-language
    string miniLanguage = geometry.Figures.ToString(x, y + baseline);

    svg.AppendLine($"<path d='{miniLanguage}' fill='black' stroke='black' stroke-width='0' />");
    y += advanceHeight;
}

svg.AppendLine("</svg>");
System.Diagnostics.Trace.WriteLine(svg.ToString());
/* Result
<svg width='140' height='640' viewBox='0 0 140 640' xmlns='http://www.w3.org/2000/svg' version='1.1'>
<path d='M116.6,86.6L23.4,86.6L23.4,116.6L23.9,116.6C33.1,105.7 49.7,96.8 70,96.8C90.3,96.8 106.9,105.7 116.1,116.6L116.6,116.6z ' fill='black' stroke='black' stroke-width='0' />
<path d='M54.5,180.3L54.5,165.1L65.8,165.1L65.8,180.3z M83.8,165.1L83.8,180.3L72.4,180.3L72.4,165.1z M79.9,190.6C81.8,193.2 83.6,196.1 85.3,199.1L72.4,199.9L72.4,186.2L90.1,186.2L90.1,159.1L72.4,159.1L72.4,149.4C79.8,148.5 86.7,147.3 92.2,145.8L87,140.4C77.5,143 60.2,145 45.7,146C46.5,147.5 47.4,150 47.7,151.5C53.5,151.3 59.7,150.8 65.8,150.2L65.8,159.1L48.5,159.1L48.5,186.2L65.8,186.2L65.8,200.3C56.8,200.8 48.7,201.3 42.5,201.6L43,208.4C54.8,207.5 71.8,206.3 88.4,205.1C89.8,207.9 90.8,210.6 91.4,212.8L97.5,210.6C95.8,204.5 90.8,195.3 85.6,188.7z M35.7,129.8L35.7,161.8C35.7,177.1 34.6,197.4 23.9,211.6C25.6,212.5 28.6,214.5 29.8,215.8C41.1,200.7 42.8,178 42.8,161.8L42.8,136.6L96.8,136.6C97.1,180.2 97,215.6 109.2,215.6C114.3,215.6 115.8,210.6 116.5,197.7C115.1,196.5 113.2,194.4 111.9,192.4C111.7,201.1 111.2,207.9 109.9,207.9C104,207.9 103.8,166.9 103.9,129.8z ' fill='black' stroke='black' stroke-width='0' />
<path d='M67.2,273.3C64.8,270.4 54.2,258.2 50.9,255.1L50.9,252.8L65.1,252.8L65.1,245.7L50.9,245.7L50.9,224.3L43.6,224.3L43.6,245.7L25.8,245.7L25.8,252.8L42.3,252.8C38.5,266.6 30.8,282 23.3,290.4C24.6,292.2 26.4,295.1 27.3,297.3C33.3,290.3 39.3,278.7 43.6,266.7L43.6,315.5L50.9,315.5L50.9,264C55,269.2 60.1,276.1 62.3,279.7z M113.6,252.8L113.6,245.7L94.7,245.7L94.7,224.3L87.4,224.3L87.4,245.7L69.4,245.7L69.4,252.8L85.8,252.8C81.2,268.8 71.9,285.1 62.4,294.4C63.8,296.1 65.8,298.9 66.8,300.9C74.6,293.1 82.1,280.1 87.4,266.4L87.4,315.5L94.7,315.5L94.7,266.1C99.1,279.2 104.9,291.4 111,299.1C112.3,297.1 114.9,294.6 116.7,293.3C108.7,284.6 101.1,268.6 96.6,252.8z ' fill='black' stroke='black' stroke-width='0' />
<path d='M73.5,325.8L65.6,325.8L65.6,357.9C65.6,369.5 58.7,396.8 25.2,409.6C26.9,411.1 29.3,414.2 30.3,415.7C58.5,404.1 67.6,382 69.5,372.3C71.5,381.9 81.2,404.8 110.1,415.7C111.2,413.7 113.4,410.5 115,408.9C80.6,396.7 73.5,369.3 73.5,357.9z M102.7,344.3C99.4,353 93.1,365 88.2,372.3L94.4,375.2C99.5,368.1 105.9,356.8 110.7,347.5z M40.4,344.5C38.8,355.6 35.4,366.4 27.4,372.4L33.8,376.8C42.7,370 46,357.9 47.8,346z ' fill='black' stroke='black' stroke-width='0' />
<path d='M101.9,447.9L101.9,498.8L73.4,498.8L73.4,426.4L65.8,426.4L65.8,498.8L38.3,498.8L38.3,448L30.8,448L30.8,514.4L38.3,514.4L38.3,506.3L101.9,506.3L101.9,514L109.5,514L109.5,447.9z ' fill='black' stroke='black' stroke-width='0' />
<path d='M116.6,553.4L116.6,523.4L116.1,523.4C106.9,534.3 90.3,543.2 70,543.2C49.7,543.2 33.1,534.3 23.9,523.4L23.4,523.4L23.4,553.4z ' fill='black' stroke='black' stroke-width='0' />
</svg>
*/
```
![Example](./docs/examples/furinkazan.svg)

## Building

GlyphLoader is built using the Visual Studio Community 2019.
