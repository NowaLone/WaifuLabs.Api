using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WaifuLabs.Api.Abstractions;
using WaifuLabs.Api.Enums;
using WaifuLabs.Api.Models;

namespace WaifuLabs.Api.Tests.Client
{
    [TestClass]
    public class ClientTests
    {
        private const string generateUriString = "https://api.waifulabs.com/generate";
        private const string generateBigUriString = "https://api.waifulabs.com/generate_big";
        private const string generatePreivewUriString = "https://api.waifulabs.com/generate_preview";

        private IClient client;

        [TestInitialize]
        public void Initialize()
        {
            client = new Api.Client.Client(new Uri(generateUriString), new Uri(generateBigUriString), new Uri(generatePreivewUriString));
        }

        [TestCleanup]
        public void Cleanup()
        {
            (client as Api.Client.Client).Dispose();
        }

        [DataTestMethod, TestCategory("Generate"), TestCategory("GenerateBig"), TestCategory("GeneratePreview")]
        public async Task Generate_AllSteps_ShouldReturnCorrectImage()
        {
            // Arrange
            // Act
            NewGirlsResponse newGirlsResponse = await client.Generate(null, 0).ConfigureAwait(false);
            newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 1).ConfigureAwait(false);
            newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 2).ConfigureAwait(false);
            newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 3).ConfigureAwait(false);
            GirlResponse girlResponse = await client.GenerateBig(newGirlsResponse.NewGirls.First().Seeds, 4).ConfigureAwait(false);
            GirlResponse girlResponsePillow = await client.GeneratePreview(newGirlsResponse.NewGirls.First().Seeds, Product.Pillow).ConfigureAwait(false);
            GirlResponse girlResponsePoster = await client.GeneratePreview(newGirlsResponse.NewGirls.First().Seeds, Product.Poster).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(newGirlsResponse);
            Assert.AreEqual(16, newGirlsResponse.NewGirls.Count);
            foreach (NewGirl item in newGirlsResponse.NewGirls)
            {
                Assert.IsNotNull(item.Seeds);
                Assert.AreEqual(18, item.Seeds.Length);
                Assert.IsTrue(Convert.TryFromBase64String(item.Image, new Span<byte>(new byte[item.Image.Length]), out _));
            }

            Assert.IsNotNull(girlResponse);
            Assert.IsTrue(Convert.TryFromBase64String(girlResponse.Girl, new Span<byte>(new byte[girlResponse.Girl.Length]), out _));

            Assert.IsNotNull(girlResponsePillow);
            Assert.IsTrue(Convert.TryFromBase64String(girlResponsePillow.Girl, new Span<byte>(new byte[girlResponsePillow.Girl.Length]), out _));

            Assert.IsNotNull(girlResponsePoster);
            Assert.IsTrue(Convert.TryFromBase64String(girlResponsePoster.Girl, new Span<byte>(new byte[girlResponsePoster.Girl.Length]), out _));
        }

        [TestMethod, TestCategory("Generate")]
        public async Task Generate_WrongStep_ShouldReturnNull()
        {
            // Arrange
            NewGirlsResponse newGirls = await client.Generate(null, 0).ConfigureAwait(false);

            // Act
            NewGirlsResponse newGirlsResponse = await client.Generate(newGirls.NewGirls.First().Seeds, 4).ConfigureAwait(false);

            // Assert
            Assert.IsNull(newGirlsResponse);
        }

        [TestMethod, TestCategory("Generate")]
        public async Task Generate_WrongData_ShouldReturnDefault()
        {
            // Arrange
            // Act
            NewGirlsResponse newGirlsResponse = await client.Generate(null, 4).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(default, newGirlsResponse);
        }

        [TestMethod, TestCategory("GenerateBig")]
        public async Task GenerateBig_WrongData_ShouldReturnDefault()
        {
            // Arrange
            // Act
            GirlResponse girlResponse = await client.GenerateBig(null, 4).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(default, girlResponse);
        }

        [TestMethod, TestCategory("GeneratePreview")]
        public async Task GeneratePreview_WrongData_ShouldReturnDefault()
        {
            // Arrange
            // Act
            GirlResponse girlResponse = await client.GeneratePreview(null, default).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(default, girlResponse);
        }

        [TestMethod, TestCategory("GenerateRandom")]
        public async Task GenerateRandom_ShouldReturnCorrectImage()
        {
            // Arrange
            // Act
            GirlResponse girlResponse = await client.GenerateRandom().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(girlResponse);
            Assert.IsTrue(Convert.TryFromBase64String(girlResponse.Girl, new Span<byte>(new byte[girlResponse.Girl.Length]), out _));
        }
    }
}