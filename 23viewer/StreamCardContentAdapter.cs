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
using FFImageLoading;
using FFImageLoading.Work;
using FFImageLoading.Transformations;

namespace viewer
{
    public class StreamCardContentAdapter : RecyclerView.Adapter
    {
        public StreamCardContent StreamContent;
        public event EventHandler<int> ItemClick;

        public override int ItemCount
        {
            get { return StreamContent.NumPhotos; }
        }

        public StreamCardContentAdapter(StreamCardContent content)
        {
            StreamContent = content;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.StreamCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            StreamViewHolder vh = new StreamViewHolder(itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StreamViewHolder vh = holder as StreamViewHolder;
            ImageService.Instance.LoadUrl(StreamContent[position].photo.LargeUrl).DownSampleInDip(height: 220).Into(vh.Image);
            ImageService.Instance.LoadUrl(StreamContent[position].Owner.BuddyIconUrl).DownSampleInDip(height: 40).Transform(new CircleTransformation(20, "#7CD164")).Into(vh.BuddyIcon);

            // Load the photo caption from the photo album:
            string title = StreamContent[position].photo.Title;

            if (title.Length > 30)
            {
                title = StreamContent[position].photo.Title.Substring(0, 30) + "...";
            }
           
            vh.Caption.Text = title;

            string name = StreamContent[position].Owner.RealName;
            if (String.IsNullOrWhiteSpace(name))
            {
                name = StreamContent[position].Owner.UserName;
            }

            vh.User.Text = name;
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}