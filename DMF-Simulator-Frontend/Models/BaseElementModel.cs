using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public abstract class BaseElementModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public virtual Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = new();
            info.TryAdd("Name", Name);
            info.TryAdd("ID", ID.ToString());
            info.TryAdd("PositionX", PositionX.ToString());
            info.TryAdd("PositionY", PositionY.ToString());
            return info;
        }
    }
}
