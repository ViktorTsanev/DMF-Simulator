using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public List<BoardModel> BoardStates { get; private set; }
        public event EventHandler MainLoopCompleted;
        public bool IsStarted { get; private set; }
        public bool IsPaused { get; private set; } = true;
        private int _startFromFile;
        public BoardModel InitialState { get; init; }

        public SimulatorManager(BoardModel boardModel, List<BoardModel> boardModelNew)
        {
            BoardModel = boardModel;
            InitialState = new();
            InitialState.Droplets = new();
            BoardModel.Droplets.ForEach(element => InitialState.Droplets.Add(element with { }));
            InitialState.Electrodes = new();
            BoardModel.Electrodes.ForEach(element => InitialState.Electrodes.Add(element with { }));

            foreach (var d in InitialState.Droplets)
            {
                Console.WriteLine(d);
            }
            BoardStates = boardModelNew;
        }

        /*public void MainLoop()
        {
            IsStarted = true;

            while (IsStarted)
            {
                StopSimulatorAsync();
            }
        }*/

        private static void IndividualChanges<T>(T oldElement, T newElement) where T : ElementModel
        {
            oldElement.TranslateX += newElement.PositionX - oldElement.PositionX - oldElement.TranslateX;
            Console.WriteLine("TrX {0} ID {1}", oldElement.TranslateX, oldElement.ID);
            oldElement.TranslateY += newElement.PositionY - oldElement.PositionY - oldElement.TranslateY;
            Console.WriteLine("TrY {0} ID {1}", oldElement.TranslateY, oldElement.ID);
            oldElement.ScaleX = (double)newElement.SizeX / oldElement.SizeX;
            Console.WriteLine("ScaleX {0} ID {1}", oldElement.ScaleX, oldElement.ID);
            oldElement.ScaleY = (double)newElement.SizeY / oldElement.SizeY;
            Console.WriteLine("ScaleY {0} ID {1}", oldElement.ScaleY, oldElement.ID);
        }

        private async Task ProcessChangesAsync()
        {
            //foreach (BoardModel b in BoardModelNew.GetRange(_startFromFile, BoardModelNew.Count - _startFromFile))
            foreach (BoardModel b in BoardStates.GetRange(_startFromFile, BoardStates.Count - _startFromFile))
            {
                if (IsStarted && !IsPaused)
                {
                    _startFromFile++;
                    BoardModel.Electrodes.ForEach(e =>
                    {
                        if (b.Electrodes != null)
                        {
                            ElectrodeModel newElectrode = b.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                            if (newElectrode != null)
                            {
                                IndividualChanges(e, newElectrode);
                                e.Status = newElectrode.Status;
                            }
                        }
                    });

                    BoardModel.Droplets.ForEach(d =>
                    {
                        if (b.Droplets != null)
                        {
                            // rename to changedDroplet
                            DropletModel newDroplet = b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                            if (newDroplet != null)
                            {
                                IndividualChanges(d, newDroplet);
                                d.Substance_Name = newDroplet.Substance_Name;
                                d.Temperature = newDroplet.Temperature;
                            }
                        }
                    });

                    // Add newly created droplets to the current list (board)
                    // TODO: check if (b.Droplets != null)
                    IEnumerable<DropletModel> newDroplets = b.Droplets.Where(p => !BoardModel.Droplets.Any(p2 => p2.ID == p.ID));
                    BoardModel.Droplets.AddRange(newDroplets);

                    MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                    await Task.Delay(1000);
                }
                else
                {
                    break;
                }
            }
        }

        public async Task StartSimulatorAsync()
        {
            if (!IsStarted || IsPaused)
            {
                IsStarted = true;
                IsPaused = false;
                await ProcessChangesAsync();
                //await StopSimulatorAsync();
            }

            //MainLoopCompleted?.Invoke(this, EventArgs.Empty);
            /*if (!IsRunning)
            {
                MainLoop();
            }*/
        }

        public void PauseSimulator()
        {
            IsPaused = true;
        }

        public async Task StopSimulatorAsync()
        {
            IsStarted = false;
            IsPaused = true;
            _startFromFile = 0;

            BoardModel.Droplets = new();
            InitialState.Droplets.ForEach(element => BoardModel.Droplets.Add(element with { }));
            BoardModel.Electrodes = new();
            InitialState.Electrodes.ForEach(element => BoardModel.Electrodes.Add(element with { }));

            MainLoopCompleted?.Invoke(this, EventArgs.Empty);
            await Task.Delay(1000);
        }
    }
}
