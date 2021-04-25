using System.Collections.Generic;
using System.Linq;

namespace DMF_Simulator_Frontend.Models
{
    public class ElectrodeModel : ElementModel
    {
        public int ElectrodeID { get; set; }
        public int DriverID { get; set; }
        public int Shape { get; set; }
        public int Status { get; set; }
        public List<List<int>> Corners { get; set; }

        public override Dictionary<string, string> GetElementInfo()
        {
            Dictionary<string, string> info = base.GetElementInfo();
            info.TryAdd("Electrode ID", ElectrodeID.ToString());
            info.TryAdd("Driver ID", DriverID.ToString());
            info.TryAdd("Shape", (Shape == 0 ? "Rectangle" : "Polygon"));
            info.TryAdd("Status", (Status == 0 ? "Off" : "On"));
            if (Corners != null)
            {
                string coords = "";
                foreach (var corner in Corners)
                {
                    int x = corner.First() + PositionX;
                    int y = corner.Last() + PositionY;
                    coords += "(" + x + ", " + y + ") ";
                }
                info.TryAdd("Corners", coords);
            }
            return info;
        }
    }
}
