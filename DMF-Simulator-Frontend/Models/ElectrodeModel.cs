using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class ElectrodeModel : ElementModel
    {
        public int ElectrodeID { get; set; }
        public int DriverID { get; set; }
        public int Shape { get; set; }
        public int Status { get; set; }
        public List<List<int>> Corners { get; set; }
    }
}
