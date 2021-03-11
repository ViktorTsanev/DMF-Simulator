using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public class ActuatorModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int ActuatorID { get; set; }
        public string Type { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int ActualTemperature { get; set; }
        public int DesiredTemperature { get; set; }
        public bool Status { get; set; }
        public int NextDesiredTemperature { get; set; }
        public bool NextStatus { get; set; }
        public List<List<int>> Corners { get; set; }
    }
}