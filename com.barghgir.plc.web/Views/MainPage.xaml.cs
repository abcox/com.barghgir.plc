using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web.Views;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage(CourseListViewModel coursesViewModel)
	{
		InitializeComponent();
		BindingContext = coursesViewModel;
	}
}

