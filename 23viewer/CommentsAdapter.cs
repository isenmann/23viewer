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
using Java.Lang;
using TwentyThreeNet;
using FFImageLoading.Transformations;
using FFImageLoading;
using FFImageLoading.Views;

namespace viewer
{
    public class CommentsAdapter : BaseAdapter<PhotoComment>
    {
        PhotoCommentCollection Comments;

        public CommentsAdapter(string photoID) : base()
        {
            Comments = MainActivity.twentyThree.PhotosCommentsGetList(photoID);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return Comments.Count; }
        }

        public override PhotoComment this[int position]
        {
            get
            {
                return Comments.ToArray<TwentyThreeNet.PhotoComment>()[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; 

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.CommentsListItems, null);
            }

            view.FindViewById<TextView>(Resource.Id.comment).Text = Comments[position].CommentHtml;
            view.FindViewById<TextView>(Resource.Id.usernameComment).Text = Comments[position].AuthorUserName;
            view.FindViewById<TextView>(Resource.Id.commented).Text = Comments[position].DateCreated.ToShortDateString();

            Person user = MainActivity.twentyThree.PeopleGetInfo(Comments[position].AuthorUserId);
            ImageService.Instance.LoadUrl(user.BuddyIconUrl).DownSampleInDip(height: 40).Transform(new CircleTransformation(20, "#7CD164")).Into(view.FindViewById<ImageViewAsync>(Resource.Id.buddyImageViewComment));
            return view;
        }
    }
}