using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace MEMENIM
{
    static class AppPersistent
    {
        public static string UserToken 
        {
            get
            {
                string val = "";
                Task t = Task.Run(async () => { val = await SecureStorage.GetAsync("UserToken"); });
                t.Wait();
                return val;
            }
            set 
            {
                Task t = Task.Run(async () => { await SecureStorage.SetAsync("UserToken", value); });
                t.Wait();
            }
        }
        public static int LocalUserId
        {
            get 
            {
                string val = "";
                Task t = Task.Run(async () => { val = await SecureStorage.GetAsync("LocalUserId"); });
                t.Wait();
                return Int32.Parse(val);

            }
            set 
            {
                Task t = Task.Run(async () => { await SecureStorage.SetAsync("LocalUserId", value.ToString()); });
                t.Wait();
            }
        }
    }
}