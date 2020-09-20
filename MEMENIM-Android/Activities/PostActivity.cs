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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Memenim.Core;
using Memenim.Core.Data;
using MEMENIM_Android;

namespace MEMENIM.Activities
{
    [Activity(Label = "PostActivity")]
    public class PostActivity : Activity
    {
        PostData m_PostData;
        ListView m_CommentsList;

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
            m_CommentsList = FindViewById<ListView>(Resource.Id.PostCommentsList);
            Button sendBtn = FindViewById<Button>(Resource.Id.PostSendComment);
            sendBtn.Click += SendBtn_Click;
            //t.ContinueWith(ct => InitContent());
            t = Task.Run(async () =>
            {
                var res = await PostAPI.GetCommentsForPost(Intent.GetIntExtra("PostID", 0));
                if(res.error)
                {
                    return;
                }
                m_CommentsList.Adapter = new CommentAdapter(this, res.data);
            });
        }

        private async void SendBtn_Click(object sender, EventArgs e)
        {
            EditText content = FindViewById<EditText>(Resource.Id.PostCommentText);
            CheckBox anonCheck = FindViewById<CheckBox>(Resource.Id.PostCommentAnon);

            var res = await PostAPI.SendComment(Intent.GetIntExtra("PostID", 0), content.Text, anonCheck.Checked, AppPersistent.UserToken);

            if(res.error)
            {
                FeedbackHelper.ShowPopup(this, res.message);
            }
            else
            {
                FeedbackHelper.ShowPopup(this, "S U C C");
            }
        }

        void InitContent()
        {
            FindViewById<TextView>(Resource.Id.PostActityContent).Text = m_PostData.text;

            Bitmap attachmen = Utils.GetImageBitmapFromUrl(m_PostData.attachments[0].photo.photo_medium);
            FindViewById<ImageView>(Resource.Id.PostActityAttachment).SetImageBitmap(attachmen);

        }
    }

    public class CommentAdapter : BaseAdapter<CommentData>
    {
        List<CommentData> items;
        List<Bitmap> itemPics;
        Activity context;

        public CommentAdapter(Activity context, List<CommentData> items)
            : base()
        {
            this.context = context;
            this.items = items;
            itemPics = new List<Bitmap>();
            foreach (var it in items)
            {
                itemPics.Add(Utils.GetImageBitmapFromUrl(it.user.photo));
            }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override CommentData this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CommentItem, null);
            view.FindViewById<TextView>(Resource.Id.CommentPoster).Text = item.user.name;
            view.FindViewById<TextView>(Resource.Id.CommentContent).Text = item.text;
            view.FindViewById<ImageView>(Resource.Id.CommentImage).SetImageBitmap(itemPics[position]);

            return view;
        }
    }

}