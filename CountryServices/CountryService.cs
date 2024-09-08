using System.Net;
using System.Text.Json;

namespace CountryServices;

/// <summary>
/// Provides information about country local currency from RESTful API
/// <see><cref>https://restcountries.com/#api-endpoints-v2</cref></see>.
/// </summary>
public class CountryService : ICountryService
{
    private const string ServiceUrl = "https://restcountries.com/v2";

    /// <summary>
    /// Gets information about currency by country code synchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
    {
        try
        {
            string urlString = $"{ServiceUrl}/alpha/{alpha2Or3Code}?fields=name,currencies";
            var url = new Uri(urlString);

            using WebClient client = new WebClient();
            string response = client.DownloadString(url);
            var currencyInfo = JsonSerializer.Deserialize<LocalCurrencyInfo>(response);

            var currency = new LocalCurrency
            {
                CountryName = currencyInfo?.CountryName,
                CurrencyCode = currencyInfo?.Currencies?[0].Code,
                CurrencySymbol = currencyInfo?.Currencies?[0].Symbol,
            };
            return currency;
        }
        catch (Exception)
        {
            throw new ArgumentException("argument is invalid");
        }
    }

    /// <summary>
    /// Gets information about currency by country code asynchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>.
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
    {
        try
        {
            string urlString = $"{ServiceUrl}/alpha/{alpha2Or3Code}/?fields=name,currencies";
            var url = new Uri(urlString);

            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url, token);
            _ = response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(token);
            var currencyInfo = await JsonSerializer.DeserializeAsync<LocalCurrencyInfo>(stream, cancellationToken: token);
            var currency = new LocalCurrency
            {
                CountryName = currencyInfo?.CountryName,
                CurrencyCode = currencyInfo?.Currencies?[0].Code,
                CurrencySymbol = currencyInfo?.Currencies?[0].Symbol,
            };
            return currency;
        }
        catch (Exception)
        {
            throw new ArgumentException("argument is null");
        }
    }

    /// <summary>
    /// Gets information about the country by the country capital synchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <returns>Information about the country as <see cref="Country"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Country GetCountryInfoByCapital(string? capital)
    {
        try
        {
            string urlString = $"{ServiceUrl}/capital/{capital}";
            var url = new Uri(urlString);

            using WebClient client = new WebClient();
            string response = client.DownloadString(url);
            var countryInfoArray = JsonSerializer.Deserialize<CountryInfo[]>(response);
            var country = new Country
            {
                Name = countryInfoArray?[0].Name,
                CapitalName = countryInfoArray?[0].CapitalName,
#pragma warning disable
                Area = countryInfoArray[0].Area,
                Population = countryInfoArray[0].Population,
                Flag = countryInfoArray?[0].Flag,
#pragma warning enable
            };
            return country;
        }
        catch (Exception)
        {
            throw new ArgumentException("argument is invalid");
        }
    }

    /// <summary>
    /// Gets information about the currency by the country capital asynchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about the country as <see cref="Country"/>>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public async Task<Country> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
    {
        try
        {
            string urlString = $"{ServiceUrl}/capital/{capital}";
            var url = new Uri(urlString);

            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(url, token);
            _ = response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(token);
            var countryInfoArray = await JsonSerializer.DeserializeAsync<CountryInfo[]>(stream, cancellationToken: token);
            var country = new Country
            {
                Name = countryInfoArray?[0].Name,
                CapitalName = countryInfoArray?[0].CapitalName,
#pragma warning disable
                Area = countryInfoArray[0].Area,
                Population = countryInfoArray[0].Population,
                Flag = countryInfoArray?[0].Flag,
#pragma warning enable
            };
            return country;
        }
        catch (Exception)
        {
            throw new ArgumentException("argument is invalid");
        }
    }
}
