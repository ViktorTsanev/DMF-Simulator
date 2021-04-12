namespace DMF_Simulator_Frontend.Models
{
    public abstract class BaseElementModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
