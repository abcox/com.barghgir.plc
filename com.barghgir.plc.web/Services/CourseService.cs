using com.barghgir.plc.web.Helpers;
using com.barghgir.plc.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using com.barghgir.plc.common.Configuration;
using Microsoft.Extensions.Logging;

namespace com.barghgir.plc.web.Services
{
    public class CourseService : BaseService
    {
        HttpClient httpClient;
        private readonly ILogger<CourseService> logger;

        public CourseService(
            IConfigurationService configurationService,
            ILogger<CourseService> logger) :base(configurationService)
        {
            this.httpClient = HttpHelper.GetHttpClient(); // new HttpClient();
            this.logger = logger;
        }

        public async Task<Course> GetCourseDetailAsync(int id)
        {
            var url = $"{BaseAddress}/course/{id}/detail";
            Console.WriteLine($"Getting course list from url {url}");
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"{nameof(GetCourseDetailAsync)} failed: {response.ReasonPhrase}");
            var result = await response.Content.ReadFromJsonAsync<Course>();
            return result;
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var url = $"{BaseAddress}/course/list"; // "https://10.0.2.2:5001/course/list";
            Console.WriteLine($"Getting course list from url {url}");
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"{nameof(GetCoursesAsync)} failed: {response.ReasonPhrase}");
            var result = await response.Content.ReadFromJsonAsync<List<Course>>();
            return result;
        }
    }
}
