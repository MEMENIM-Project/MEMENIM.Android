using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Memenim.Core;
using Memenim.Core.Data;

namespace MEMENIM_Android.Fragmensts
{
    public class UserSettingsFragment : Fragment
    {
        TextView m_UserNameView;
        TextView m_LoginView;
        EditText m_ProfilePicURL;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static UserSettingsFragment NewInstance()
        {
            var frag = new UserSettingsFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.UserSettingsLayout, container, false);

            m_LoginView = view.FindViewById<TextView>(Resource.Id.UserLogin);
            m_UserNameView = view.FindViewById<TextView>(Resource.Id.UserName);
            m_ProfilePicURL = view.FindViewById<EditText>(Resource.Id.UserPicURL);

            Task t = Task.Run(async() =>
            {
                await UpdateData();
            });

            Button changeAvatar = view.FindViewById<Button>(Resource.Id.ChangeAvatarBtn);

            changeAvatar.Click += ChangeAvatar_Click;
            t.Wait();

            return view;
        }

        private async void ChangeAvatar_Click(object sender, EventArgs e)
        {
            var res = await UsersAPI.GetUserProfileByID(AppPersistent.LocalUserId);

            if (!res.error)
            {
                res.data[0].photo = m_ProfilePicURL.Text;
                await ChangeUserData(res.data[0]);
            }
            else
            {
                FeedbackHelper.ShowPopup(Activity, res.message);
            }
        }

        private async Task ChangeUserData(ProfileData data)
        {
            var res = await UsersAPI.EditProfile(data, AppPersistent.UserToken);
            if(res.error)
            {
                FeedbackHelper.ShowPopup(Activity, res.message);
            }
            else
            {
                FeedbackHelper.ShowPopup(Activity, "Success");
            }
        }

        private async Task UpdateData()
        {
            var requestData = await UsersAPI.GetUserProfileByID(AppPersistent.LocalUserId);

            if(!requestData.error)
            {
                m_LoginView.Text = requestData.data[0].login;
                m_UserNameView.Text = requestData.data[0].name;
            }
        }
    }
}