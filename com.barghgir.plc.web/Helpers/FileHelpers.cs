using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.barghgir.plc.common.Helpers
{
    public static class FileHelpers
    {
        public static async Task<T> GetDeserializedContent<T>(string path)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(path);
            //using var reader = new StreamReader(stream);
            //var contents = await reader.ReadToEndAsync();
            //var contents = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(stream);
        }
    }
}
