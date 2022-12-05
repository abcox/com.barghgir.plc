using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace com.barghgir.plc.web.Helpers;

public static class PresentationHelpers
{
    public static async Task Alert(string message, double fontSize = 14)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //string text = "This is a Toast";
        ToastDuration duration = ToastDuration.Short;
        //double fontSize = 14;
        var toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
}
