using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels
{
    [QueryProperty(nameof(Course), nameof(Course))]
    public partial class CourseListEditViewModel : ObservableObject
    {
        public CourseListEditViewModel()
        {
            Title = "New Course";
        }

        [ObservableProperty]
        string title;

        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(GetCourseDetailCommand))]
        Course course;

        partial void OnCourseChanged(Course course)
        {
            //Task.Run(async () => {
            //    await this.GetCourseDetailAsync();
            //}).Wait();

            //await PresentationHelpers.Alert("");

            Title = course?.Title ?? Title;

            if (course == null)
            {

            }
            else
            {

            }
        }
    }
}
