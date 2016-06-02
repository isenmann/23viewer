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
using FFImageLoading.Views;
using FFImageLoading;
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
        private List<PhotoInformation> photos = new List<PhotoInformation>();

        public StreamCardContent()
        {
            // Must be done asynchronously in the next step
            ContactCollection contacts = MainActivity.twentyThree.ContactsGetList();
            PhotoCollection favourites = MainActivity.twentyThree.FavoritesGetList();

            PhotoCollection collection = new PhotoCollection();
            PhotoSearchExtras searchOptions = PhotoSearchExtras.All;

            foreach (var contact in contacts)
            {
                PhotoCollection photoCollection = MainActivity.twentyThree.PeopleGetPublicPhotos(contact.UserId, 1, 5, SafetyLevel.None, searchOptions);

                foreach (var photo in photoCollection)
                {
                    bool favourite = false;

                    foreach (var item in favourites)
                    {
                        if(item.PhotoId == photo.PhotoId)
                        {
                            favourite = true;
                            break;
                        }
                    }
                    
                    int numberOfComments = MainActivity.twentyThree.PhotosCommentsGetList(photo.PhotoId).Count;
                    int numberOfFavourites = MainActivity.twentyThree.PhotosGetFavorites(photo.PhotoId).Count;
                    
                    PhotoInformation info = new PhotoInformation() { photo = photo, Owner = contact, IsFavourite = favourite, NumberOfFavourites = numberOfFavourites, NumberOfComments = numberOfComments };
                    photos.Add(info);
                }
            }

            photos.Sort((a, b) => b.photo.DateUploaded.CompareTo(a.photo.DateUploaded));
        }

        public int NumPhotos
        {
            get { return photos.Count; }
        }

        public PhotoInformation this[int i]
        {
            get { return photos[i]; }
        }
    }
}