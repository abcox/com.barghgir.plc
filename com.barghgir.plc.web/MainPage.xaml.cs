using com.barghgir.plc.web.Models;
using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage(CourseListViewModel coursesViewModel)
	{
		InitializeComponent();
		BindingContext = coursesViewModel;
	}
}

