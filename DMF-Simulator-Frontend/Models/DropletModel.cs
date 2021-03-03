using System;

namespace DMF_Simulator_Frontend.Models
{
    public class DropletModel
    {
        public int DistanceFromBottom { get; private set; } = 450;
        public int DistanceFromLeft { get; private set; } = 220;
        public void MoveDown(int speed)
        {
            DistanceFromBottom -= speed;
        }
    }
}
