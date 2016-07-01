using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace viewer
{
    public class PhotoDataDialogFragment : DialogFragment
    {
        View view;
        TextView title;
        TextView description;
        TextView viewCount;
        TextView camera;
        TextView cameraExif;
        TextView photoTags;

        TwentyThreeNet.Photo photo;
        public bool CommentAdded = false;

        public PhotoDataDialogFragment(TwentyThreeNet.Photo photo)
        {
            this.photo = photo;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = inflater.Inflate(Resource.Layout.PhotoData, container, false);
            title = view.FindViewById<TextView>(Resource.Id.photoDataTitle);
            description = view.FindViewById<TextView>(Resource.Id.photoDataDescription);
            viewCount = view.FindViewById<TextView>(Resource.Id.photoDataViews);
            camera = view.FindViewById<TextView>(Resource.Id.photoDataCamera);
            cameraExif = view.FindViewById<TextView>(Resource.Id.photoDataCameraExif);
            photoTags = view.FindViewById<TextView>(Resource.Id.photoDataTags);

            TwentyThreeNet.ExifTagCollection exifData = MainActivity.twentyThree.PhotosGetExif(this.photo.PhotoId, this.photo.Secret);

            int index = -1;

            if (photo.Title.Contains("\n"))
            {
                title.Text = photo.Title.Substring(0, photo.Title.IndexOf("\n"));
                index = photo.Title.IndexOf("\n");
            }
            else if (photo.Title.Contains("\r"))
            {
                title.Text = photo.Title.Substring(0, photo.Title.IndexOf("\r"));
                index = photo.Title.IndexOf("\r");
            }
            else
            {
                title.Text = photo.Title;
            }

            if (String.IsNullOrWhiteSpace(title.Text))
            {
                title.Text = " - ";
            }

            if (index != -1)
            {
                description.Text = photo.Title.Substring(index);
            }

            viewCount.Text = String.Format(Android.App.Application.Context.GetString(Resource.String.number_of_views), photo.Views);

            if(exifData.Count > 0)
            {
                var cameraMaker = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "Make");
                var cameraModel = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "Camera Model Name");

                camera.Text = cameraMaker?.Text + " " + cameraModel?.Text;

                var focalLength = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "Focal Length");
                if(focalLength != null)
                {
                    cameraExif.Text = focalLength.Text.Replace(" ","");
                }

                var fNumber = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "F Number");
                if (fNumber != null)
                {
                    cameraExif.Text += "  f/" + fNumber.Text;
                }

                var exposure = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "Exposure Time");
                if (exposure != null)
                {
                    cameraExif.Text += "  " + exposure.Text + "s";
                }

                var iso = exifData.FirstOrDefault<TwentyThreeNet.ExifTag>(p => p.Label == "ISO");
                if (iso != null)
                {
                    cameraExif.Text += "  ISO" + iso.Text;
                }

                if (String.IsNullOrWhiteSpace(cameraExif.Text))
                {
                    cameraExif.Text = " - ";
                }
            }

            foreach (var tag in photo.Tags)
            {
                photoTags.Text += tag + "   ";
            }

            if (String.IsNullOrWhiteSpace(photoTags.Text))
            {
                photoTags.Text = " - ";
            }

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.CommentsDialogAnimation; //set the animation
        }
    }
}