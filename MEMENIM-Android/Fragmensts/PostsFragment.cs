using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Memenim.Core;
using Memenim.Core.Data;
using Android.Support.V7.Widget;
using System.Threading.Tasks;
using Android.Graphics;
using System.Threading;
using MEMENIM.Activities;

namespace MEMENIM
{
    public class PostAdapter : RecyclerView.Adapter
    {
        List<PostData> items;
        List<Bitmap> itemPics;

        public event EventHandler<int> ItemClick;


        public PostAdapter(List<PostData> data)
        {
            items = data;
            itemPics = new List<Bitmap>();
            foreach(var it in items)
            {
                itemPics.Add(Utils.GetImageBitmapFromUrl(it.attachments[0].photo.photo_medium));
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // set the view's size, margins, paddings and layout parameters
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.PostCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            PostViewHolder vh = new PostViewHolder(itemView, OnClick);
            return vh;
        }


        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            PostViewHolder vh = viewHolder as PostViewHolder;

            // Load the photo image resource from the photo album:
            vh.Image.SetImageBitmap(itemPics[position]);

            // Load the photo caption from the photo album:
            vh.Post.Text = items[position].text;
        }

        public override int ItemCount
        {
            get
            {
                return items.Count;
            }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, items[position].id);
        }
    }

    public class PostViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; set; }
        public TextView Post { get; set; }


        public PostViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.postCardImage);
            Post = itemView.FindViewById<TextView>(Resource.Id.postCardContent);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }


    public class PostsFragment : Fragment
    {
        RecyclerView m_RecyclerView;
        PostAdapter m_PostsAdapter;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public static PostsFragment NewInstance()
        {
            var frag = new PostsFragment { Arguments = new Bundle() };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PostsLayout, container, false);

            m_RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.PostsList);

            // Plug in the linear layout manager:
            var layoutManager = new LinearLayoutManager(Activity) { Orientation = LinearLayoutManager.Vertical };
            m_RecyclerView.SetLayoutManager(layoutManager);
            m_RecyclerView.HasFixedSize = true;

            List<PostData> posts = new List<PostData>();

            Task t = Task.Run(async () =>
            {
                posts = await GetPosts(new PostRequest()
                {
                    count = 20,
                    type = PostRequest.EPostType.New
                });
            });

            t.Wait();
            InitPostsData(posts);
            //t.ContinueWith(ct => InitPostsData(posts));

            return view;
        }

        public async Task<List<PostData>> GetPosts(PostRequest filter)
        {
            var response = await PostAPI.GetAllPosts(filter, AppPersistent.UserToken);
            if (response.error)
            {
                return new List<PostData>();
            }

            return response.data;
        }

        void InitPostsData(List<PostData> posts)
        {
            m_PostsAdapter = new PostAdapter(posts);
            m_RecyclerView.SetAdapter(m_PostsAdapter);
            m_PostsAdapter.ItemClick += OnItemClick;
        }

        void OnItemClick(object sender, int position)
        {
            var intent = new Intent(Activity, typeof(PostActivity));
            intent.PutExtra("PostID", position);
            StartActivity(intent);
        }
    }
}