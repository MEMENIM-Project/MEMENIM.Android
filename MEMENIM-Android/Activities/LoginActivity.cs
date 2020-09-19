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
using Memenim.Core;

namespace MEMENIM_Android.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextView m_LoginError;
        EditText m_UsernameView;
        EditText m_PassView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ActivityLogin);

            m_LoginError = FindViewById<TextView>(Resource.Id.LoginError);
            m_UsernameView = FindViewById<EditText>(Resource.Id.LoginUser);
            m_PassView = FindViewById<EditText>(Resource.Id.LoginPass);
            Button btnLogin = FindViewById<Button>(Resource.Id.LoginBtn);

            btnLogin.Click += BtnLogin_Click;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var res = await UsersAPI.Login(m_UsernameView.Text, m_PassView.Text);
            if (res.error)
            {
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