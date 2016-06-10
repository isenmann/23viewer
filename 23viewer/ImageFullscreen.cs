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
using Square.Picasso;
using Android.Support.V4.App;
using ImageViews.Photo;

namespace viewer
{
    [Activity(Label = "ImageFullscreen", Theme = "@style/ToolbarTheme")]
    public class ImageFullscreen : FragmentActivity
    {
        PhotoViewAttacher mAttacher;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.SetStatusBarColor(new Android.Graphics.Color(124, 209, 100));

            SetContentView(Resource.Layout.ImageFullscreen);

            var imageView = FindViewById<ImageView>(Resource.Id.imageViewFullscreen);
            mAttacher = new PhotoViewAttacher(imageView);

            string photoID = Intent.GetStringExtra("photoid") ?? String.Empty;
            
            if(!String.IsNullOrWhiteSpace(photoID))
            {
                Picasso.With(Application.Context).Load(photoID).Priority(Picasso.Priority.High).Resize(1000, 0).Into(imageView, new Action(() => { mAttacher.Update(); }), new Action(() => { }));
            }
        }
    }
}