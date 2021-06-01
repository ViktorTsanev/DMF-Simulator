using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public abstract record ElementModel : BaseElementModel
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int TranslateX { get; set; }
        public int TranslateY { get; set; }
        public double ScaleX { get; set; } = 1;
        public double ScaleY { get; set; } = 1;
        public bool Visible { get; set; } = true;
        public static int AnimationSpeed { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();

            if (info.ContainsKey("PositionX"))
            {
                info["PositionX"] = (PositionX + TranslateX).ToString();
            }
            else
            {
                info.TryAdd("PositionX", (PositionX + TranslateX).ToString());
            }

            if (info.ContainsKey("PositionY"))
            {
                info["PositionY"] = (PositionY + TranslateY).ToString();
            }
            else
            {
                info.TryAdd("PositionY", (PositionY + TranslateY).ToString());
            }

            info.TryAdd("SizeX", (SizeX * ScaleX).ToString());
            info.TryAdd("SizeY", (SizeY * ScaleY).ToString());

            return info;
        }

        public virtual void ApplyElementChanges(ElementModel newElement)
        {
            TranslateX += newElement.PositionX - PositionX - TranslateX;
            TranslateY += newElement.PositionY - PositionY - TranslateY;
            ScaleX = (double)newElement.SizeX / SizeX;
            ScaleY = (double)newElement.SizeY / SizeY;
        }
    }
}
