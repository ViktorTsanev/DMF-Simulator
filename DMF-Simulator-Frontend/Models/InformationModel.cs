namespace DMF_Simulator_Frontend.Models
{
    public record InformationModel
    {
        public string PlatformName { get; set; }
        public string PlatformType { get; init; }
        public string PlatformID { get; init; }
        public int SizeX { get; init; }
        public int SizeY { get; init; }
    }
}
