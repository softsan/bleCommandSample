using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace bleCommands.Droid
{
    [Activity(Label = "bleCommands", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(
        new[] { "android.intent.action.VOICE_COMMAND",
                "android.bluetooth.headset.profile.action.CONNECTION_STATE_CHANGED",
                "android.bluetooth.headset.action.VENDOR_SPECIFIC_HEADSET_EVENT",
                "android.bluetooth.adapter.action.STATE_CHANGED",

        },
        Categories = new[] { "android.intent.category.DEFAULT", "android.intent.category.BROWSABLE" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterReceiver();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RegisterReceiver()
        {
            IntentFilter filter = new IntentFilter(Intent.ActionMediaButton);//"android.intent.action.MEDIA_BUTTON"
            var receiver = new BleIntercepterReceiver();

            filter.Priority = 10000; //this line sets receiver priority
            RegisterReceiver(receiver, filter);
        }
    }


    public class BleIntercepterReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var intentAction = intent.Action;
            if(!string.IsNullOrWhiteSpace(intentAction))
                Toast.MakeText(context, intentAction, ToastLength.Long);


            var evt= (KeyEvent) intent.GetParcelableExtra(Intent.ExtraKeyEvent);
            if (evt == null)
                return;

            var action = evt.Action;
            if (action == KeyEventActions.Down)
                Toast.MakeText(context, "Down Button pressed", ToastLength.Long);

            if (action == KeyEventActions.Up)
                Toast.MakeText(context, "Up Button pressed", ToastLength.Long);

            


        }
    }
}
