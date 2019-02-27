using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace XamarinAndroidNative
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText tv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            tv = FindViewById<EditText>(Resource.Id.et);

            Button btn = FindViewById<Button>(Resource.Id.button1);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this,NetStandartLibrary.Greeting.SayHi(tv.Text),ToastLength.Short).Show();          
        }
    }
}