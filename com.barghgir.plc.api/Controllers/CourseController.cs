using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using com.barghgir.plc.data.Models;
using Context = com.barghgir.plc.data.Context;
using System.Linq;
using com.barghgir.plc.common.Extensions;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.Extensions.Hosting;

namespace com.barghgir.plc.api.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    public static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
    public static readonly string TestJsonPathForContent = Path.Combine(BaseDir, @$"seed\{nameof(Content)}.json");
    public static readonly string TestJsonPathForCourses = Path.Combine(BaseDir, @$"seed\{nameof(Course)}s.json");
    public static readonly string TestJsonPathForCourseContent = Path.Combine(BaseDir, @$"seed\{nameof(CourseContent)}.json");

    private readonly Context.CcaDevContext dbContext;
    private readonly string? imageSourceUrl;
    private readonly int? imageWidthPx;
    private readonly int? imageHeightPx;
    private readonly ILogger<CourseController> logger;
    private readonly ApiOptions options;

    public CourseController(
        Context.CcaDevContext dbContext,
        ILogger<CourseController> logger,
        IOptions<ApiOptions> apiOptions
        )
    {
        this.logger = logger;

        if (apiOptions?.Value == null) { logger.LogWarning("Api config failure"); }

        this.options = apiOptions?.Value ?? new ApiOptions { };
        this.dbContext = dbContext;
        this.imageSourceUrl = options?.Images?.SourceUrl;
        this.imageWidthPx = options?.Images?.ListBackgroundSize?.WidthPx;
        this.imageHeightPx = options?.Images?.ListBackgroundSize?.HeightPx;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseWithoutContent>), StatusCodes.Status200OK)]
    [Route("list", Name = "GetCourseList")]
    public IActionResult GetList()
    {
        IEnumerable<CourseWithoutContent>? response = null;

        try
        {
            //var courses = DataHelper.GetJsonFromFile<Course>(TestJsonPathForCourses).Result;

            var courses = dbContext.Courses.ToList();

            // TODO:  filter/omit courses with no content (and make a GetCourseListForAdmin that brings them all back (implement filters/search)

            if (courses == null || courses.Count == 0)
            {
                logger.LogWarning($"No data");
                return BadRequest();
            }
            response = courses.Select(x => new CourseWithoutContent
            {
                Id = x.Id,
                ImageId = x.ImageId,
                Category = x.Category,
                //Content = x.Content,
                ImageUrl = $"{imageSourceUrl}/{x.ImageId}/{imageWidthPx}/{imageHeightPx}",
                Subtitle = x.Subtitle,
                Title = x.Title,
                ContentTypeIcon = x.ContentTypeId == 1 ? "Headset" : "Videocam"
                //ContentType = new ContentType((int)x.ContentTypeId, (x.ContentTypeId == 1 ? ContentType.audio : ContentType.video).ToString())
            })?.AsEnumerable();
        }
        catch (Exception ex)
        {
            logger.LogError($"Something exceptional happened. {ex.Message}");
        }
        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(IEnumerable<CourseWithoutContent>), StatusCodes.Status200OK)]
    [Route("update", Name = "UpdateCourseItem")]
    public IActionResult UpdateCourseItem(Course course)
    {
        var id = course.Id;
        Context.Course? entity = dbContext.Courses.Find(course.Id);
        if  (entity == null)
        {
            logger.LogWarning($"Course not found for Id {id}");
            return new BadRequestObjectResult(new { id, message = "not found" });
        }
        try
        {
            dbContext.Entry(entity).CurrentValues.SetValues(course);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new BadRequestObjectResult(new { id, message = ex.Message });
        }
        return Ok(course);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Course), StatusCodes.Status200OK)]
    [Route("{id}/detail", Name = "GetCourseDetail")]
    public IActionResult GetDetail(int id)
    {
        Course? response = null;

        try
        {
            var course = dbContext.Courses
                .Include(x => x.CourseContents)
                .ThenInclude(x => x.Content)
                .FirstOrDefault(x => x.Id.Equals(id));

            if (course == null)
            {
                logger.LogWarning($"Course not found for id {id}");
                return BadRequest();
            }

            var imgSourceUrl = options?.Images?.SourceUrl;
            var imgWidthPx = options?.Images?.DetailBackgroundSize?.WidthPx ?? 0;
            var imgHeightPx = options?.Images?.DetailBackgroundSize?.HeightPx ?? 0;
            response = new Course(course)
            {
                Content = course.CourseContents.OrderBy(x => x.SortOrder ?? course.CourseContents.Count).Select(x => data.Models.Content.GetContent(x?.Content)).ToList(),
                ImageUrl = $"{imgSourceUrl}/{course.ImageId}/{imgWidthPx}/{imgHeightPx}"
            };
            var i = 1;
            response.Content.ToList().ForEach(x => {
                x.Index = i++;
                x.DurationSeconds = x.DurationSeconds ?? 0;
                x.DurationDisplay = $"{x.DurationSeconds / 60}:{$"{(x.DurationSeconds ?? 0) % 60}".PadLeft(2, '0')}";
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Something exceptional happened. {exceptionMessage}", ex.Message);
        }
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Course), StatusCodes.Status200OK)]
    [Route("{id}/detailFromJsonTestFiles", Name = "GetDetailFromJsonTestFiles")]
    public IActionResult GetDetailFromJsonTestFiles(int id)
    {
        Course? course = null;
        //await SqlHelper.SeedTablesFromJsonFiles();
        try
        {
            //var courseList = DataHelper.GetJsonFromFile<Course>(TestJsonPathForCourses).Result;

            var courseList = dbContext.Courses.ToList();

            if (courseList == null)
            {
                logger.LogWarning($"Courses has no data");
                return BadRequest();
            }

            logger.LogInformation($"Courses count: {courseList.Count}");

            course = new Course(courseList.FirstOrDefault(x => x.Id == id));

            if (course == null)
            {
                logger.LogWarning($"Course not found for id {id}");
                return BadRequest();
            }

            var courseContent = DataHelper.GetJsonFromFile<CourseContent>(TestJsonPathForCourseContent)
                .Result?.AsQueryable().Where(x => x.CourseId == course.Id);

            if (courseContent == null)
            {
                logger.LogWarning($"Content not found for course id {course.Id}");
                return BadRequest();
            }

            var content = DataHelper.GetJsonFromFile<Content>(TestJsonPathForContent)
                .Result?.AsQueryable();

            if (content == null)
            {
                logger.LogWarning($"Content has no data");
                return BadRequest();
            }

            course.Content = (
                from cc in courseContent
                join c in content on cc.ContentId equals c.Id
                select c
                ).ToList();

            var i = 1;
            course.Content = course.Content.Select(x => new Content
            {
                DurationDisplay = $"{x.DurationSeconds / 60}:{$"{(x?.DurationSeconds ?? 0) % 60}".PadLeft(2, '0')}",
                DurationSeconds = x?.DurationSeconds ?? 0,
                Id = x.Id,
                Source = i < 4 ? $"{options.Azure.Storage.Url}/{i}.mp4" : x.Source,
                Title = x.Title,
                Index = i++,
            }).ToList();
            course.ImageUrl = $"{options.Images.SourceUrl}/{course.ImageId}/{options.Images.DetailBackgroundSize.WidthPx}/{options.Images.DetailBackgroundSize.HeightPx}";
        }
        catch ( Exception ex )
        {
            logger.LogError("Something exceptional happened. {exceptionMessage}", ex.Message);
        }
        return Ok(course);
    }
}