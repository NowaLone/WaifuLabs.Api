using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WaifuLabs.Api.Models
{
    /// <summary>
    /// Available intermediate girl options object.
    /// </summary>
    public class NewGirlsResponse
    {
        #region Properties

        /// <summary>
        /// A collection of available intermediate girl options.
        /// </summary>
        [JsonPropertyName("newGirls")]
        public ICollection<NewGirl> NewGirls { get; set; }

        #endregion Properties
    }
}