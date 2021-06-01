using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public record SensorModel : ElementModel
    {
        public int SensorID { get; set; }
        public string Type { get; set; }
        public int ValueRed { get; set; }
        public int ValueGreen { get; set; }
        public int ValueBlue { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Sensor ID", SensorID.ToString());
            info.TryAdd("Type", Type);
            info.TryAdd("Value Red", ValueRed.ToString());
            info.TryAdd("Value Green", ValueGreen.ToString());
            info.TryAdd("Value Blue", ValueBlue.ToString());
            return info;
        }

        public override void ApplyElementChanges(ElementModel newElement)
        {
            base.ApplyElementChanges(newElement);
            ValueRed = ((SensorModel)newElement).ValueRed;
            ValueGreen = ((SensorModel)newElement).ValueGreen;
            ValueBlue = ((SensorModel)newElement).ValueBlue;
        }
    }
}
