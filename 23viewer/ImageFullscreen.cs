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
            mAttacher.PhotoTap += MAttacher_PhotoTap;
            string photoID = Intent.GetStringExtra("photourl") ?? String.Empty;
            
            if(!String.IsNullOrWhiteSpace(photoID))
            {
                Picasso.With(Application.Context).Load(photoID).Priority(Picasso.Priority.High).Resize(1000, 0).Into(imageView, new Action(() => { mAttacher.Update(); }), new Action(() => { }));
            }
        }

        private void MAttacher_PhotoTap(object sender, PhotoViewAttacher.PhotoTapEventArgs e)
        {
            View decorView = this.Window.DecorView;
            if (decorView != null)
            {
                if (decorView.SystemUiVisibility == StatusBarVisibility.Hidden)
                {
                    Window.SetStatusBarColor(new Android.Graphics.Color(124, 209, 100));
                    decorView.SystemUiVisibility = StatusBarVisibility.Visible;
                }
                else
                {
                    decorView.SystemUiVisibility = StatusBarVisibility.Hidden;
                    Window.SetStatusBarColor(new Android.Graphics.Color(0, 0, 0));
                }
            }
        }
    }
}