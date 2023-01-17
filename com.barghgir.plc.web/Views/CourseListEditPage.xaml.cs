using com.barghgir.plc.data.Models;
using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web.Views;

public partial class CourseListEditPage : ContentPage
{
	public CourseListEditPage(CourseListEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}