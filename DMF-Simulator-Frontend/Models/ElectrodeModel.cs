using System;

namespace DMF_Simulator_Frontend.Models
{
    public class ElectrodeModel
    {
        public string name { get; set; }
        public int ID { get; set; }
        public int electrodeID { get; set; }
        public int driverID { get; set; }
        public int shape { get; set; }
        public int positionX { get; set; } //= new Random().Next(0, 400);
        public int positionY { get; set; } //= new Random().Next(0, 400);
        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public int status { get; set; }
        public string corners { get; set; }
    }
}
