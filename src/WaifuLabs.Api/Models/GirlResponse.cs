using System.Text.Json.Serialization;

namespace WaifuLabs.Api.Models
{
    /// <summary>
    /// Large size girl object.
    /// </summary>
    public class GirlResponse
    {
        #region Properties

        /// <summary>
        /// Large size image of a girl in Base64 format.
        /// </summary>
        [JsonPropertyName("girl")]
        public string Girl { get; set; }

        #endregion Properties
    }
}