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
using Android.Util;

namespace viewer
{
	[Activity (Label = "23 viewer", Icon = "@mipmap/icon", Theme = "@style/ToolbarTheme")]
    public class MainActivity : FragmentActivity
    {
        public static TwentyThree twentyThree = null;

        protected override void OnCreate (Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            Window.SetStatusBarColor(new Android.Graphics.Color(124, 209, 100));

            SetContentView(Resource.Layout.Main);

            var fragments = new Android.Support.V4.App.Fragment[]
            {
                new StreamFragment(),
                new ExploreFragment(),
                new ProfileFragment()
            };

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);

            TabLayout.Tab tabCall = tabLayout.GetTabAt(0);
            tabCall.SetIcon(Resource.Drawable.home);

            tabCall = tabLayout.GetTabAt(1);
            tabCall.SetIcon(Resource.Drawable.search);

            tabCall = tabLayout.GetTabAt(2);
            tabCall.SetIcon(Resource.Drawable.face);
        }
    }
}