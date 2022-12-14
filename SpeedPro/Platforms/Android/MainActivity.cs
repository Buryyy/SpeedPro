using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.View;
using Activity = Android.App.Activity;

namespace SpeedPro;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    public static Activity Activity { get; private set; }
    public static Context Context { get; private set; }
    private static readonly string[] BLE_PERMISSIONS = new string[]{
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
    };

    private static readonly string[] ANDROID_12_BLE_PERMISSIONS = new string[] {
            Manifest.Permission.BluetoothScan,
            Manifest.Permission.BluetoothConnect,
            Manifest.Permission.AccessFineLocation,
    };

    protected override void OnCreate(Bundle savedInstanceState)
    {
        Activity = this;
        Context = this;
        //RequestBlePermissions(this, 0);
        base.OnCreate(savedInstanceState);
        SetStatusBarStyle();
    }

    private void SetStatusBarStyle()
    {
        Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        Window.SetStatusBarColor(new Android.Graphics.Color(244, 237, 237));
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightNavigationBar;
        Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);


        if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
        {
            WindowCompat.GetInsetsController(Window, Window.DecorView).AppearanceLightStatusBars = true;
        }
    }

    public static bool IsBLEAccessGranted()
    {
        if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            foreach (var permission in ANDROID_12_BLE_PERMISSIONS)
            {
                if (AndroidX.Core.Content.ContextCompat.CheckSelfPermission(Context, permission) == Android.Content.PM.Permission.Denied)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var permission in BLE_PERMISSIONS)
            {
                if (AndroidX.Core.Content.ContextCompat.CheckSelfPermission(Context, permission) == Android.Content.PM.Permission.Denied)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static void RequestBlePermissions(Activity activity, int requestCode)
    {
        if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
            ActivityCompat.RequestPermissions(activity, ANDROID_12_BLE_PERMISSIONS, requestCode);
        else
            ActivityCompat.RequestPermissions(activity, BLE_PERMISSIONS, requestCode);
    }
}
