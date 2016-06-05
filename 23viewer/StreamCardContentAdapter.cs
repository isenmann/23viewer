using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Transformations;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;

namespace viewer
{
    public class StreamCardContentAdapter : RecyclerView.Adapter
    {
        private List<PhotoInformation> Photos = new List<PhotoInformation>();
        public event EventHandler<int> ItemClick;

        public override int ItemCount
        {
            get { return Photos.Count; }
        }

        public StreamCardContentAdapter()
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.StreamCardView, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            StreamViewHolder vh = new StreamViewHolder(itemView, OnClick);
            return vh;
        }

        public void UpdatePhotos(List<PhotoInformation> newPhotos)
        {
            this.Photos.Clear();
            this.Photos.AddRange(newPhotos);

            this.NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StreamViewHolder vh = holder as StreamViewHolder;

            Picasso.With(Application.Context).Load(Photos[position].photo.MediumUrl).Resize(500,0).Into(vh.Image);
            ImageService.Instance.LoadUrl(Photos[position].Owner.BuddyIconUrl).DownSampleInDip(height: 40).Transform(new CircleTransformation(20, "#7CD164")).Into(vh.BuddyIcon);

            // Load the photo caption from the photo album:
            string title = Photos[position].photo.Title;
            if (String.IsNullOrWhiteSpace(title))
            {
                title = Photos[position].photo.Description;
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

            string name = Photos[position].Owner.RealName;
            if (String.IsNullOrWhiteSpace(name))
            {
                name = Photos[position].Owner.UserName;
            }

            vh.User.Text = name;

            TimeSpan uploadTimespan = DateTime.Now - Photos[position].photo.DateUploaded;

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

            if (Photos[position].IsFavourite)
            {
                Picasso.With(Application.Context).Load(Resource.Mipmap.ic_star_black_48dp).Fit().Into(vh.MarkedAsFavourite);
            }
            else
            {
                Picasso.With(Application.Context).Load(Resource.Mipmap.ic_star_border_black_48dp).Fit().Into(vh.MarkedAsFavourite);
            }

            Picasso.With(Application.Context).Load(Resource.Mipmap.ic_comment_black_48dp).Fit().Into(vh.Comment);

            MainActivity.twentyThree.PhotosCommentsGetListAsync(Photos[position].photo.PhotoId, OnPhotosCommentsGetList);
            MainActivity.twentyThree.PhotosGetFavoritesAsync(Photos[position].photo.PhotoId, OnPhotosGetFavorites);

            vh.NumberOfFavourites.Text = Photos[position].NumberOfFavourites.ToString();
            vh.NumberOfComments.Text = Photos[position].NumberOfComments.ToString();    
        }

        void OnPhotosCommentsGetList(TwentyThreeNet.TwentyThreeResult<TwentyThreeNet.PhotoCommentCollection> result)
        {
            if (!result.HasError)
            {
                PhotoInformation info = Photos.FirstOrDefault<PhotoInformation>(p => p.photo.PhotoId == result.Result.PhotoId);
                if (info != null)
                {
                    if(info.NumberOfComments != result.Result.Count)
                    {
                        info.NumberOfComments = result.Result.Count;
                        this.NotifyItemChanged(Photos.IndexOf(info));
                    }
                }
            }
        }

        void OnPhotosGetFavorites(TwentyThreeNet.TwentyThreeResult<TwentyThreeNet.PhotoFavoriteCollection> result)
        {
            if (!result.HasError)
            {
                PhotoInformation info = Photos.FirstOrDefault<PhotoInformation>(p => p.photo.PhotoId == result.Result.PhotoId);
                if (info != null)
                {
                    if (info.NumberOfFavourites != result.Result.Count)
                    {
                        info.NumberOfFavourites = result.Result.Count;
                        this.NotifyItemChanged(Photos.IndexOf(info));
                    }
                }
            }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
    }
}