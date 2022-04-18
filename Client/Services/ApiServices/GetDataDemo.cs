using Dtos;

namespace Client.Services.ApiServices
{
    public class GetDataDemo : ApiServiceBase
    {
        public GetDataDemo(HttpClient client) : base(client)
        {
        }

        public async Task<List<WeatherForecast>> GetDemoData() => await GetAsync<List<WeatherForecast>>("api/WeatherForecast/GetWeatherForecast");
    }
}
