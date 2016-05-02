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
using TwentyThreeNet;

namespace viewer
{
    [Activity(Label = "Authorization")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            Button button = FindViewById<Button>(Resource.Id.buttonAuthorize);
            button.Click += AuthorizeClicked;
        }
        
        protected override void OnResume()
        {
            base.OnResume();

            var prefs = Application.Context.GetSharedPreferences("23viewer.auth", FileCreationMode.Private);

            if (prefs != null)
            {
                var apiKey = prefs.GetString("KEY", null);
                var apiShare = prefs.GetString("SHARE", null);
                var frob = prefs.GetString("FROB", null);
                var apiToken = prefs.GetString("TOKEN", null);

                if (apiKey != null && apiShare != null && frob != null && apiToken == null)
                {
                    if (MainActivity.twentyThree == null) { MainActivity.twentyThree = new TwentyThreeNet.TwentyThree(apiKey, apiShare); }

                    try
                    {
                        Auth auth = MainActivity.twentyThree.AuthGetToken(frob);
                        apiToken = auth.Token;
                    }
                    catch (TwentyThreeNet.TwentyThreeApiException)
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, Resource.String.auth_warning, ToastLength.Short).Show();
                        });

                        //maybe have to delete the apiKey, apiShare and frob. Not sure at the moment
                    }

                    var editor = prefs.Edit();
                    editor.PutString("TOKEN", apiToken);
                    editor.Commit();
                }

                if (apiKey == null || apiShare == null || apiToken == null || frob == null)
                {
                    return;
                }

                if (MainActivity.twentyThree == null)
                {
                    MainActivity.twentyThree = new TwentyThreeNet.TwentyThree(apiKey, apiShare, apiToken);
                }
                else
                {
                    MainActivity.twentyThree.AuthToken = apiToken;
                }

                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
        }

        private void AuthorizeClicked(object sender, EventArgs e)
        {
            var prefs = Application.Context.GetSharedPreferences("23viewer.auth", FileCreationMode.Private);
            if (prefs != null)
            {
                var apiKey = prefs.GetString("KEY", null);
                var apiShare = prefs.GetString("SHARE", null);
                var apiToken = prefs.GetString("TOKEN", null);

                if (apiKey == null || apiShare == null || apiToken == null)
                {
                    string key = "23viewerAPIKey";
                    string share = Guid.NewGuid().ToString().ToLowerInvariant().Replace("-", "");

                    MainActivity.twentyThree = new TwentyThreeNet.TwentyThree(key, share);
                    string frob = MainActivity.twentyThree.AuthGetFrob();
                    string url = MainActivity.twentyThree.AuthCalcUrl(frob, AuthLevel.Delete);

                    var editor = prefs.Edit();
                    editor.PutString("KEY", key);
                    editor.PutString("SHARE", share);
                    editor.PutString("FROB", frob);
                    editor.Commit();

                    var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                    StartActivity(intent);
                }
                else
                {
                    if (apiKey != null && apiShare != null && apiToken != null)
                    {
                        if (MainActivity.twentyThree == null) { MainActivity.twentyThree = new TwentyThreeNet.TwentyThree(apiKey, apiShare, apiToken); }
                        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    }
                }
            }
        }
    }
}