using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public abstract record BaseElementModel
    {
        public string Name { get; set; }
        public int ID { get; init; }
        public int PositionX { get; init; }
        public int PositionY { get; init; }

        public virtual Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = new();
            info.TryAdd("Name", Name);
            info.TryAdd("ID", ID.ToString());
            info.TryAdd("PositionX", PositionX.ToString());
            info.TryAdd("PositionY", PositionY.ToString());
            return info;
        }

        public virtual void ApplyElementChanges(BaseElementModel newElement)
        {
            Name = newElement.Name;
        }
    }
}
