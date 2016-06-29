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
        private PhotoCommentCollection Comments;
        private EditText EditCommentText;
        private ImageView SendCommentButton;

        public event EventHandler<string> AddCommentClick;

        public CommentsAdapter(string photoID) : base()
        {
            MainActivity.twentyThree.InstanceCacheDisabled = true;
            Comments = MainActivity.twentyThree.PhotosCommentsGetList(photoID);
            MainActivity.twentyThree.InstanceCacheDisabled = false;
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

            TimeSpan commentTimespan = DateTime.Now - Comments[position].DateCreated.ToLocalTime();

            string commentTimeText = string.Empty;

            if (commentTimespan.Days > 0)
            {
                if (commentTimespan.Days == 1)
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.day_ago), commentTimespan.Days.ToString());
                }
                else
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.days_ago), commentTimespan.Days.ToString());
                }
            }
            else if (commentTimespan.Hours > 0)
            {
                if (commentTimespan.Hours == 1)
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.hour_ago), commentTimespan.Hours.ToString());
                }
                else
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.hours_ago), commentTimespan.Hours.ToString());
                }

            }
            else if (commentTimespan.Minutes > 0)
            {
                if (commentTimespan.Minutes == 1)
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.minute_ago), commentTimespan.Minutes.ToString());
                }
                else
                {
                    commentTimeText = string.Format(Application.Context.GetString(Resource.String.minutes_ago), commentTimespan.Minutes.ToString());
                }
            }

            view.FindViewById<TextView>(Resource.Id.commented).Text = commentTimeText;

            Person user = MainActivity.twentyThree.PeopleGetInfo(Comments[position].AuthorUserId);
            string name = user.RealName;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = user.UserName;
            }
            
            view.FindViewById<TextView>(Resource.Id.usernameComment).Text = name;
            ImageService.Instance.LoadUrl(user.BuddyIconUrl).DownSampleInDip(height: 40).Transform(new CircleTransformation(20, "#7CD164")).Into(view.FindViewById<ImageViewAsync>(Resource.Id.buddyImageViewComment));
            return view;
        }

        public void Refresh()
        {
            // Reload comments
            string photoID = Comments.PhotoId;
            Comments.Clear();
            MainActivity.twentyThree.InstanceCacheDisabled = true;
            Comments = MainActivity.twentyThree.PhotosCommentsGetList(Comments.PhotoId);
            MainActivity.twentyThree.InstanceCacheDisabled = false;
        }
    }
}