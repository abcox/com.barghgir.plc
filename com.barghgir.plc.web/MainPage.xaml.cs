using com.barghgir.plc.web.Models;
using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web;

public partial class MainPage : ContentPage
{
	//int count = 0;

	public MainPage(CoursesViewModel coursesViewModel)
	{
		InitializeComponent();
		BindingContext = coursesViewModel;
	}

	//private void OnCounterClicked(object sender, EventArgs e)
	//{
	//	count++;

	//	if (count == 1)
	//		CounterBtn.Text = $"Clicked {count} time";
	//	else
	//		CounterBtn.Text = $"Clicked {count} times";

	//	SemanticScreenReader.Announce(CounterBtn.Text);
	//}
}

