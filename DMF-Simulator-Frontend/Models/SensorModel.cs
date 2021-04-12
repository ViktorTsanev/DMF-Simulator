namespace DMF_Simulator_Frontend.Models
{
    public class SensorModel : ElementModel
    {
        public int SensorID { get; set; }
        public string Type { get; set; }
        public int ValueRed { get; set; }
        public int ValueGreen { get; set; }
        public int ValueBlue { get; set; }
    }
}
