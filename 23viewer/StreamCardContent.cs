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
    }

    public class StreamCardContent
    {
        private List<PhotoInformation> photos = new List<PhotoInformation>();

        public StreamCardContent()
        {
            // Must be done asynchronously in the next step
            ContactCollection contacts = MainActivity.twentyThree.ContactsGetList();

            PhotoCollection collection = new PhotoCollection();
            PhotoSearchExtras searchOptions = PhotoSearchExtras.DateUploaded;

            foreach (var contact in contacts)
            {
                PhotoCollection userCollection = MainActivity.twentyThree.PeopleGetPublicPhotos(contact.UserId, 1, 10, SafetyLevel.None, searchOptions);
               
                foreach (var photo in userCollection)
                {
                    PhotoInformation info = new PhotoInformation() { photo = photo, Owner = contact };

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