namespace DMF_Simulator_Frontend.Models
{
    public abstract class ElementModel : BaseElementModel
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int TranslateX { get; set; }
        public int TranslateY { get; set; }
        public double ScaleX { get; set; } = 1;
        public double ScaleY { get; set; } = 1;
        public bool Visible { get; set; } = true;
    }
}
