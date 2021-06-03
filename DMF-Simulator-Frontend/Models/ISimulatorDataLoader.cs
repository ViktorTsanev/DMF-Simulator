using MatBlazor;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    interface ISimulatorDataLoader
    {
        public async Task<SimulatorData> LoadSimulatorDataAsync(IMatFileUploadEntry[] files, string initialBoardFileName)
        {
            SimulatorData simulatorData = new();
            simulatorData.AnimationTimePoints = new();
            simulatorData.BoardStates = new();

            foreach (var file in files)
            {
                string fileContent = await ConvertFileToStringAsync(file);

                if (file.Name == initialBoardFileName)
                {
                    simulatorData.AnimationTimePoints.Add(0);
                    simulatorData.InitialBoard = DeserializeBoard(fileContent);
                }
                else
                {
                    string trimmedFileName = file.Name.Replace(".json", "");
                    int.TryParse(trimmedFileName, out int timePoint);
                    simulatorData.AnimationTimePoints.Add(timePoint);
                    simulatorData.BoardStates.Add(DeserializeBoard(fileContent));
                }
            }

            return simulatorData;
        }

        public static async Task<string> ConvertFileToStringAsync(IMatFileUploadEntry file)
        {
            using MemoryStream memoryStream = new();
            await file.WriteToStreamAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new(memoryStream);
            return await reader.ReadToEndAsync();
        }

        public static BoardModel DeserializeBoard(string boardContent)
        {
            return JsonConvert.DeserializeObject<BoardModel>(boardContent);
        }
    }
}
