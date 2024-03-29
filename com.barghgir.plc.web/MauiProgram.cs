﻿using com.barghgir.plc.web.Controls;
using com.barghgir.plc.web.Handlers;
using com.barghgir.plc.web.Services;
using com.barghgir.plc.web.ViewModels;
using com.barghgir.plc.web.Views;
using CommunityToolkit.Maui;
using MauiIcons.Material;

namespace com.barghgir.plc.web;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont(filename: "MaterialIcons-Regular.ttf", alias: "MaterialIcons");
            })
			.UseMaterialMauiIcons()
			.ConfigureMauiHandlers(handlers =>
			{
                handlers.AddHandler(typeof(Video), typeof(VideoHandler));
            });

		// Course
        builder.Services.AddTransient<CourseDetailPage>();
        builder.Services.AddTransient<CourseDetailViewModel>();

        builder.Services.AddSingleton<CourseListViewModel>(); // Main page

        builder.Services.AddTransient<CourseListEditPage>();
        builder.Services.AddSingleton<CourseListEditViewModel>();

        builder.Services.AddSingleton<CourseService>();
        builder.Services.AddSingleton<MemberService>(); // has SignIn

        builder.Services.AddDataProtection();
		builder.Services.AddSingleton<DataProtectionService>();

        // General
        builder.Services.AddSingleton<AdminPage>();
        builder.Services.AddSingleton<AdminViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<SignInPage>();
        builder.Services.AddSingleton<SignInViewModel>();

        builder.Services.AddSingleton(Connectivity.Current);
        builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

        return builder.Build();
	}
}
