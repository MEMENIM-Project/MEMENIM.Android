using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Felipecsl.GifImageViewLib;
using Memenim.Core;
using MEMENIM_Android;

namespace MEMENIM.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextView m_LoginError;
        EditText m_UsernameView;
        EditText m_PassView;
        GifImageView m_LoadingGif;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ActivityLogin);

            m_LoginError = FindViewById<TextView>(Resource.Id.LoginError);
            m_UsernameView = FindViewById<EditText>(Resource.Id.LoginUser);
            m_PassView = FindViewById<EditText>(Resource.Id.LoginPass);
            Button btnLogin = FindViewById<Button>(Resource.Id.LoginBtn);
            m_LoadingGif = FindViewById<GifImageView>(Resource.Id.loadingCat);

            System.IO.Stream input = Assets.Open("progress_nc.gif");
            byte[] bytes = Utils.ConvertByteArray(input);

            m_LoadingGif.SetBytes(bytes);
            m_LoadingGif.StartAnimation();

            btnLogin.Click += BtnLogin_Click;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            ImageView overlay = FindViewById<ImageView>(Resource.Id.loadingOverlay);
            overlay.Visibility = ViewStates.Visible;
            m_LoadingGif.Visibility = ViewStates.Visible;
            var res = await UsersAPI.Login(m_UsernameView.Text, m_PassView.Text);
            if (res.error)
            {
                overlay.Visibility = ViewStates.Invisible;
                m_LoadingGif.Visibility = ViewStates.Invisible;
                m_LoginError.Text = res.message;
            }
            else
            {
                AppPersistent.UserToken = res.data.token;
                AppPersistent.LocalUserId = res.data.id;
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }
    }
}