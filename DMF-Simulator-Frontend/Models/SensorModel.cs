namespace DMF_Simulator_Frontend.Models
{
    public class SensorModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int SensorID { get; set; }
        public string Type { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int ValueRed { get; set; }
        public int ValueGreen { get; set; }
        public int ValueBlue { get; set; }
    }
}