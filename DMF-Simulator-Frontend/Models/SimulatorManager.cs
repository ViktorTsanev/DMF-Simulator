using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public BoardModel BoardModelNew { get; private set; }
        public event EventHandler MainLoopCompleted;
        public bool IsRunning { get; private set; } = false;

        public SimulatorManager(BoardModel boardModel, BoardModel boardModelNew)
        {
            BoardModel = boardModel;
            BoardModelNew = boardModelNew;
        }

        public void MainLoop()
        {
            IsRunning = true;

            while (IsRunning)
            {
                EndSimulator();
            }
        }

        private void ProcessChanges()
        {
            //BoardModelNew.Droplets
            Random r = new Random();
            BoardModel.Droplets.FirstOrDefault().TranslateX = r.Next(-100, 100);
            BoardModel.Droplets.FirstOrDefault().TranslateY = r.Next(-100, 100);
            BoardModel.Droplets.LastOrDefault().TranslateX = r.Next(-100, 100);
            BoardModel.Droplets.LastOrDefault().TranslateY = r.Next(-100, 100);
        }

        public void StartSimulator()
        {
            ProcessChanges();
            MainLoopCompleted?.Invoke(this, EventArgs.Empty);
            /*if (!IsRunning)
            {
                MainLoop();
            }*/
        }

        public void EndSimulator()
        {
            IsRunning = false;
        }
    }
}
