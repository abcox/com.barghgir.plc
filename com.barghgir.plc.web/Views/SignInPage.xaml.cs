using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage(SignInViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}