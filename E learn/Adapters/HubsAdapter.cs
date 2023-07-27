using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class HubsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<HubsAdapterClickEventArgs> ItemClick;
        public event EventHandler<HubsAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<HubsAdapterClickEventArgs> HubButtonClick;
        List<PlacesModel> items;

        public HubsAdapter(List<PlacesModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.hubRows;
            itemView = LayoutInflater.From(parent.Context).
                  Inflate(id, parent, false);

            var vh = new HubsAdapterViewHolder(itemView, OnClick, OnLongClick, OnHubButtonClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as HubsAdapterViewHolder;
            holder.Name.Text = items[position].HubName;
        }

        public override int ItemCount => items.Count;

        void OnClick(HubsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(HubsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        void OnHubButtonClick(HubsAdapterClickEventArgs args) => HubButtonClick?.Invoke(this, args);
    }

    public class HubsAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public Button Name { get; set; }

        public HubsAdapterViewHolder(View itemView, Action<HubsAdapterClickEventArgs> clickListener,
                            Action<HubsAdapterClickEventArgs> longClickListener, Action<HubsAdapterClickEventArgs> HubButtonClickListener) : base(itemView)
        {
            //TextView = v;
            Name = itemView.FindViewById<Button>(Resource.Id.name);
            Name.Click += (sender, e) => HubButtonClickListener(new HubsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.Click += (sender, e) => clickListener(new HubsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new HubsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class HubsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}