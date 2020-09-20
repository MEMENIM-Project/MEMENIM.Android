using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MEMENIM
{
    static class FeedbackHelper
    {
        public static void ShowPopup(Context context, string content)
        {
            var toast = Toast.MakeText(context, content, ToastLength.Short);
            toast.Show();
        }
    }
}