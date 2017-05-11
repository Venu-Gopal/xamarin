using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using CMSapp.PushNotification;
using CMSapp.Data;

namespace CMSApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            DataHandler.saveFirebaseToken(refreshedToken);
        }
    }
}