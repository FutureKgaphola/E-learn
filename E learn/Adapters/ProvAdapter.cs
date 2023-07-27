using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class ProvAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ProvAdapterClickEventArgs> ItemClick;
        public event EventHandler<ProvAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<ProvAdapterClickEventArgs> provClick;
        List<ProvModel> items;

        public ProvAdapter(List<ProvModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.provincesData;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ProvAdapterViewHolder(itemView, OnClick, OnLongClick, OnprovClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ProvAdapterViewHolder;
            holder.txtname.Text = items[position].provname;
        }

        public override int ItemCount => items.Count;

        void OnClick(ProvAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ProvAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        void OnprovClick(ProvAdapterClickEventArgs args) => provClick?.Invoke(this, args);
    }

    public class ProvAdapterViewHolder : RecyclerView.ViewHolder
    {
        public Button txtname { get; set; }

        public ProvAdapterViewHolder(View itemView, Action<ProvAdapterClickEventArgs> clickListener,
                            Action<ProvAdapterClickEventArgs> longClickListener,
                            Action<ProvAdapterClickEventArgs> provClickListener) : base(itemView)
        {
            txtname= itemView.FindViewById<Button>(Resource.Id.limp);
            txtname.Click+= (sender, e) => provClickListener(new ProvAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.Click += (sender, e) => clickListener(new ProvAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ProvAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ProvAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}