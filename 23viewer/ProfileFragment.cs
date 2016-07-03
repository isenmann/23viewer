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

namespace viewer
{
    public class ProfileFragment : Fragment
    {
        private string userID;
        private TextView userName;
        private TextView userNick;
        private TextView contacts;
        private ImageViewAsync buddyIcon;

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
            contacts = view.FindViewById<TextView>(Resource.Id.countContacts);
            buddyIcon = view.FindViewById<ImageViewAsync>(Resource.Id.userBuddy);

            TwentyThreeNet.Person me = MainActivity.twentyThree.PeopleGetInfo(userID);
            userName.Text = me.RealName;
            userNick.Text = me.UserName;
            contacts.Text = String.Format(Android.App.Application.Context.GetString(Resource.String.number_of_contacts), MainActivity.twentyThree.ContactsGetList().Count);

            ImageService.Instance.LoadUrl(me.BuddyIconUrl).Transform(new CircleTransformation(5, "#7CD164")).Into(buddyIcon);

            return view;
        }
    }
}