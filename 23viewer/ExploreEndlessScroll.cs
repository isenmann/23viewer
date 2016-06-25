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
using Android.Support.V7.Widget;

namespace viewer
{
    public class ExploreEndlessScroll : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;

        private LinearLayoutManager LayoutManager;

        public ExploreEndlessScroll(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount)
            {
                LoadMoreEvent(this, null);
            }
        }
    }
}