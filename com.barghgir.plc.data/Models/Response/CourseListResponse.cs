using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using com.barghgir.plc.data.Models;
using com.barghgir.plc.data.Models.Request;

namespace com.barghgir.plc.data.Models.Response;

public class CourseListResponse<T> : PagedResponse
{
    public List<T>? Items { get; set; }
    public Filter<T>? Filter { get; set; }
}