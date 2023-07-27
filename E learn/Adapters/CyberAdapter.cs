using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using FFImageLoading;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class CyberAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CyberAdapterClickEventArgs> ItemClick;
        public event EventHandler<CyberAdapterClickEventArgs> ItemLongClick;
        List<CyberModel> items;

        public CyberAdapter(List<CyberModel> data)
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

            var vh = new CyberAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as CyberAdapterViewHolder;
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

        void OnClick(CyberAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(CyberAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class CyberAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView Name { get; set; }
        public ImageView image { get; set; }

        public CyberAdapterViewHolder(View itemView, Action<CyberAdapterClickEventArgs> clickListener,
                            Action<CyberAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            Name = itemView.FindViewById<TextView>(Resource.Id.name);
            image = itemView.FindViewById<ImageView>(Resource.Id.companyImage);
            itemView.Click += (sender, e) => clickListener(new CyberAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new CyberAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class CyberAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}