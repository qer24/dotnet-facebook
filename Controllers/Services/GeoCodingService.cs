using Newtonsoft.Json;
using System.Globalization;

namespace dotnet_facebook.Controllers.Services;

public static class GeocodingService
{
    public class GeoResponse
    {
        [JsonProperty("results")]
        public List<GeoResult> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class GeoResult
    {
        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }

    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public GeoLocation Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class GeoLocation
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonProperty("northeast")]
        public GeoLocation Northeast { get; set; }

        [JsonProperty("southwest")]
        public GeoLocation Southwest { get; set; }
    }

    private const string API_KEY = "AIzaSyCx_DipJ1F4-No4jfYwGCCPkXXfMfaI6Bc";

    public static async Task<GeoResponse?> GetCityCountryAsync(double latitude, double longitude)
    {
        using (var httpClient = new HttpClient())
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}&key={API_KEY}";
            var response = await httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<GeoResponse>(response);
        }
    }

    public static (string City, string Country) ParseCityCountry(GeoResponse geoResponse)
    {
        var city = "";
        var country = "";

        if (geoResponse?.Results != null && geoResponse.Results.Count > 0)
        {
            var result = geoResponse.Results.FirstOrDefault();

            if (result == null)
            {
                return (city, country);
            }

            foreach (var component in result.AddressComponents)
            {
                if (component.Types.Contains("locality"))
                {
                    city = component.LongName;
                }
                else if (component.Types.Contains("country"))
                {
                    country = component.LongName;
                }
            }
        }

        return (city, country);
    }
}

