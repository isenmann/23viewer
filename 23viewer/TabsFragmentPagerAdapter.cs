using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Java.Lang;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.App;

namespace viewer
{
    public class TabsFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Android.Support.V4.App.Fragment[] fragments;

        public override int Count
        {
            get
            {
                return fragments.Length;
            }
        }

        public TabsFragmentPagerAdapter(Android.Support.V4.App.FragmentManager fm, Android.Support.V4.App.Fragment[] fragments) : base(fm)
        {
            this.fragments = fragments;
        }
        
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
    }
}