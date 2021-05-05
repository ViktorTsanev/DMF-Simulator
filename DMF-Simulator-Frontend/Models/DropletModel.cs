using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class DropletModel : ElementModel
    {
        public string Substance_Name { get; set; }
        public string Color { get; set; }
        public float Temperature { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Substance", Substance_Name);
            info.TryAdd("Color", Color);
            info.TryAdd("Temperature", Temperature.ToString());
            return info;
        }
    }
}
