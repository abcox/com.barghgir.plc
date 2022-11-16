using com.barghgir.plc.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels;

[QueryProperty(nameof(Course), nameof(Course))]
public partial class CourseDetailViewModel : BaseViewModel
{
    public CourseDetailViewModel()
    {

    }

    [ObservableProperty]
    Course course;

}
