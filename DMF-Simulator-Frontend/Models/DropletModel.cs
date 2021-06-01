using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public record DropletModel : ElementModel
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

        public override void ApplyElementChanges(ElementModel newElement)
        {
            base.ApplyElementChanges(newElement);
            Substance_Name = ((DropletModel)newElement).Substance_Name;
            Temperature = ((DropletModel)newElement).Temperature;
        }
    }
}
