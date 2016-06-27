# 23 viewer

23 viewer should become an Android app for the 23 photo community. 

At the moment the app is a work in progress. You can authorize the app against your 23 account and you will see a list of all your contacts' photos. The image, description, username and the buddy icon should be displayed, that's it at the moment. Nothing more.

I will add new features (better design, full support of all functionality of 23, ...) in the future. As I said this is a work in progress.

I have nothing to do with the company behind 23 and I'm not an employee of them. I do this app in my spare time, mostly to learn to code on Android with C#.

For connecting the app to 23, I use my own 23 API DLL (heavily based on flickrnet-dll), which you can find here: https://github.com/isenmann/twentythree-net

## Donations

If you want to support me, you can donate at: https://www.paypal.me/danielisenmann

## Screenshots

This short gif animation shows the current state of the app. The navigation with its three swipable tabs are ready, the cards with the images and titles are downloaded on demand while scrolling through the list. This card will be look much better in the future with a lot more information, this is only the first step right now. The displayed photos are the photos of your contacts, sorted by date.

<img src="https://raw.githubusercontent.com/isenmann/23viewer/master/Screenshots/StreamAnimation.gif" width="240">

A few words to the navigation at the top. The tab with the home icon should display all new photos from your contacts and is an endless scrolling list as you can see in the gif animation. Of course, you can tap on an image to see it in fullscreen, see also the comments and counts of favourites, etc. Also the exif data will be available, if the image has one. The tab with the search icon will have two tasks, the first one will be to search 23 (at least what is possible) and second task will be to show the last recent uploads or "Interesting photos" of 23. The last tab will be your profile, with your photos and albums, your buddy icon, your contacts displayed with buddy icons, etc. 

If these features are implemented, I will start to implement the upload feature. This will include uploading photos from your smartphone directly to 23, no matter if they are in the gallery or directly from your smartphone camera.

## Update (June 26th, 2016)

The implementation of the explore tab has started. Right now it's only possible to see the most recent public uploads to 23. At the moment it looks like the stream, just with other photos, but there will be some changes in the near future, e.g. searching 23 like on the website, browsing tags, see new/other photogroups etc. 

## Update (June 14th, 2016)

It's possible now to view all comments for a photo with clicking on the comment icon. Also adding a comment is nearly finished which is also possible from the comments dialog. The fullscreen photo is now zoomable like in any other photo/image application.

<img src="https://raw.githubusercontent.com/isenmann/23viewer/master/Screenshots/commentsDialog.png" width="240">

## Update (June 7th, 2016)

It's now possible to click on the star to mark a photo as favourite or to remove it from you favourite list. Next step will be to add a comment to a photo and to see all the other comments.

## Update (June 5th, 2016)

The next three small design steps and features are ready. You can now see when the photo was uploaded and you can see the number of comments and number of favourites besides the icons. If the star is filled with green then you have marked this image as favourite otherwise not. Also the photos are now displayed correctly which mean, that they are scaled to full width of the cards. 

<img src="https://raw.githubusercontent.com/isenmann/23viewer/master/Screenshots/LatestScreenshot.png" width="240">
<img src="https://raw.githubusercontent.com/isenmann/23viewer/master/Screenshots/LatestScreenshot_2.png" width="240">
