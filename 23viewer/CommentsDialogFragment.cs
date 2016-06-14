using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace viewer
{
    public class CommentsDialogFragment : DialogFragment
    {
        ListView listview;
        string photoID;

        public CommentsDialogFragment(string photoID)
        {
            this.photoID = photoID;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.Comments, container, false);
            listview = view.FindViewById<ListView>(Resource.Id.list);

            CommentsAdapter adapter = new CommentsAdapter(photoID);
            adapter.AddCommentClick += Adapter_AddCommentClick;
            listview.Adapter = adapter;

            return view;
        }

        private void Adapter_AddCommentClick(object sender, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                MainActivity.twentyThree.PhotosCommentsAddComment(photoID, comment);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.CommentsDialogAnimation; //set the animation
        }
    }
}