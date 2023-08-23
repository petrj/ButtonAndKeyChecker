using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ButtonAndKeyChecker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private DateTime _lastKeyPressed = DateTime.MinValue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                do
                {
                    Thread.Sleep(1000);

                    if (_lastKeyPressed == DateTime.MinValue ||
                    ((DateTime.Now - _lastKeyPressed).TotalSeconds > 5))
                    {
                        RunOnUiThread(() =>
                        {
                            SetText("Press a key or button", 20);
                        });
                    }

                } while (true);
            }).Start();
        }

        public void SetText(string text, float size=40)
        {
            var ketTexttView = FindViewById<TextView>(Resource.Id.keyTextView);
            ketTexttView.Text = text;
            ketTexttView.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            SetText(keyCode.ToString());
            _lastKeyPressed = DateTime.Now;

            //return base.OnKeyDown(keyCode, e);
            return true;
        }
    }
}

