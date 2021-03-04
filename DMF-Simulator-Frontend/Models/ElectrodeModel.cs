using System;

namespace DMF_Simulator_Frontend.Models
{
    public class ElectrodeModel
    {
        public int DistanceFromBottom { get; private set; } = new Random().Next(0, 400);
        public int DistanceFromLeft { get; private set; } = new Random().Next(0, 400);
    }
}
