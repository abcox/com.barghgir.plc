using com.barghgir.plc.web.Helpers;
using com.barghgir.plc.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.Services
{
    public class CourseService
    {
        //HttpClient httpClient;

        List<Course> courseList = new();
        Course courseDetail = new();

        public CourseService()
        {
            //httpClient = new HttpClient();
        }

        public static string BaseAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "https://192.168.2.53:45455" /* "http://localhost:5260" "https://10.0.2.2:45455" "https://192.168.2.53:45455" "https://10.0.2.2:5001" */ : "https://localhost:7132";

        public static string CourseListUrl = $"{BaseAddress}/course/list";
        public static string CourseDetailUrl = $"{BaseAddress}/course/{{id}}/detail";

        public static HttpClient GetHttpClient()
        {
            HttpClient httpClient;
#if DEBUG
            //HttpsClientHandlerService handler = new HttpsClientHandlerService();
            //HttpClient httpClient = new HttpClient(handler.GetPlatformMessageHandler());

            //HttpClient httpClient = new DevHttpsConnectionHelper(7132).HttpClient;
            var handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());

#else
            httpClient = new HttpClient();
#endif
            return httpClient;
        }

        public async Task<Course> GetCourseDetailAsync(int id)
        {
            var response = await GetHttpClient().GetAsync($"{BaseAddress}/course/{id}/detail");

            if (response.IsSuccessStatusCode)
            {
                courseDetail = await response.Content.ReadFromJsonAsync<Course>();
            }
            // TODO: figure out how to send notifications to present user with feedback like "successful" or "error"

            return courseDetail;
        }

        public async Task<List<Course>> GetCourses()
        {
            var url = CourseListUrl; // "https://10.0.2.2:5001/course/list";
            var testJsonFilename = "courses.json";
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine($"Getting course list from url {url}");

                    var response = await GetHttpClient().GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        courseList = await response.Content.ReadFromJsonAsync<List<Course>>();
                    }
                }
                else
                {
                    Debug.WriteLine($"Url is empty. Will try to substitute with test JSON data from filename {testJsonFilename}");
                }

                if (courseList.Count == 0)
                {
                    Console.WriteLine($"Substituting with test JSON data from filename {testJsonFilename}");

                    using var stream = await FileSystem.OpenAppPackageFileAsync(testJsonFilename);
                    using var reader = new StreamReader(stream);
                    var contents = await reader.ReadToEndAsync();
                    courseList = JsonSerializer.Deserialize<List<Course>>(contents);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get courses from url {url} or from test JSON filename {testJsonFilename}. ERROR: {ex.Message}");
            }

            return courseList;
        }

    }
}
