using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using E_learn.Models;
using FFImageLoading;
using System;
using System.Collections.Generic;

namespace E_learn.Adapters
{
    class IntroAdapter : RecyclerView.Adapter
    {
        public event EventHandler<IntroAdapterClickEventArgs> ItemClick;
        public event EventHandler<IntroAdapterClickEventArgs> ItemLongClick;
        List<IntroModel> items;

        public IntroAdapter(List<IntroModel> data)
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

            var vh = new IntroAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as IntroAdapterViewHolder;
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

        void OnClick(IntroAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(IntroAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class IntroAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; set; }
        public ImageView image { get; set; }

        public IntroAdapterViewHolder(View itemView, Action<IntroAdapterClickEventArgs> clickListener,
                            Action<IntroAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.name);
            image = itemView.FindViewById<ImageView>(Resource.Id.companyImage);


            itemView.Click += (sender, e) => clickListener(new IntroAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new IntroAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class IntroAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}