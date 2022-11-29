#if IOS || MACCATALYST
using PlatformView = com.barghgir.plc.web.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = com.barghgir.plc.web.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = com.barghgir.plc.web.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using com.barghgir.plc.web.Controls;
using Microsoft.Maui.Handlers;

namespace com.barghgir.plc.web.Handlers;

public partial class VideoHandler
{
    public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(Video.Source)] = MapSource,
        [nameof(Video.IsLooping)] = MapIsLooping,
        [nameof(Video.Position)] = MapPosition
    };

    public static CommandMapper<Video, VideoHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(Video.UpdateStatus)] = MapUpdateStatus,
        [nameof(Video.PlayRequested)] = MapPlayRequested,
        [nameof(Video.PauseRequested)] = MapPauseRequested,
        [nameof(Video.StopRequested)] = MapStopRequested
    };

    public VideoHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}