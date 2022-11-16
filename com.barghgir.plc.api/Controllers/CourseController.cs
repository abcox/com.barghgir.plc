using com.barghgir.plc.api.Helpers;
using com.barghgir.plc.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace com.barghgir.plc.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private static readonly string testDataFilePath = "data/courses.json";
        //private static readonly List<Course>? courses = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;

        private readonly ILogger<CourseController> logger;

        public CourseController(ILogger<CourseController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("list", Name = "GetCourseList")]
        public IEnumerable<Course>? GetList()
        {
            var courses = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;

            if (courses == null)
            {
                logger.LogWarning($"Course data not found");
                return null;
            }

            return courses;
        }

        [HttpGet]
        [Route("{id}/detail", Name = "GetCourseDetail")]
        public CourseDetail? GetDetail(int id)
        {
            var courseList = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;
            
            if (courseList == null)
            {
                logger.LogWarning($"Course data not found");
                return null;
            }

            var course = courseList.FirstOrDefault(x => x.Id == id);

            if (course == null)
            {
                logger.LogWarning($"Course data not found for id {id}");
                return null;
            }

            return new CourseDetail
            {
                Id = id,
                MediaTracks = DataHelper.GetDataFromFile<MediaTrack>("data/media.json").Result ?? new List<MediaTrack> { },
                Title = course.Title
            };
        }
    }
}