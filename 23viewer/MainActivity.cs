using Android.App;
using Android.Widget;
using Android.OS;
using TwentyThreeNet;
using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using Android.Graphics;
using System.Net;

namespace viewer
{
	[Activity (Label = "23 viewer", Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
        public static TwentyThree twentyThree = null;

        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.myButton);
            button.Click += Button_Click;
		}
        
        private void Button_Click(object sender, System.EventArgs e)
        {
            // hard coded image from my personal 23 page
            // this is only for test purposes
            PhotoInfo col = twentyThree.PhotosGetInfo("22487590");
            TextView view = FindViewById<TextView>(Resource.Id.Description);
            view.Text = col.Description;

            //definitely not the way I will do it, this is just a fast hack for tests
            WebClient web = new WebClient();
            web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
            web.DownloadDataAsync(new Uri(col.Large1kUrl));
        }

        void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            //definitely not the way I will do it, this is just a fast hack for tests
            if (e.Error != null)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, e.Error.Message, ToastLength.Short).Show();
                });
            }
            else
            {
                Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);

                RunOnUiThread(() =>
                {
                    ImageView view = FindViewById<ImageView>(Resource.Id.imageView1);
                    view.SetImageBitmap(bm);
                });
            }
        }
    }
}