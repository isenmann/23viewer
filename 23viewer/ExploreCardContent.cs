using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwentyThreeNet;

namespace viewer
{
    public class ExploreCardContent
    {
        public List<PhotoInformation> Photos = new List<PhotoInformation>();
        public List<PhotoInformation> NewPhotos = new List<PhotoInformation>();
        public event EventHandler<List<PhotoInformation>> LoadingFinished;
        PhotoCollection Favourites = new PhotoCollection();
        public int PageIndex = 0;

        public ExploreCardContent()
        {
            Task.Factory.StartNew(() => GetPhotoInformation(true));
        }

        public void GetPhotoInformation(bool fireEvent)
        {
            Photos.Clear();

            Favourites = MainActivity.twentyThree.FavoritesGetList();
            Photos.AddRange(this.LoadPhotos());

            if (fireEvent)
            {
                LoadingFinished?.Invoke(this, Photos);
            }
        }

        private List<PhotoInformation> LoadPhotos()
        {
            List<PhotoInformation> photos = new List<PhotoInformation>();

            PhotoCollection photoCollection = MainActivity.twentyThree.PhotosGetRecent(++PageIndex, 3, PhotoSearchExtras.All);

            foreach (var photo in photoCollection)
            {
                bool favourite = false;

                foreach (var item in Favourites)
                {
                    if (item.PhotoId == photo.PhotoId)
                    {
                        favourite = true;
                        break;
                    }
                }

                PhotoInfo photoInfo = MainActivity.twentyThree.PhotosGetInfo(photo.PhotoId, photo.Secret);
                Contact contact = new Contact();
                contact.UserName = photoInfo.OwnerUserName;
                contact.RealName = photoInfo.OwnerRealName;
                contact.UserId = photoInfo.OwnerUserId;

                photo.DateUploaded = photoInfo.DateUploaded;
                var size = MainActivity.twentyThree.PhotosGetSizes(photo.PhotoId).First<Size>(p => p.Label.Equals("Medium"));

                PhotoInformation info = new PhotoInformation() { photo = photo, Owner = contact, IsFavourite = favourite, NumberOfFavourites = 0, NumberOfComments = 0, Size = size };
                photos.Add(info);
            }

            return photos;
        } 

        public void LoadNextPage()
        {
            NewPhotos.Clear();

            foreach (var photo in this.LoadPhotos())
            {
                NewPhotos.Add(photo);
            }

            Photos.AddRange(NewPhotos);
        }
    }
}