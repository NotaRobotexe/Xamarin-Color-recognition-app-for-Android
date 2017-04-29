using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Graphics;
using Android.Util;
using System;
using Java.IO;

namespace App1
{
    [Activity(Label = "plsreffres1", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class MainActivity : Activity
    {
        touch tch;
        ImageView imageView;
        float pixel, lastMoveX = -1, lastMoveY;
        bool moveOrBorder = false, enableTouch = false;
        TextView tv6, tv7, tv8;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            var button = FindViewById<Button>(Resource.Id.button1);

            tv6 = FindViewById<TextView>(Resource.Id.textView6);
            tv8 = FindViewById<TextView>(Resource.Id.textView8);
            tv7 = FindViewById<TextView>(Resource.Id.textView7);

            CreateDirectoryForPictures(); ///na externom

            button.Click += Button_Click;

            var dp = 20;
            pixel = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Resources.DisplayMetrics); // convert 20dp to pixel;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("photo.png", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
            enableTouch = true;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            Bitmap workingBitmap = BitmapFactory.DecodeFile(App._file.Path);
            Bitmap mutableBitmap;

            int origWidth = workingBitmap.Width;
            int origHeight = workingBitmap.Height;

            int destWidth = imageView.Width;
            int destHeight = imageView.Height;

            if (origWidth > destWidth || origHeight > destHeight)
            {
                if (origWidth > 720)
                    origWidth = 720;
                if (origHeight > 1080)
                    origWidth = 1080;

                Bitmap b2 = Bitmap.CreateScaledBitmap(workingBitmap, destWidth, destHeight, false);
                ByteArrayOutputStream outStream = new ByteArrayOutputStream();
                // compress to the format you want, JPEG, PNG... 
                // 70 is the 0-100 quality percentage
                b2.Compress(Bitmap.CompressFormat.Jpeg, 70, System.IO.Stream.Null);
                workingBitmap = b2;


            }

            if (workingBitmap.Width > workingBitmap.Height) //rotate image if is langwise
            {
                Matrix matrix = new Matrix();
                matrix.PostRotate(90);
                mutableBitmap = Bitmap.CreateBitmap(workingBitmap, 0, 0, workingBitmap.Width, workingBitmap.Height, matrix, true);
            }

            else
            {
                mutableBitmap = workingBitmap.Copy(Bitmap.Config.Argb8888, true);
            }

            tch = new touch(mutableBitmap, imageView.Width, imageView.Height, mutableBitmap.Width, mutableBitmap.Height, pixel);
            GC.Collect();
            imageView.SetImageBitmap(tch.drawRec(imageView.Width / 2, imageView.Height / 2));
            tch.findColor();

            System.Console.WriteLine("*****************************************************************************" + workingBitmap.Width + " " + workingBitmap.Height);

            tv8.Text = tch.colors[0];
            tv7.Text = tch.colors[1];
            tv6.Text = tch.colors[2];
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (enableTouch == true)
            {
                tch.clean();
                var touchX = e.GetX();
                var touchY = e.GetY();

                if (lastMoveX == -1)
                {
                    lastMoveX = imageView.Width / 2;
                    lastMoveY = imageView.Height / 2;
                    tv8.Text = tch.colors[0];
                    tv7.Text = tch.colors[1];
                }

                if (touchX < Resources.DisplayMetrics.WidthPixels - pixel && touchX > pixel && touchY < pixel + imageView.Height && touchY > pixel)
                {
                    switch (e.Action)
                    {
                        case MotionEventActions.Down:
                            moveOrBorder = tch.moveOrChangeBorder(touchX, touchY);

                            break;
                        case MotionEventActions.Move:
                            if (tch.realX > pixel + pixel / 3 && tch.realX < imageView.Width - tch.w / tch.pixTOimgX + pixel - pixel / 3 && tch.realY > pixel + pixel / 3 && tch.realY < imageView.Height - tch.h / tch.pixTOimgY + pixel - pixel / 3)
                            {
                                if (moveOrBorder == true)
                                {
                                    tch.changingBorder(touchX, touchY, lastMoveX, lastMoveY);
                                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                                }

                                else
                                {
                                    lastMoveX = touchX;
                                    lastMoveY = touchY;

                                    imageView.SetImageBitmap(tch.drawRec(touchX, touchY));
                                }
                            }

                            else if (tch.realX <= pixel + pixel / 3 && lastMoveX < touchX)
                            { //x left
                                if (moveOrBorder == true)
                                {
                                    tch.changingBorder(touchX, touchY, lastMoveX, lastMoveY);
                                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                                }

                                else
                                {
                                    lastMoveX = touchX;
                                    lastMoveY = touchY;

                                    imageView.SetImageBitmap(tch.drawRec(touchX, touchY));
                                }
                            }

                            else if (tch.realX >= imageView.Width - tch.w / tch.pixTOimgX + pixel - pixel / 3 && lastMoveX > touchX) //x right
                            {
                                if (moveOrBorder == true)
                                {
                                    tch.changingBorder(touchX, touchY, lastMoveX, lastMoveY);
                                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                                }

                                else
                                {
                                    lastMoveX = touchX;
                                    lastMoveY = touchY;

                                    imageView.SetImageBitmap(tch.drawRec(touchX, touchY));
                                }
                            }

                            else if (tch.realY <= pixel + pixel / 3 && lastMoveY < touchY)
                            {
                                if (moveOrBorder == true)
                                {
                                    tch.changingBorder(touchX, touchY, lastMoveX, lastMoveY);
                                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                                }

                                else
                                {
                                    lastMoveX = touchX;
                                    lastMoveY = touchY;

                                    imageView.SetImageBitmap(tch.drawRec(touchX, touchY));
                                }
                            }

                            else if (tch.realY >= imageView.Height - tch.h / tch.pixTOimgY + pixel - pixel / 3 && lastMoveY > touchY)
                            {
                                if (moveOrBorder == true)
                                {
                                    tch.changingBorder(touchX, touchY, lastMoveX, lastMoveY);
                                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                                }

                                else
                                {
                                    lastMoveX = touchX;
                                    lastMoveY = touchY;

                                    imageView.SetImageBitmap(tch.drawRec(touchX, touchY));
                                }
                            }

                            break;

                        case MotionEventActions.Up:

                            tch.findColor();
                            imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                            tv8.Text = tch.colors[0];
                            tv7.Text = tch.colors[1];
                            tv6.Text = tch.colors[2];
                            break;
                    }
                }
                else
                {
                    imageView.SetImageBitmap(tch.drawRec(lastMoveX, lastMoveY));
                }
            }
            GC.Collect();
            return true;
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "colorDetector");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }
    }

    public static class App
    {
        public static File _file;
        public static File _dir;
    }
}
