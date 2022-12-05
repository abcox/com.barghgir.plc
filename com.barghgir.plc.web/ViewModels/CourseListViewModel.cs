using com.barghgir.plc.web.Helpers;
using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using CommunityToolkit.Maui.Core.Views;
using com.barghgir.plc.web.Views;

namespace com.barghgir.plc.web.ViewModels
{
    [QueryProperty(nameof(IsSignedIn), nameof(IsSignedIn))]
    [QueryProperty(nameof(IsAdmin), nameof(IsAdmin))]
    public partial class CourseListViewModel : BaseViewModel
    {
        CourseService courseService;

        public RangeEnabledObservableCollection<Course> Courses { get; } = new ();

        public CourseListViewModel(
            CourseService courseService,
            IConnectivity connectivity
            ): base(connectivity)
        {
            Title = "Courses";
            this.courseService = courseService;

            Task.Run(() => GetCoursesAsync());

            isSignedIn = false; // todo: wire up state
        }

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(ShowSignIn))]
        [NotifyPropertyChangedFor(nameof(SignInOutText))]
        bool isSignedIn;

        public bool ShowSignIn { get; } = true; // => !IsSignedIn;

        public string SignInOutText => isSignedIn ? "Sign-Out" : "Sign-In";

        [RelayCommand]
        async Task GoToDetailAsync(Course course)
        {
            if (course is null) return;
            
            Console.WriteLine($"Going to course '{course.Title}'...");

            await Shell.Current.GoToAsync(nameof(CourseDetailPage),true,new Dictionary<string, object>
            {
                {"Course", course}
            });
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
                var courses = await courseService.GetCoursesAsync();
                if (Courses.Count > 0)
                    Courses.Clear();
                Courses.CollectionChanged += (s, e) => Console.WriteLine("{0} changed", nameof(Courses));
                Courses.InsertRange(courses);
                await PresentationHelpers.Alert($"Loaded {Courses.Count} courses");
            }
            catch (Exception ex)
            {
                var msg = "Failed to get courses";
                Debug.WriteLine($"{msg}: {ex.Message}");
                //await Shell.Current.DisplayAlert("Error!",
                //    $"Failed to get courses: {ex.Message}", "OK");
                await PresentationHelpers.Alert(msg);
                //throw;
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
