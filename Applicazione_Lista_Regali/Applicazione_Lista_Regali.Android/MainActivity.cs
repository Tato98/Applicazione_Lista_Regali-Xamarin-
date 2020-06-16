using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Android;
using Java.Security;
using Android.Support.V4.Content;
using Xamarin.Essentials;
using System.Security.Permissions;
using System.Threading.Tasks;
using Permissions = Xamarin.Essentials.Permissions;
using Applicazione_Lista_Regali.Utilities;

namespace Applicazione_Lista_Regali.Droid
{
    [Activity(Label = "Applicazione_Lista_Regali", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {


            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.SetFlags("SwipeView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            _ = CheckAndRequestContactsPermission();
            LoadApplication(new App());

            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task<PermissionStatus> CheckAndRequestContactsPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.ContactsRead>();
                if (status == PermissionStatus.Denied) { DependencyService.Get<IMessage>().ShortAlert(" Accesso ai contatti negato "); }
            }

            return status;
        }
    }
}