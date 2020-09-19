using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Memenim.Core;
using Memenim.Core.Data;

namespace MEMENIM_Android.Activities
{
    [Activity(Label = "PostActivity")]
    public class PostActivity : Activity
    {
        PostData m_PostData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PostActivityLayout);

            Task t = Task.Run(async () => 
            {
                var res = await PostAPI.GetPostById(Intent.GetIntExtra("PostID", 0), AppPersistent.UserToken);
                if(res.error)
                {
                    FeedbackHelper.ShowPopup(this, res.message);
                    return;
                }
                m_PostData = res.data[0];
            });

            t.Wait();
            InitContent();
            //t.ContinueWith(ct => InitContent());

        }

        void InitContent()
        {
            FindViewById<TextView>(Resource.Id.PostActityContent).Text = m_PostData.text;

            Bitmap attachmen = Utils.GetImageBitmapFromUrl(m_PostData.attachments[0].photo.photo_medium);
            FindViewById<ImageView>(Resource.Id.PostActityAttachment).SetImageBitmap(attachmen);

        }
    }
}