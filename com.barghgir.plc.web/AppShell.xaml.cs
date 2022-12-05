using com.barghgir.plc.web.Views;

namespace com.barghgir.plc.web;

public partial class AppShell : Shell
{
    public static readonly Dictionary<string, Type> pages = new Dictionary<string, Type>
    {
        //{ "/", typeof(MainPage) },
        { nameof(CourseDetailPage), typeof(CourseDetailPage) },
        { nameof(SignInPage), typeof(SignInPage) },
    };

    public AppShell()
	{
		InitializeComponent();

		foreach (var page in pages)
			Routing.RegisterRoute(route: page.Key, type: page.Value);
	}
}
