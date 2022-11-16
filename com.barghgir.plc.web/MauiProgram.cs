using com.barghgir.plc.web.Services;
using com.barghgir.plc.web.ViewModels;
using com.barghgir.plc.web.Views;
using MauiIcons.Material;

namespace com.barghgir.plc.web;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMaterialMauiIcons();

		// Course
        builder.Services.AddTransient<CourseDetailPage>();
        builder.Services.AddSingleton<CourseDetailViewModel>();
        builder.Services.AddSingleton<CourseListViewModel>();
        builder.Services.AddSingleton<CourseService>();

        // General
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
