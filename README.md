# GlyphLoader
GlyphLoader is a .NET Standerd library for TrueType, OpenType font.  
It is written in C#, designed to be small, efficient and portable while capable of producing high-quality glyph images.  
In WebAssembly environment, it can be used for application development using glyph outline information.  

## Features

- It provides the similar function as WPF GlyphTypeface class.
- The output glyph outline can be easily converted to SVG tags.
- It also supports vertical writing in Japanese.

## Release Notes

### 0.1-alpha

- Currently under development.

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
- OpenType(OTF) and OpenType collections(OTC) [comming soon]
- Web Open Font Format 2(WOFF2) [comming soon]

## License
MIT

## Building

GlyphLoader is built using the Visual Studio Community 2019.
