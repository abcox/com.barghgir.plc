using com.barghgir.plc.web.Helpers;
using com.barghgir.plc.web.Models;
using com.barghgir.plc.web.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels
{
    public partial class CoursesViewModel : BaseViewModel
    {
        CourseService courseService;

        public RangeEnabledObservableCollection<Course> Courses { get; } = new ();

        public CoursesViewModel(CourseService courseService)
        {
            Title = "Courses";
            this.courseService = courseService;
        }

        [RelayCommand]
        async Task GetCoursesAsync()
        {
            //if (IsBusy)
            //    return;
            if (GetCoursesCommand.IsRunning) return;

            try
            {
                IsBusy = true;

                var courses = await courseService.GetCourses();

                if (Courses.Count > 0)
                    Courses.Clear();

                Courses.CollectionChanged += (s, e) => Console.WriteLine("{0} changed", nameof(Courses));
                Courses.InsertRange(courses);
                Console.WriteLine("{0}.Count: {1}", nameof(Courses), Courses.Count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to get courses. Error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!",
                    $"Failed to get courses: {ex.Message}", "OK");
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
