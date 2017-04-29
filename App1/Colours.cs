using System;
using System.Reflection;
using System.Xml;

namespace App1
{
    class Colours
    {
        int red, green, blue;
        string correctname = "";

        float engine(int red2, int green2, int blue2)
        {
            float diffRed = Math.Abs(red - red2);
            float diffGreen = Math.Abs(green - green2);
            float diffBlue = Math.Abs(blue - blue2);

            float pctDiffRed = diffRed / 255;
            float pctDiffGreen = diffGreen / 255;
            float pctDiffBlue = diffBlue / 255;

            return (pctDiffRed + pctDiffGreen + pctDiffBlue) / 3 * 100;
        }

        void readFromXml()
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            System.IO.Stream _xmlStream = _assembly.GetManifestResourceStream("App1.Allcolours.xml");
            System.IO.StreamReader _textStreamReader = new System.IO.StreamReader(_xmlStream);
            string xml = _textStreamReader.ReadToEnd();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            float percent = 100;
            foreach (XmlNode node in xmlDoc.DocumentElement)
            {
                string name = node.InnerText;
                int r = Int32.Parse(node.Attributes[2].InnerText);
                int g = Int32.Parse(node.Attributes[3].InnerText);
                int b = Int32.Parse(node.Attributes[4].InnerText);

                float perfromEngine = engine(r, g, b); /////////////////////////////////////////////////// engine                    

                if (perfromEngine < percent)
                {
                    percent = perfromEngine;
                    correctname = name;
                }

            }
        }

        public string returnName(int r, int g, int b)
        {
            red = r;
            green = g;
            blue = b;
            readFromXml();
            return correctname;
        }

    }
}