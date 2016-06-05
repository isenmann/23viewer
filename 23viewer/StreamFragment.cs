using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace viewer
{
    public class StreamFragment : Fragment
    {
        RecyclerView RecyclerView;
        LinearLayoutManager LayoutManager;
        StreamCardContentAdapter StreamContentAdapter;
        StreamCardContent StreamContent;
        SwipeRefreshLayout RefreshLayout;
        Android.App.ProgressDialog progress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            progress = new Android.App.ProgressDialog(this.Context);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);

            progress.SetMessage(Resources.GetString(Resource.String.startup_Wait));
            progress.SetCancelable(false);
            progress.Show();
        }

        void OnItemClick(object sender, int position)
        {
            int photoNum = position + 1;

            // Just to test the click handle, will be replaced by displaying the photo in fullscreen in the near future
            Toast.MakeText(this.Context, "This photo number " + photoNum, ToastLength.Short).Show();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Stream, container, false);

            // Prepare the data source:
            StreamContent = new StreamCardContent();
            StreamContent.LoadingFinished += StreamContent_LoadingFinished;

            // Get our RecyclerView layout:
            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            RecyclerView.Visibility = ViewStates.Invisible;

            StreamContentAdapter = new StreamCardContentAdapter();
            StreamContentAdapter.ItemClick += OnItemClick;

            // Plug the adapter into the RecyclerView:
            RecyclerView.SetAdapter(StreamContentAdapter);

            LayoutManager = new LinearLayoutManager(this.Context);
            RecyclerView.SetLayoutManager(LayoutManager);

            RefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            RefreshLayout.SetColorSchemeColors(Resource.Color.tabBarColor, Resource.Color.statusBarColor);
            RefreshLayout.Refresh += RefreshLayout_Refresh;

            return view;
        }

        private void RefreshLayout_Refresh(object sender, System.EventArgs e)
        {
            System.Threading.Tasks.Task s = new System.Threading.Tasks.Task(() => StreamContent.GetPhotoInformation(false));
            s.Start();
            s.Wait();

            this.Activity.RunOnUiThread(() =>
            {
                (RecyclerView.GetAdapter() as StreamCardContentAdapter).UpdatePhotos(StreamContent.Photos);
            });

            RefreshLayout.Refreshing = false;
        }

        private void StreamContent_LoadingFinished(object sender, List<PhotoInformation> e)
        {
            this.Activity.RunOnUiThread(() =>
            {
                (RecyclerView.GetAdapter() as StreamCardContentAdapter).UpdatePhotos(e);
                RecyclerView.Visibility = ViewStates.Visible;
                progress.Hide();
            });
        }
    }
}