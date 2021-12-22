# WaifuLabs.Api
An unofficial library for interacting with https://waifulabs.com.

Support author of https://waifulabs.com [@sizigistudios](https://twitter.com/SizigiStudios)

[![](https://waifulabs.com/patreon_button.png)](https://www.patreon.com/bePatron?u=23037728)
[![](https://waifulabs.com/kofi_button.png)](https://ko-fi.com/B0B5106CI)

# Example

```CSharp
const string generateUriString = "https://api.waifulabs.com/generate";
const string generateBigUriString = "https://api.waifulabs.com/generate_big";
const string generatePreivewUriString = "https://api.waifulabs.com/generate_preview";

// Init client.
IClient client = new Api.Client.Client(new Uri(generateUriString), new Uri(generateBigUriString), new Uri(generatePreivewUriString));

// Choose your initial waifu.
NewGirlsResponse newGirlsResponse = await client.Generate(null, 0);

// Tune the color palette.
newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 1);

// Fine tune the details.
newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 2);

// Finish with your favorite pose!.
newGirlsResponse = await client.Generate(newGirlsResponse.NewGirls.First().Seeds, 3);

// Meet your waifu...
GirlResponse girlResponse = await client.GenerateBig(newGirlsResponse.NewGirls.First().Seeds, 4);

// On a pillow
GirlResponse girlResponsePillow = await client.GeneratePreview(newGirlsResponse.NewGirls.First().Seeds, Product.Pillow);

// And a poster.
GirlResponse girlResponsePoster = await client.GeneratePreview(newGirlsResponse.NewGirls.First().Seeds, Product.Poster);

// Or do it all in one step.
GirlResponse girlResponse = await client.GenerateRandom();
```