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
        View view;
        ImageView sendButton;
        EditText text;
        string photoID;
        public bool CommentAdded = false;

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

            view = inflater.Inflate(Resource.Layout.Comments, container, false);
            listview = view.FindViewById<ListView>(Resource.Id.list);

            sendButton = view.FindViewById<ImageView>(Resource.Id.sendCommentButton);
            text = view.FindViewById<EditText>(Resource.Id.commentEditText);

            CommentsAdapter adapter = new CommentsAdapter(photoID);
            listview.Adapter = adapter;

            sendButton.Click += SendButton_Click;

            return view;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(text.Text))
            {
                MainActivity.twentyThree.PhotosCommentsAddComment(photoID, text.Text);
                (listview.Adapter as CommentsAdapter).Refresh();
                text.Text = String.Empty;
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