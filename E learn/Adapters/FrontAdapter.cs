using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using FFImageLoading;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class FrontAdapter : RecyclerView.Adapter
    {
        public event EventHandler<FrontAdapterClickEventArgs> ItemClick;
        public event EventHandler<FrontAdapterClickEventArgs> ItemLongClick;
        List<FrontModel> items;

        public FrontAdapter(List<FrontModel> data)
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

            var vh = new FrontAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as FrontAdapterViewHolder;
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

        void OnClick(FrontAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(FrontAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class FrontAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView Name { get; set; }
        public ImageView image { get; set; }

        public FrontAdapterViewHolder(View itemView, Action<FrontAdapterClickEventArgs> clickListener,
                            Action<FrontAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            Name = itemView.FindViewById<TextView>(Resource.Id.name);
            image = itemView.FindViewById<ImageView>(Resource.Id.companyImage);
            itemView.Click += (sender, e) => clickListener(new FrontAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new FrontAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class FrontAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}