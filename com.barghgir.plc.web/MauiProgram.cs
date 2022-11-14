using com.barghgir.plc.web.Services;
using com.barghgir.plc.web.ViewModels;
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

        builder.Services.AddSingleton<CoursesViewModel>();
        builder.Services.AddSingleton<CourseService>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
