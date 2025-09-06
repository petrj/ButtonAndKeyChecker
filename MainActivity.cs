using Android.Views;

namespace ButtonAndKeyChecker
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    //[IntentFilter(
    //    new[] { Android.Content.Intent.ActionMain },
    //    Categories = new[] { Android.Content.Intent.CategoryLeanbackLauncher }
    //)]
    public class MainActivity : Activity
    {
        private DateTime _lastKeyPressed = DateTime.MinValue;
        private DateTime _lastMotionEventTime = DateTime.MinValue;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
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

                    if (
                        ((DateTime.Now - _lastKeyPressed).TotalSeconds > 3)
                        &&
                         ((DateTime.Now - _lastMotionEventTime).TotalSeconds > 2)
                      )
                    {
                        RunOnUiThread(() =>
                        {
                            SetTitleVisibility(ViewStates.Visible);
                        });
                    }

                    Thread.Sleep(250);

                } while (true);
            }).Start();

        }

        public void SetKeyText(string text, float size = 40)
        {
            var keyTextView = FindViewById<TextView>(Resource.Id.keyTextView);
            if (keyTextView != null)
            {
                keyTextView.Text = text;
                keyTextView.SetTextSize(Android.Util.ComplexUnitType.Dip, size);
            }
        }

        private void SetTitleVisibility(ViewStates visibility)
        {
            var titleTextView = FindViewById<TextView>(Resource.Id.titleTextView);
            if (titleTextView != null)
            {
                titleTextView.Visibility = visibility;
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
            SetTitleVisibility(ViewStates.Invisible);
            _lastKeyPressed = DateTime.Now;

            return true;
        }

        public override bool OnGenericMotionEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            SetTitleVisibility(ViewStates.Invisible);
            _lastMotionEventTime = DateTime.Now;

            return base.OnGenericMotionEvent(e);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            SetTitleVisibility(ViewStates.Invisible);
            _lastMotionEventTime = DateTime.Now;

            return base.OnTouchEvent(e);
        }

        public override bool OnTrackballEvent(MotionEvent e)
        {
            SetMotionEventText($"{e.Action.ToString()}");
            SetTitleVisibility(ViewStates.Invisible);
            _lastMotionEventTime = DateTime.Now;

            return base.OnTrackballEvent(e);
        }


    }
}