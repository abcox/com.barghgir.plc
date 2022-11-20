using com.barghgir.plc.api.Data;
using com.barghgir.plc.api.Helpers;
using com.barghgir.plc.data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace com.barghgir.plc.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private static readonly string testDataFilePath = "data/courses.json";
        //private static readonly List<Course>? courses = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;

        private readonly ILogger<CourseController> logger;
        private readonly ApiOptions options;

        public CourseController(ILogger<CourseController> logger, IOptions<ApiOptions> options)
        {
            this.logger = logger;

            if (options?.Value != null) { logger.LogWarning("Api config failure"); }
            this.options = options?.Value ?? new ApiOptions { };
        }

        [HttpGet]
        [Route("list", Name = "GetCourseList")]
        public IEnumerable<Course>? GetList()
        {
            IEnumerable<Course>? response = null;

            try
            {
                var courses = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;

                // TODO:  filter/omit courses with no content (and make a GetCourseListForAdmin that brings them all back (implement filters/search)

                if (courses == null)
                {
                    logger.LogWarning($"Course data not found");
                    return null;
                }
                response = courses.Select(x => new Course
                {
                    Id = x.Id,
                    ImageId = x.ImageId,
                    Category = x.Category,
                    Content = x.Content,
                    ImageUrl = $"{options.Images.SourceUrl}/{x.ImageId}/{options.Images.ListBackgroundSize.WidthPx}/{options.Images.ListBackgroundSize.HeightPx}",
                    Subtitle = x.Subtitle,
                    Title = x.Title,
                })?.AsEnumerable();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something exceptional happened. {ex.Message}");
            }
            return response;
        }

        [HttpGet]
        [Route("{id}/detail", Name = "GetCourseDetail")]
        public Course? GetDetail(int id)
        {
            Course? course = null;

            try
            {
                var courseList = DataHelper.GetDataFromFile<Course>(testDataFilePath).Result;

                if (courseList == null)
                {
                    logger.LogWarning($"Courses has no data");
                    return null;
                }

                logger.LogInformation($"Courses count: {courseList.Count}");

                course = courseList.FirstOrDefault(x => x.Id == id);

                if (course == null)
                {
                    logger.LogWarning($"Course not found for id {id}");
                    return null;
                }

                var courseContent = DataHelper.GetDataFromFile<CourseContent>("data/CourseContent.json")
                    .Result?.AsQueryable().Where(x => x.CourseId == course.Id);

                if (courseContent == null)
                {
                    logger.LogWarning($"Content not found for course id {course.Id}");
                    return null;
                }

                var content = DataHelper.GetDataFromFile<Content>("data/Content.json")
                    .Result?.AsQueryable();

                if (content == null)
                {
                    logger.LogWarning($"Content has no data");
                    return null;
                }

                course.Content = (
                    from cc in courseContent
                    join c in content on cc.ContentId equals c.Id
                    select c
                    ).ToList();

                var i = 1;
                course.Content.ForEach(x => x.Index = i++);
                course.ImageUrl = $"{options.Images.SourceUrl}/{course.ImageId}/{options.Images.DetailBackgroundSize.WidthPx}/{options.Images.DetailBackgroundSize.HeightPx}";
            }
            catch ( Exception ex )
            {
                logger.LogError($"Something exceptional happened. {ex.Message}");
            }

            return course;
        }
    }
}