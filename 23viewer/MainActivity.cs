using Android.App;
using Android.Widget;
using Android.OS;
using TwentyThreeNet;
using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using Android.Graphics;
using System.Net;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Runtime;

namespace viewer
{
	[Activity (Label = "23 viewer", Icon = "@mipmap/icon", Theme = "@style/ToolbarTheme")]

    public class MainActivity : FragmentActivity
    {
        public static TwentyThree twentyThree = null;

        protected override void OnCreate (Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var fragments = new Android.Support.V4.App.Fragment[]
            {
                new StreamFragment(),
                new ExploreFragment(),
                new ProfileFragment()
            };

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments);

            // Give the TabLayout the ViewPager
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);

            TabLayout.Tab tabCall = tabLayout.GetTabAt(0);
            tabCall.SetIcon(Resource.Mipmap.ic_home_white_48dp);

            tabCall = tabLayout.GetTabAt(1);
            tabCall.SetIcon(Resource.Mipmap.ic_search_white_48dp);

            tabCall = tabLayout.GetTabAt(2);
            tabCall.SetIcon(Resource.Mipmap.ic_face_white_48dp);
        }
        
        /*
        private void Button_Click(object sender, System.EventArgs e)
        {
            // hard coded image from my personal 23 page
            // this is only for test purposes
            PhotoInfo col = twentyThree.PhotosGetInfo("22487590");
            TextView view = FindViewById<TextView>(Resource.Id.Description);
            view.Text = col.Description;

            //definitely not the way I will do it, this is just a fast hack for tests
            WebClient web = new WebClient();
            web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
            web.DownloadDataAsync(new Uri(col.Large1kUrl));
        }

        void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            //definitely not the way I will do it, this is just a fast hack for tests
            if (e.Error != null)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, e.Error.Message, ToastLength.Short).Show();
                });
            }
            else
            {
                Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);

                RunOnUiThread(() =>
                {
                    ImageView view = FindViewById<ImageView>(Resource.Id.imageView1);
                    view.SetImageBitmap(bm);
                });
            }
        }
        */
    }
}