// <copyright file="FeatureRecord.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

namespace WaterTrans.GlyphLoader.OpenType
{
    /// <summary>
    /// The relationship of scripts, language systems, features, and lookups for substitution and positioning tables.
    /// </summary>
    public class FeatureRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureRecord"/> class.
        /// </summary>
        /// <param name="scriptTag">The script identification tag.</param>
        /// <param name="languageSystemTag">The language system tag.</param>
        /// <param name="featureTag">The feature system tag.</param>
        /// <param name="featureIndex">The feature index.</param>
        internal FeatureRecord(string scriptTag, string languageSystemTag, string featureTag, int featureIndex)
        {
            ScriptTag = scriptTag;
            LanguageSystemTag = languageSystemTag;
            FeatureTag = featureTag;
            FeatureIndex = featureIndex;
        }

        /// <summary>Gets an identification.</summary>
        public string Id
        {
            get { return ScriptTag + "." + LanguageSystemTag + "." + FeatureTag; }
        }

        /// <summary>Gets the script identification tag.</summary>
        public string ScriptTag { get; }

        /// <summary>Gets the language system tag.</summary>
        public string LanguageSystemTag { get; }

        /// <summary>Gets the feature system tag.</summary>
        public string FeatureTag { get; }

        /// <summary>Gets the feature index.</summary>
        public int FeatureIndex { get; }
    }
}
