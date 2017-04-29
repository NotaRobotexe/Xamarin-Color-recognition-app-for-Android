using System;
using Android.Graphics;

namespace App1
{
    class touch
    {
        private Bitmap sourceImg, workImg;
        private Canvas canvas;
        private Paint paint;
        private Color imgcol;
        Colours col = new Colours();
        private float imgVW, imgVH, imgW, imgH, pixel;
        public float w, h, pixTOimgX, pixTOimgY;
        public float x, y, realX, realY, offsetH, offsetW;
        private bool whitchSideChange;
        public string[] colors = new string[3];

        public touch(Bitmap img, float imgVw, float imgVh, float imgw, float imgh, float pix)
        {
            pixel = pix;
            w = pixel * 3;
            h = pixel * 3;
            imgH = imgh;
            imgW = imgw;
            imgVH = imgVh;
            imgVW = imgVw;
            sourceImg = img;
            Console.WriteLine(img.Width + " " + img.Height + " first image ");
            pixTOimgX = imgw / imgVw;
            pixTOimgY = imgh / imgVh;
        }

        public Bitmap drawRec(float touchX, float touchY)
        {
            try
            {

                workImg = Bitmap.CreateBitmap(sourceImg.Width, sourceImg.Height, Bitmap.Config.Argb8888);
                Console.WriteLine(workImg.Width + " " + workImg.Height + " workImg ");

                x = (touchX - pixel) * pixTOimgX;
                y = (touchY - pixel) * pixTOimgY;

                x = x - w / 2;
                y = y - h / 2;

                if (x < pixel / 3 * pixTOimgX)
                {
                    x = pixel / 3 * pixTOimgX;
                }
                else if (y < pixel / 3 * pixTOimgY)
                {
                    y = pixel / 3 * pixTOimgY;
                }
                else if (x > imgVW * pixTOimgX - w - pixel / 3 * pixTOimgX)
                {
                    x = imgVW * pixTOimgX - w - pixel / 3 * pixTOimgX;
                }
                else if (y > imgVH * pixTOimgY - h - pixel / 3 * pixTOimgY)
                {
                    y = imgVH * pixTOimgY - h - pixel / 3 * pixTOimgY;
                }

                realX = x / pixTOimgX + pixel;
                realY = y / pixTOimgY + pixel;

                paint = new Paint();
                paint.StrokeWidth = pixel / 3;
                paint.SetARGB(255, 255, 0, 0);

                canvas = new Canvas(workImg);
                canvas.DrawBitmap(sourceImg, 0, 0, null);
                canvas.DrawLine(x, y, x + w, y, paint);
                canvas.DrawLine(x, y, x, y + h, paint);
                canvas.DrawLine(x, y + h, x + w, y + h, paint);
                canvas.DrawLine(x + w, y, x + w, y + h, paint);
                Console.WriteLine(workImg.Width + " " + workImg.Height + " workImg2");
                return workImg;
            }
            catch
            {
                System.Console.WriteLine("11111111111111111111111111111111111111111111111111111111111");
                return workImg;
            }
        }

        public bool moveOrChangeBorder(float touchX, float touchY)  //toto nevysvetlim /// upravit velkost uchytu
        {
            try
            {
                if (touchX > (x / pixTOimgX + pixel) && touchX < (x / pixTOimgX + w / pixTOimgX + pixel) && touchY >= (y / pixTOimgY + pixel - pixel / 2) && touchY <= (y / pixTOimgY + pixel + pixel / 2))
                {
                    whitchSideChange = true;
                    return true;
                }

                else if (touchX > (x / pixTOimgX + pixel) && touchX < (x / pixTOimgX + w / pixTOimgX + pixel) && touchY >= (y / pixTOimgY + h / pixTOimgY + pixel - pixel / 2) && touchY <= (y / pixTOimgY + h / pixTOimgY + pixel + pixel / 2))
                {
                    whitchSideChange = true;
                    return true;
                }

                // should be width

                else if (touchY >= (y / pixTOimgY + pixel) && touchY <= (y / pixTOimgY + h / pixTOimgY + pixel) && touchX <= (x / pixTOimgX + pixel + pixel / 2) && touchX >= (x / pixTOimgX + pixel / 2))
                {
                    whitchSideChange = false;
                    return true;
                }

                else if (touchY >= (y / pixTOimgY + pixel) && touchY <= (y / pixTOimgY + h / pixTOimgY + pixel) && touchX <= (x / pixTOimgX + pixel + pixel / 2 + w / pixTOimgX) && touchX >= (x / pixTOimgX + pixel / 2 + w / pixTOimgX))
                {
                    whitchSideChange = false;
                    return true;
                }

            }

            catch
            {
                System.Console.WriteLine("66666666666666666666666666666666666666666666666666666666666666666");
            }
            return false;
        }

        public void changingBorder(float touchX, float touchY, float lastX, float lastY)
        {
            try
            {

                if (whitchSideChange == true)
                {
                    offsetH = (((lastY - pixel) - (touchY - pixel)) * pixTOimgY) * 2;
                    h = Math.Abs(offsetH);
                }

                else if (whitchSideChange == false)
                {
                    offsetW = (((lastX - pixel) - (touchX - pixel)) * pixTOimgX) * 2;
                    w = Math.Abs(offsetW);
                }
            }

            catch
            {

                Console.WriteLine("111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
            }

        }

        public void findColor()
        {
            try
            {

                long[] rgbcol = new long[3];
                int numOfPixel = 0;

                for (int i = (int)x; i < x + w; i++)
                {
                    for (int a = (int)y; a < y + h; a++)
                    {
                        numOfPixel++;
                        imgcol = new Color(sourceImg.GetPixel(i, a));
                        rgbcol[0] += imgcol.R;
                        rgbcol[1] += imgcol.G;
                        rgbcol[2] += imgcol.B;
                    }
                }

                colors[0] = rgbcol[0] / numOfPixel + " " + rgbcol[1] / numOfPixel + " " + rgbcol[2] / numOfPixel;
                colors[1] = "#" + (rgbcol[0] / numOfPixel).ToString("X") + (rgbcol[1] / numOfPixel).ToString("X") + (rgbcol[2] / numOfPixel).ToString("X");
                colors[2] = col.returnName((int)(rgbcol[0] / numOfPixel), (int)(rgbcol[1] / numOfPixel), (int)(rgbcol[2] / numOfPixel));
            }
            catch
            {
                Console.WriteLine("asdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
            }
        }

        public void clean()
        {
            try
            {

                if (workImg != null)
                {
                    workImg.Recycle();
                    workImg = null;
                }
            }
            catch
            {
                Console.WriteLine("88888888888888888888888888888888888888888888888888888888888888");
            }
        }
    }
}