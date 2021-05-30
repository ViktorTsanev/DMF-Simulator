using System.Collections.Generic;

namespace DMF_Simulator_Frontend.Models
{
    public record SimulatorData
    {
        public BoardModel InitialBoard { get; set; }
        public List<BoardModel> BoardStates { get; set; }
        public List<int> AnimationTimePoints { get; set; }
    }
}
