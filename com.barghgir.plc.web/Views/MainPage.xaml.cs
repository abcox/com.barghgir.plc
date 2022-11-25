using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web.Views;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage(CourseListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Shell.Current.DisplayAlert(nameof(MainPage), nameof(OnAppearing), "OK");
    }

	protected override void OnDisappearing() { }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var isAdmin = (BindingContext as CourseListViewModel).IsAdmin;
        //if (isAdmin)
        //(this as ContentPage)?.ToolbarItems.Add(AdminToolbarItem);
        //else
        //(AdminToolbarItem.Parent as ContentPage)?.ToolbarItems.RemoveAt(1);
        //Shell.Current.DisplayAlert(nameof(MainPage), $"{nameof(OnNavigatedTo)}; isAdmin: {isAdmin}", "OK");
    }
}

