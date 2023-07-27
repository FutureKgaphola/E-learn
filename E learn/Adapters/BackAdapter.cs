using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using FFImageLoading;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class BackAdapter : RecyclerView.Adapter
    {
        public event EventHandler<BackAdapterClickEventArgs> ItemClick;
        public event EventHandler<BackAdapterClickEventArgs> ItemLongClick;
        List<BackendModel> items;

        public BackAdapter(List<BackendModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.rowData;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new BackAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as BackAdapterViewHolder;
            holder.Name.Text = items[position].name;
            getimage(items[position].img, holder.image);
        }
        void getimage(string imageUrl, ImageView imageView)
        {
            if (imageUrl.ToLower().Contains("http") == true)
            {
                ImageService.Instance.LoadUrl(imageUrl)
                .Retry(3, 200)
                .DownSample(350, 350)
                .Into(imageView);
            }
            else { imageView.SetBackgroundResource(Resource.Mipmap.elearn); }
        }


        public override int ItemCount => items.Count;

        void OnClick(BackAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(BackAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class BackAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView Name { get; set; }
        public ImageView image { get; set; }

        public BackAdapterViewHolder(View itemView, Action<BackAdapterClickEventArgs> clickListener,
                            Action<BackAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.name);
            image = itemView.FindViewById<ImageView>(Resource.Id.companyImage);
            itemView.Click += (sender, e) => clickListener(new BackAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new BackAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class BackAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}