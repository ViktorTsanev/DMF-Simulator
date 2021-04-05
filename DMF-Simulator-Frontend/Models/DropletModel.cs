namespace DMF_Simulator_Frontend.Models
{
    public class DropletModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public string Color { get; set; }
        public float Temperature { get; set; }
        public int TranslateX { get; set; }
        public int TranslateY { get; set; }
    }
}
