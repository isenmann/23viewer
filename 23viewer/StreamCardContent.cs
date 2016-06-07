using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwentyThreeNet;

namespace viewer
{
    public class PhotoInformation
    {
        public Photo photo;
        public Contact Owner;
        public bool IsFavourite;
        public int NumberOfFavourites;
        public int NumberOfComments;
    }

    public class StreamCardContent
    {
        public List<PhotoInformation> Photos = new List<PhotoInformation>();
        public event EventHandler<List<PhotoInformation>> LoadingFinished;

        public StreamCardContent()
        {
            Task.Factory.StartNew(() => GetPhotoInformation(true));
        }

        public void GetPhotoInformation(bool fireEvent)
        {
            Photos.Clear();

            // Must be done asynchronously in the next step
            ContactCollection contacts = MainActivity.twentyThree.ContactsGetList();
            PhotoCollection favourites = MainActivity.twentyThree.FavoritesGetList();

            PhotoCollection collection = new PhotoCollection();
            PhotoSearchExtras searchOptions = PhotoSearchExtras.None;

            PhotoCollection photoCollection = MainActivity.twentyThree.PhotosGetContactsPhotos(50, false, false, false, searchOptions);

            foreach (var photo in photoCollection)
            {
                bool favourite = false;

                foreach (var item in favourites)
                {
                    if (item.PhotoId == photo.PhotoId)
                    {
                        favourite = true;
                        break;
                    }
                }

                Contact contact = contacts.FirstOrDefault<Contact>(p => p.UserId == photo.UserId);
                PhotoInfo photoInfo = MainActivity.twentyThree.PhotosGetInfo(photo.PhotoId, photo.Secret);

                photo.DateUploaded = photoInfo.DateUploaded;

                PhotoInformation info = new PhotoInformation() { photo = photo, Owner = contact, IsFavourite = favourite, NumberOfFavourites = 0, NumberOfComments = 0 };
                Photos.Add(info);
            }

            Photos.Sort((a, b) => b.photo.DateUploaded.CompareTo(a.photo.DateUploaded));

            // Activate cache for all other operations again
            MainActivity.twentyThree.InstanceCacheDisabled = false;
            
            if (fireEvent)
            {
                LoadingFinished?.Invoke(this, Photos);
            }
        }
    }
}