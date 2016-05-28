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
    public class StreamCardContent
    {
        private List<Photo> photos = new List<Photo>();

        public StreamCardContent()
        {
            // Must be done asynchronously in the next step
            ContactCollection contacts = MainActivity.twentyThree.ContactsGetList();

            PhotoCollection collection = new PhotoCollection();

            foreach (var item in contacts)
            {
                PhotoCollection userCollection = MainActivity.twentyThree.PeopleGetPublicPhotos(item.UserId, 1, 10, SafetyLevel.None, PhotoSearchExtras.DateUploaded);
                foreach (var photo in userCollection)
                {
                    photos.Add(photo);
                }
            }

            photos.Sort((a, b) => b.DateUploaded.CompareTo(a.DateUploaded));
        }

        public int NumPhotos
        {
            get { return photos.Count; }
        }

        public Photo this[int i]
        {
            get { return photos[i]; }
        }
    }
}