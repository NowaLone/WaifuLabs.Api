using System.Collections.Generic;
using System.Threading.Tasks;
using WaifuLabs.Api.Enums;
using WaifuLabs.Api.Models;

namespace WaifuLabs.Api.Abstractions
{
    /// <summary>
    /// Client interface for iteraction with <see href="https://waifulabs.com"/> API.
    /// </summary>
    public interface IClient
    {
        #region Methods

        /// <summary>
        /// Returns intermediate girl seeds with images.
        /// </summary>
        /// <param name="currentGirl">Girl seed for generation. Should be <see langword="null"/> if <paramref name="step"/> is 0.</param>
        /// <param name="step">Generation step. Correct value is from 0 to 3.</param>
        /// <returns>An intermediate girl seeds with images.</returns>
        Task<NewGirlsResponse> Generate(IEnumerable<object> currentGirl, int step);

        /// <summary>
        /// Returns large size girl object.
        /// </summary>
        /// <param name="currentGirl">Girl seed for image generation.</param>
        /// <param name="step">Generation step. Correct value is from 0 to 4. </param>
        /// <param name="size">Generation size. Irrelevant.</param>
        /// <returns>A large size girl object.</returns>
        Task<GirlResponse> GenerateBig(IEnumerable<object> currentGirl, int step, int size = 512);

        /// <summary>
        /// Returns product image object with a girl.
        /// </summary>
        /// <param name="currentGirl">Girl seed for image generation.</param>
        /// <param name="productType">Product type.</param>
        /// <returns>A product image object with a girl.</returns>
        Task<GirlResponse> GeneratePreview(IEnumerable<object> currentGirl, Product productType);

        /// <summary>
        /// Returns a girl image object that has passed all the stages of generation.
        /// </summary>
        /// <returns>A girl image object that has passed all the stages of generation.</returns>
        Task<GirlResponse> GenerateRandom();

        #endregion Methods
    }
}