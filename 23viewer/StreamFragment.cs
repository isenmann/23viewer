using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwentyThreeNet;

namespace viewer
{
    public class StreamFragment : Fragment
    {
        RecyclerView RecyclerView;
        RecyclerView.LayoutManager LayoutManager;
        StreamCardContentAdapter StreamContentAdapter;
        StreamCardContent StreamContent;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);   
        }

        void OnItemClick(object sender, int position)
        {
            int photoNum = position + 1;

            // Just to test the click handle, will be replaced by displaying the photo in fullscreen in the near future
            Toast.MakeText(this.Context, "This photo number " + photoNum, ToastLength.Short).Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Stream, container, false);

            // Prepare the data source:
            StreamContent = new StreamCardContent();

            // Instantiate the adapter and pass in its data source:
            StreamContentAdapter = new StreamCardContentAdapter(StreamContent);
            StreamContentAdapter.ItemClick += OnItemClick;

            // Get our RecyclerView layout:
            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Plug the adapter into the RecyclerView:
            RecyclerView.SetAdapter(StreamContentAdapter);

            LayoutManager = new LinearLayoutManager(this.Context);
            RecyclerView.SetLayoutManager(LayoutManager);

            return view;
        }
    }
}