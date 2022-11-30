using com.barghgir.plc.web.ViewModels;

namespace com.barghgir.plc.web.Views;

public partial class CourseDetailPage : ContentPage
{
    public CourseDetailPage(CourseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        //_ = viewModel.GetCourseDetailCommand;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        //var viewModel = (CourseDetailViewModel)BindingContext;
        //if (viewModel.GetCourseDetailCommand.CanExecute(null))
        //    viewModel.GetCourseDetailCommand.Execute(null);
    }

    void OnContentPageUnloaded(object sender, EventArgs e)
    {
        video.Handler?.DisconnectHandler();
    }
}