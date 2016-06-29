using Android.Content;
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

        void OnImageClick(object sender, int position)
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(ImageFullscreen));
            Bundle b = new Bundle();
            b.PutString("photourl", StreamContentAdapter.Photos[position].photo.LargeUrl); 
            intent.PutExtras(b); 
            StartActivity(intent);
        }

        private void MarkAsFavClick(object sender, int position)
        {
            if (StreamContentAdapter.Photos[position].IsFavourite)
            {
                MainActivity.twentyThree.FavoritesRemove(StreamContentAdapter.Photos[position].photo.PhotoId);
                Toast.MakeText(this.Context, Android.App.Application.Context.GetString(Resource.String.delete_as_fav), ToastLength.Short).Show();
            }
            else
            {
                MainActivity.twentyThree.FavoritesAdd(StreamContentAdapter.Photos[position].photo.PhotoId);
                Toast.MakeText(this.Context, Android.App.Application.Context.GetString(Resource.String.marking_as_fav), ToastLength.Short).Show();
            }

            StreamContentAdapter.FavouriteStatusChanged(position);
            MainActivity.twentyThree.InstanceCacheDisabled = true;
        }

        private void CommentsClick(object sender, int position)
        {
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            CommentsDialogFragment commentsDialog = new CommentsDialogFragment(StreamContentAdapter.Photos[position].photo.PhotoId);
            commentsDialog.Show(transaction, "Comments dialog");
        }

        private void InfoClick(object sender, int e)
        {
            // Open info dialogfragment
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
            StreamContentAdapter.ImageClick += OnImageClick;
            StreamContentAdapter.MarkAsFavClick += MarkAsFavClick;
            StreamContentAdapter.CommentsClick += CommentsClick;
            StreamContentAdapter.InfoClick += InfoClick;

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
            // Do really an refresh of the data
            MainActivity.twentyThree.InstanceCacheDisabled = true;
            System.Threading.Tasks.Task s = new System.Threading.Tasks.Task(() => StreamContent.GetPhotoInformation(false));
            s.ContinueWith(RefreshingFinished);
            s.Start();
        }

        private void RefreshingFinished(System.Threading.Tasks.Task t)
        {
            MainActivity.twentyThree.InstanceCacheDisabled = false;

            if (t.IsFaulted) { return; }
            
            this.Activity.RunOnUiThread(() =>
            {
                RefreshLayout.Refreshing = false;
                (RecyclerView.GetAdapter() as StreamCardContentAdapter).UpdatePhotos(StreamContent.Photos);
            });   
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