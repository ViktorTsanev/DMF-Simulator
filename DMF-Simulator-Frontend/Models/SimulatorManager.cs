using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public List<BoardModel> BoardModelNew { get; private set; }
        public event EventHandler MainLoopCompleted;
        public bool IsRunning { get; private set; }

        public SimulatorManager(BoardModel boardModel, List<BoardModel> boardModelNew)
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

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel b in BoardModelNew)
            {
                BoardModel.Droplets.ForEach(delegate (DropletModel d)
                {
                    d.TranslateX += b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault().PositionX - d.PositionX - d.TranslateX;
                    Console.WriteLine("TrX {0} ID {1}", d.TranslateX, d.ID);
                    d.TranslateY += b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault().PositionY - d.PositionY - d.TranslateY;
                    Console.WriteLine("TrY {0} ID {1}", d.TranslateY, d.ID);
                    d.ScaleX = (double)b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault().SizeX / d.SizeX;
                    Console.WriteLine("ScaleX {0} ID {1}", d.ScaleX, d.ID);
                    d.ScaleY = (double)b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault().SizeY / d.SizeY;
                    Console.WriteLine("ScaleY {0} ID {1}", d.ScaleY, d.ID);
                });
                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(1000);
            }
        }

        public async Task StartSimulatorAsync()
        {
            await ProcessChangesAsync();
            //MainLoopCompleted?.Invoke(this, EventArgs.Empty);
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
