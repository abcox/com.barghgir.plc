using com.barghgir.plc.web.Views;

namespace com.barghgir.plc.web;

public partial class AppShell : Shell
{
    public static readonly Dictionary<string, Type> pages = new Dictionary<string, Type>
	{
		{ nameof(CourseDetailPage), typeof(CourseDetailPage) }
	};

    public AppShell()
	{
		InitializeComponent();

		foreach (var page in pages)
			Routing.RegisterRoute(page.Key, page.Value);
	}
}
