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
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Transformations;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace viewer
{
    public class ProfileFragment : Fragment
    {
        private string userID;
        private TextView userName;
        private TextView userNick;
        private TextView counts;
        private ImageViewAsync buddyIcon;
        private FragmentActivity context;

        public override void OnAttach(Context context)
        {
            this.context = (FragmentActivity)context;
            base.OnAttach(context);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var prefs = Android.App.Application.Context.GetSharedPreferences("23viewer.auth", FileCreationMode.Private);
            if (prefs != null)
            {
                userID = prefs.GetString("USERID", null);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Profile, container, false);
            userName = view.FindViewById<TextView>(Resource.Id.userName);
            userNick = view.FindViewById<TextView>(Resource.Id.nickName);
            counts = view.FindViewById<TextView>(Resource.Id.counts);
            buddyIcon = view.FindViewById<ImageViewAsync>(Resource.Id.userBuddy);

            TwentyThreeNet.Person me = MainActivity.twentyThree.PeopleGetInfo(userID);
            userName.Text = me.RealName;
            userNick.Text = me.UserName;
            counts.Text = String.Format(Android.App.Application.Context.GetString(Resource.String.profileStats), MainActivity.twentyThree.ContactsGetList().Count, me.PhotosSummary.PhotoCount);

            ImageService.Instance.LoadUrl(me.BuddyIconUrl).Transform(new CircleTransformation(5, "#7CD164")).Into(buddyIcon);

            var fragments = new Android.Support.V4.App.Fragment[]
           {
                new ProfilePhotos(),
                new ProfileAlbums(),
                new ProfileContacts()
           };

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpagerProfile);
            viewPager.Adapter = new TabsFragmentPagerAdapter(context.SupportFragmentManager, fragments);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsProfile);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.SetTabTextColors(Resource.Color.statusBarColor, Resource.Color.splash_background);

            TabLayout.Tab tabCall = tabLayout.GetTabAt(0);
            tabCall.SetText(Resource.String.photos);

            tabCall = tabLayout.GetTabAt(1);
            tabCall.SetText(Resource.String.albums);

            tabCall = tabLayout.GetTabAt(2);
            tabCall.SetText(Resource.String.contacts);

            return view;
        }
    }
}