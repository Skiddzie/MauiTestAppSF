// Add these using directives
using Android;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

public class MainActivity : MauiAppCompatActivity
{
    const int RequestBluetoothPermissionsId = 1;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Request Bluetooth permissions
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Bluetooth) != (int)Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothAdmin) != (int)Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothScan) != (int)Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) != (int)Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, new string[] {
                Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin,
                Manifest.Permission.BluetoothScan,
                Manifest.Permission.BluetoothConnect,
                Manifest.Permission.AccessFineLocation
            }, RequestBluetoothPermissionsId);
        }
    }
}
