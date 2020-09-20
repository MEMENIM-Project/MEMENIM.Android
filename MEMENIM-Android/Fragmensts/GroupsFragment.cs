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
using MEMENIM_Android;

namespace MEMENIM.Fragmensts
{
    public class PlaceholderFragment : Fragment
    {
        string[] wipSmiles = new string[]
        {
            "凹(´･ω･｀)凹",
            "(っ´ω｀c)",
            "つ´Д`)つ",
            "(┛ಠДಠ)┛彡┻━┻"
        };

        TextView wipText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static PlaceholderFragment NewInstance()
        {
            var frag = new PlaceholderFragment { Arguments = new Bundle() };
            return frag;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.PlaceholderPageLayout, container, false);


            wipText = view.FindViewById<TextView>(Resource.Id.InProgressPlaceholderText);

            SetWIPText();

            return view;
        }

        void SetWIPText()
        {
            Random rnd = new Random();
            int pos = rnd.Next(0, wipSmiles.Length);
            wipText.Text = wipSmiles[pos];
        }

        public override void OnResume()
        {
            base.OnResume();
            SetWIPText();
        }
    }
}