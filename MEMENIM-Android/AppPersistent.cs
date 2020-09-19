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

namespace MEMENIM_Android
{
    static class AppPersistent
    {
        public static string UserToken { get; set; }
        public static int LocalUserId { get; set; }
    }
}