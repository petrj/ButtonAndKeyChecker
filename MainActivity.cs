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
        private DateTime _lastMotionEventTime = DateTime.MinValue;

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
                    if (_lastKeyPressed == DateTime.MinValue ||
                    ((DateTime.Now - _lastKeyPressed).TotalSeconds > 3))
                    {
                        RunOnUiThread(() =>
                        {
                            SetKeyText("", 20);
                        });
                    }

                    if (_lastMotionEventTime == DateTime.MinValue ||
                                        ((DateTime.Now - _lastMotionEventTime).TotalSeconds > 2))
                    {
                        RunOnUiThread(() =>
                        {
                            SetMotionEventText("");
                        });
                    }

                    Thread.Sleep(250);

                } while (true);
            }).Start();
        }

        public void SetKeyText(string text, float size=40)
        {
            var keyTextView = FindViewById<TextView>(Resource.Id.keyTextView);

            if (keyTextView != null)
            {
                keyTextView.Text = text;
                keyTextView.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
            }
        }

        public void SetMotionEventText(string text, float size = 40)
        {
            var motionEventTextView = FindViewById<TextView>(Resource.Id.motionEventTextView);
            if (motionEventTextView != null)
            {
                motionEventTextView.Text = text;
                motionEventTextView.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            SetKeyText(keyCode.ToString());
            _lastKeyPressed = DateTime.Now;

            //return base.OnKeyDown(keyCode, e);
            return true;
        }

        public override bool OnGenericMotionEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            _lastMotionEventTime = DateTime.Now;

            return base.OnGenericMotionEvent(e);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            _lastMotionEventTime = DateTime.Now;

            return base.OnTouchEvent(e);
        }

        public override bool OnTrackballEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            _lastMotionEventTime = DateTime.Now;

            return base.OnTrackballEvent(e);
        }
    }
}

