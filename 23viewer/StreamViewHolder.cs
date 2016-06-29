using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading.Views;
using System;

namespace viewer
{
    public class StreamViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public ImageViewAsync BuddyIcon { get; private set; }
        public TextView Caption { get; private set; }
        public TextView User { get; private set; }
        public TextView Date { get; private set; }
        public ImageView MarkedAsFavourite { get; private set; }
        public TextView NumberOfFavourites { get; private set; }
        public ImageView Comment { get; private set; }
        public TextView NumberOfComments { get; private set; }
        public ImageView Info { get; private set; }

        public Action<int> ImageClicked;
        public Action<int> MarkedAsFavClicked;
        public Action<int> CommentsClicked;
        public Action<int> InfoClicked;

        public StreamViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            BuddyIcon = itemView.FindViewById<ImageViewAsync>(Resource.Id.buddyImageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.title);
            User = itemView.FindViewById<TextView>(Resource.Id.username);
            Date = itemView.FindViewById<TextView>(Resource.Id.uploaded);
            MarkedAsFavourite = itemView.FindViewById<ImageView>(Resource.Id.favouritedImageView);
            NumberOfFavourites = itemView.FindViewById<TextView>(Resource.Id.numberFav);
            Comment = itemView.FindViewById<ImageView>(Resource.Id.commentImageView);
            NumberOfComments = itemView.FindViewById<TextView>(Resource.Id.numberComments);
            Info = itemView.FindViewById<ImageView>(Resource.Id.infoImageView);

            Image.Click += Image_Click;
            MarkedAsFavourite.Click += MarkedAsFavourite_Click;
            Comment.Click += Comment_Click;
            Info.Click += Info_Click;
        }

        private void Info_Click(object sender, EventArgs e)
        {
            InfoClicked?.Invoke(AdapterPosition);
        }

        private void Comment_Click(object sender, EventArgs e)
        {
            CommentsClicked?.Invoke(AdapterPosition);
        }

        private void MarkedAsFavourite_Click(object sender, EventArgs e)
        {
            MarkedAsFavClicked?.Invoke(AdapterPosition);
        }

        private void Image_Click(object sender, EventArgs e)
        {
            ImageClicked?.Invoke(AdapterPosition);
        }
    }
}