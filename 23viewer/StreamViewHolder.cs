using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using FFImageLoading.Views;

namespace viewer
{
    public class StreamViewHolder : RecyclerView.ViewHolder
    {
        public ImageViewAsync Image { get; private set; }
        public ImageViewAsync BuddyIcon { get; private set; }
        public TextView Caption { get; private set; }
        public TextView User { get; private set; }

        public StreamViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageViewAsync>(Resource.Id.imageView);
            BuddyIcon = itemView.FindViewById<ImageViewAsync>(Resource.Id.buddyImageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.title);
            User = itemView.FindViewById<TextView>(Resource.Id.username);

            itemView.Click += (sender, e) => listener(this.AdapterPosition);
        }
    }
}