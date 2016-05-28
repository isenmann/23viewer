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
            ImageService.Instance.LoadUrl(StreamContent[position].LargeUrl).DownSampleInDip(height: 220).Into(vh.Image);

            // Load the photo caption from the photo album:
            vh.Caption.Text = StreamContent[position].Title;
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}