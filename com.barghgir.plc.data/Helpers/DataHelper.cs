using System.Text.Json;

namespace com.barghgir.plc.data.Helpers
{
    public static class DataHelper
    {
        public static async Task<List<T>?> GetJsonFromFile<T>(string jsonFilePath)
        {
            using var stream = File.Open(jsonFilePath, FileMode.Open);
            using var reader = new StreamReader(stream);
            string jsonString = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<List<T>>(jsonString, options);

            return jsonResponse;
        }
    }
}
