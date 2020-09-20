using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Memenim.Core;
using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using MEMENIM.Fragmensts;
using System.Threading.Tasks;
using MEMENIM_Android;

namespace MEMENIM.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        BottomNavigationView m_NavigationMenu;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            m_NavigationMenu = FindViewById<BottomNavigationView>(Resource.Id.BottomNavbar);
            m_NavigationMenu.NavigationItemSelected += NavigationMenu_NavigationItemSelected;

            if(string.IsNullOrEmpty(AppPersistent.UserToken))
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }
            LoadFragment(Resource.Id.menu_groups);
        }

        private void NavigationMenu_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }


        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_chat:
                case Resource.Id.menu_search:
                case Resource.Id.menu_groups:
                    fragment = PlaceholderFragment.NewInstance();
                    break;
                case Resource.Id.menu_posts:
                    fragment = PostsFragment.NewInstance();
                    break;
                case Resource.Id.menu_profile:
                    fragment = UserSettingsFragment.NewInstance();
                    break;
            }
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.FrameContent, fragment)
                .Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}