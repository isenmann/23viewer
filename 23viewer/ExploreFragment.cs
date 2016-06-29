using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;

namespace viewer
{
    public class ExploreFragment : Fragment
    {
        RecyclerView RecyclerView;
        LinearLayoutManager LayoutManager;
        ExploreCardContentAdapter ExploreContentAdapter;
        ExploreCardContent ExploreContent;
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
            b.PutString("photourl", ExploreContentAdapter.Photos[position].photo.LargeUrl);
            intent.PutExtras(b);
            StartActivity(intent);
        }

        private void MarkAsFavClick(object sender, int position)
        {
            if (ExploreContentAdapter.Photos[position].IsFavourite)
            {
                MainActivity.twentyThree.FavoritesRemove(ExploreContentAdapter.Photos[position].photo.PhotoId);
                Toast.MakeText(this.Context, Android.App.Application.Context.GetString(Resource.String.delete_as_fav), ToastLength.Short).Show();
            }
            else
            {
                MainActivity.twentyThree.FavoritesAdd(ExploreContentAdapter.Photos[position].photo.PhotoId);
                Toast.MakeText(this.Context, Android.App.Application.Context.GetString(Resource.String.marking_as_fav), ToastLength.Short).Show();
            }

            ExploreContentAdapter.FavouriteStatusChanged(position);
            MainActivity.twentyThree.InstanceCacheDisabled = true;
        }

        private void CommentsClick(object sender, int position)
        {
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            CommentsDialogFragment commentsDialog = new CommentsDialogFragment(ExploreContentAdapter.Photos[position].photo.PhotoId);
            commentsDialog.Show(transaction, "Comments dialog");
        }

        private void InfoClick(object sender, int e)
        {
            // Open info dialogfragment
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Explore, container, false);

            // Prepare the data source:
            ExploreContent = new ExploreCardContent();
            ExploreContent.LoadingFinished += ExploreContent_LoadingFinished;

            // Get our RecyclerView layout:
            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewExplore);
            RecyclerView.Visibility = ViewStates.Invisible;

            ExploreContentAdapter = new ExploreCardContentAdapter();
            ExploreContentAdapter.ImageClick += OnImageClick;
            ExploreContentAdapter.MarkAsFavClick += MarkAsFavClick;
            ExploreContentAdapter.CommentsClick += CommentsClick;
            ExploreContentAdapter.InfoClick += InfoClick;

            // Plug the adapter into the RecyclerView:
            RecyclerView.SetAdapter(ExploreContentAdapter);

            LayoutManager = new LinearLayoutManager(this.Context);
            RecyclerView.SetLayoutManager(LayoutManager);

            var onScrollListener = new ExploreEndlessScroll(LayoutManager);

            onScrollListener.LoadMoreEvent += LoadNextPage;

            RecyclerView.AddOnScrollListener(onScrollListener);

            RefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayoutExplore);

            RefreshLayout.SetColorSchemeColors(Resource.Color.tabBarColor, Resource.Color.statusBarColor);
            RefreshLayout.Refresh += RefreshLayout_Refresh;

            return view;
        }

        private void ExploreContent_LoadingFinished(object sender, List<PhotoInformation> e)
        {
            this.Activity.RunOnUiThread(() =>
            {
                (RecyclerView.GetAdapter() as ExploreCardContentAdapter).UpdatePhotos(e);
                RecyclerView.Visibility = ViewStates.Visible;
                progress.Hide();
            });
        }

        private void RefreshLayout_Refresh(object sender, System.EventArgs e)
        {
            // Do really an refresh of the data
            MainActivity.twentyThree.InstanceCacheDisabled = true;
            System.Threading.Tasks.Task s = new System.Threading.Tasks.Task(() => ExploreContent.GetPhotoInformation(false));
            s.ContinueWith(RefreshingFinished);
            s.Start();
        }

        private void RefreshingFinished(System.Threading.Tasks.Task t)
        {
            if (t.IsFaulted) { return; }

            this.Activity.RunOnUiThread(() =>
            {
                RefreshLayout.Refreshing = false;
                (RecyclerView.GetAdapter() as ExploreCardContentAdapter).UpdatePhotos(ExploreContent.Photos);
            });
        }

        private bool LoadingNextPage = false;

        private void LoadNextPage(object sender, System.EventArgs e)
        {
            if (LoadingNextPage)
                return;

            LoadingNextPage = true;

            // Do really an refresh of the data
            MainActivity.twentyThree.InstanceCacheDisabled = true;
            System.Threading.Tasks.Task s = new System.Threading.Tasks.Task(() => ExploreContent.LoadNextPage());
            s.ContinueWith(LoadNextPageFinished);
            s.Start();
        }

        private void LoadNextPageFinished(System.Threading.Tasks.Task t)
        {
            LoadingNextPage = false;
            MainActivity.twentyThree.InstanceCacheDisabled = false;

            if (t.IsFaulted) { return; }

            this.Activity.RunOnUiThread(() =>
            {
                (RecyclerView.GetAdapter() as ExploreCardContentAdapter).NextPageReady(ExploreContent.NewPhotos);
            });
        }
    }
}