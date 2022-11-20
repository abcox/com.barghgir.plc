using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.web.ViewModels;

[QueryProperty(nameof(Course), nameof(Course))]
public partial class CourseDetailViewModel : BaseViewModel
{
    private readonly CourseService courseService;

    public CourseDetailViewModel(CourseService courseService)
    {
        this.courseService = courseService;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GetCourseDetailCommand))]
    Course course;

    partial void OnCourseChanged(Course value)
    {
        Task.Run(() => this.GetCourseDetailAsync()).Wait();
    }

    [RelayCommand]
    public async Task GetCourseDetailAsync()
    {
        if (GetCourseDetailCommand.IsRunning) return;

        try
        {
            IsBusy = true;

            course = await courseService.GetCourseDetailAsync(course.Id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get course detail. Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!",
                $"Failed to get course detail: {ex.Message}", "OK");
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }
}