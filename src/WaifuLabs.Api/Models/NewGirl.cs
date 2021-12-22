using System.Text.Json.Serialization;

namespace WaifuLabs.Api.Models
{
    /// <summary>
    /// Intermediate girl options.
    /// </summary>
    public class NewGirl
    {
        #region Properties

        /// <summary>
        /// Image of a girl in Base64 format.
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        /// Available seeds for the current girl.
        /// </summary>
        [JsonPropertyName("seeds")]
        public object[] Seeds { get; set; }

        #endregion Properties
    }
}