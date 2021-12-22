using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WaifuLabs.Api.Abstractions;
using WaifuLabs.Api.Enums;
using WaifuLabs.Api.Models;

namespace WaifuLabs.Api.Client
{
    /// <summary>
    /// Provides a <see cref="IClient"/> implementation for interacting with <see href="https://waifulabs.com"/> API.
    /// </summary>
    public class Client : IClient, IDisposable
    {
        #region Fields

        private readonly Uri generateUri;
        private readonly Uri generateBigUri;
        private readonly Uri generatePreviewUri;
        private readonly HttpClient client;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class with the specified API uris.
        /// </summary>
        /// <param name="generateUri">Seed generation API uri.</param>
        /// <param name="generateBigUri">Large size image generation API uri.</param>
        /// <param name="generatePreviewUri">Product image genartion API uri.</param>
        public Client(Uri generateUri, Uri generateBigUri, Uri generatePreviewUri)
        {
            this.generateUri = generateUri;
            this.generateBigUri = generateBigUri;
            this.generatePreviewUri = generatePreviewUri;
            client = new HttpClient();
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            client.Dispose();
        }

        /// <inheritdoc cref="IClient.Generate(IEnumerable{object}, int)"/>
        public Task<NewGirlsResponse> Generate(IEnumerable<object> currentGirl, int step)
        {
            string content = JsonSerializer.Serialize(new { currentGirl, step });

            return SendRequest<NewGirlsResponse>(content, generateUri);
        }

        /// <inheritdoc cref="IClient.GenerateBig(IEnumerable{object}, int, int)"/>
        public Task<GirlResponse> GenerateBig(IEnumerable<object> currentGirl, int step, int size = 512)
        {
            string content = JsonSerializer.Serialize(new { currentGirl, step, size });

            return SendRequest<GirlResponse>(content, generateBigUri);
        }

        /// <inheritdoc cref="IClient.GeneratePreview(IEnumerable{object}, Product)"/>
        public Task<GirlResponse> GeneratePreview(IEnumerable<object> currentGirl, Product product)
        {
            string content = JsonSerializer.Serialize(new { currentGirl, product });

            return SendRequest<GirlResponse>(content, generatePreviewUri);
        }

        /// <inheritdoc cref="IClient.GenerateRandom"/>
        public async Task<GirlResponse> GenerateRandom()
        {
            Random random = new Random();

            NewGirlsResponse newGirlsResponse = await Generate(null, 0).ConfigureAwait(false);
            newGirlsResponse = await Generate(newGirlsResponse.NewGirls.Skip(random.Next(0, 16)).First().Seeds, 1).ConfigureAwait(false);
            newGirlsResponse = await Generate(newGirlsResponse.NewGirls.Skip(random.Next(0, 16)).First().Seeds, 2).ConfigureAwait(false);
            newGirlsResponse = await Generate(newGirlsResponse.NewGirls.Skip(random.Next(0, 16)).First().Seeds, 3).ConfigureAwait(false);
            return await GenerateBig(newGirlsResponse.NewGirls.Skip(random.Next(0, 16)).First().Seeds, 4).ConfigureAwait(false);
        }

        private async Task<T> SendRequest<T>(string content, Uri requestUri)
        {
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(content) })
            {
                using (HttpResponseMessage responseMessage = await client.SendAsync(httpRequestMessage).ConfigureAwait(false))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        using (Stream utf8Json = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            return await JsonSerializer.DeserializeAsync<T>(utf8Json).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        return default;
                    }
                }
            }
        }

        #endregion Methods
    }
}