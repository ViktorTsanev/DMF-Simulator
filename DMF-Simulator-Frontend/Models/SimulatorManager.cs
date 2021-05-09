using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    public class SimulatorManager
    {
        public BoardModel BoardModel { get; private set; }
        public BoardModel InitialState { get; init; }
        public List<BoardModel> BoardStates { get; private set; }
        public event EventHandler SimulatorStateChanged;
        public static bool IsStarted { get; private set; }
        public static bool IsPaused { get; private set; } = true;
        private static int _startSimFromState;

        public SimulatorManager(BoardModel boardModel, List<BoardModel> boardModelNew)
        {
            BoardModel = boardModel;

            InitialState = new();
            InitialState.Droplets = new();
            BoardModel.Droplets.ForEach(element => InitialState.Droplets.Add(element with { }));
            InitialState.Electrodes = new();
            BoardModel.Electrodes.ForEach(element => InitialState.Electrodes.Add(element with { }));

            BoardStates = boardModelNew;
        }

        private static void IndividualChanges<T>(T oldElement, T newElement) where T : ElementModel
        {
            oldElement.TranslateX += newElement.PositionX - oldElement.PositionX - oldElement.TranslateX;
            //Console.WriteLine("TrX {0} ID {1}", oldElement.TranslateX, oldElement.ID);
            oldElement.TranslateY += newElement.PositionY - oldElement.PositionY - oldElement.TranslateY;
            //Console.WriteLine("TrY {0} ID {1}", oldElement.TranslateY, oldElement.ID);
            oldElement.ScaleX = (double)newElement.SizeX / oldElement.SizeX;
            //Console.WriteLine("ScaleX {0} ID {1}", oldElement.ScaleX, oldElement.ID);
            oldElement.ScaleY = (double)newElement.SizeY / oldElement.SizeY;
            //Console.WriteLine("ScaleY {0} ID {1}", oldElement.ScaleY, oldElement.ID);
        }

        private async Task ProcessChangesAsync()
        {
            foreach (BoardModel b in BoardStates.GetRange(_startSimFromState, BoardStates.Count - _startSimFromState))
            {
                if (IsStarted && !IsPaused)
                {
                    _startSimFromState++;
                    BoardModel.Electrodes.ForEach(e =>
                    {
                        if (b.Electrodes != null)
                        {
                            ElectrodeModel changedElectrode = b.Electrodes.Where(t => t.ID == e.ID).FirstOrDefault();
                            if (changedElectrode != null)
                            {
                                IndividualChanges(e, changedElectrode);
                                e.Status = changedElectrode.Status;
                            }
                        }
                    });

                    BoardModel.Droplets.ForEach(d =>
                    {
                        if (b.Droplets != null)
                        {
                            DropletModel changedDroplet = b.Droplets.Where(t => t.ID == d.ID).FirstOrDefault();
                            if (changedDroplet != null)
                            {
                                IndividualChanges(d, changedDroplet);
                                d.Substance_Name = changedDroplet.Substance_Name;
                                d.Temperature = changedDroplet.Temperature;
                            }
                        }
                    });

                    // Add newly created droplets to the current list (board)
                    if (b.Droplets != null)
                    {
                        IEnumerable<DropletModel> newDroplets = b.Droplets.Where(p => !BoardModel.Droplets.Any(p2 => p2.ID == p.ID));
                        BoardModel.Droplets.AddRange(newDroplets);
                    }

                    SimulatorStateChanged?.Invoke(this, EventArgs.Empty);
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
                if (_startSimFromState < BoardStates.Count)
                {
                    await ProcessChangesAsync();
                }
                if (_startSimFromState >= BoardStates.Count)
                {
                    await StopSimulatorAsync();
                }
            }
        }

        public static void PauseSimulator()
        {
            IsPaused = true;
        }

        public async Task StopSimulatorAsync()
        {
            IsStarted = false;
            IsPaused = true;
            _startSimFromState = 0;

            BoardModel.Droplets = new();
            InitialState.Droplets.ForEach(element => BoardModel.Droplets.Add(element with { }));
            BoardModel.Electrodes = new();
            InitialState.Electrodes.ForEach(element => BoardModel.Electrodes.Add(element with { }));

            SimulatorStateChanged?.Invoke(this, EventArgs.Empty);
            await Task.Delay(1000);
        }
    }
}
