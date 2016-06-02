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
using Android.Content.Res;

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
            if (String.IsNullOrWhiteSpace(title))
            {
                title = StreamContent[position].photo.Description;
            }

            if (!String.IsNullOrWhiteSpace(title))
            {
                if (title.Length > 30)
                {
                    title = title.Substring(0, 30) + "...";
                }  
            }
            else
            {
                title = String.Empty;
            }

            vh.Caption.Text = title;

            string name = StreamContent[position].Owner.RealName;
            if (String.IsNullOrWhiteSpace(name))
            {
                name = StreamContent[position].Owner.UserName;
            }

            vh.User.Text = name;

            TimeSpan uploadTimespan = DateTime.Now - StreamContent[position].photo.DateUploaded;

            string uploadTimeText = String.Empty;

            if (uploadTimespan.Days > 0)
            {
                if (uploadTimespan.Days == 1)
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.day_ago), uploadTimespan.Days.ToString());
                }
                else
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.days_ago), uploadTimespan.Days.ToString());
                }
            }
            else if (uploadTimespan.Hours > 0)
            {
                if (uploadTimespan.Hours == 1)
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.hour_ago), uploadTimespan.Hours.ToString());
                }
                else
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.hours_ago), uploadTimespan.Hours.ToString());
                }

            }
            else if (uploadTimespan.Minutes > 0)
            {
                if (uploadTimespan.Minutes == 1)
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.minute_ago), uploadTimespan.Minutes.ToString());
                }
                else
                {
                    uploadTimeText = String.Format(Application.Context.GetString(Resource.String.minutes_ago), uploadTimespan.Minutes.ToString());
                }
            }

            vh.Date.Text = uploadTimeText;

            if (StreamContent[position].IsFavourite)
            {
                ImageService.Instance.LoadCompiledResource(Resource.Mipmap.ic_star_black_48dp.ToString()).DownSampleInDip(height: 40).Into(vh.MarkedAsFavourite);
            }
            else
            {
                ImageService.Instance.LoadCompiledResource(Resource.Mipmap.ic_star_border_black_48dp.ToString()).DownSampleInDip(height: 40).Into(vh.MarkedAsFavourite);
            }

            ImageService.Instance.LoadCompiledResource(Resource.Mipmap.ic_comment_black_48dp.ToString()).DownSampleInDip(height: 40).Into(vh.Comment);

            vh.NumberOfFavourites.Text = StreamContent[position].NumberOfFavourites.ToString();
            vh.NumberOfComments.Text = StreamContent[position].NumberOfComments.ToString();    
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}