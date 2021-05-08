using MatBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DMF_Simulator_Frontend.Models
{
    interface ISimulatorDataLoader
    {
        public async Task<Tuple<BoardModel, List<BoardModel>>> LoadSimulatorDataAsync(IMatFileUploadEntry[] files, string initialBoardFileName)
        {
            BoardModel initialBoard = new();
            List<BoardModel> boardStates = new();

            foreach (var file in files)
            {
                string fileContent = await ConvertFileToStringAsync(file);

                if (file.Name == initialBoardFileName)
                {
                    initialBoard = DeserializeBoard(fileContent);
                }
                else
                {
                    boardStates.Add(DeserializeBoard(fileContent));
                }
            }

            return new Tuple<BoardModel, List<BoardModel>>(initialBoard, boardStates);
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
