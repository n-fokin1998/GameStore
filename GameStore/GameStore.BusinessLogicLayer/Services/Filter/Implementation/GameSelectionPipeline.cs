using GameStore.BusinessLogicLayer.DTO;
using System.Collections.Generic;

namespace GameStore.BusinessLogicLayer.Services.Filter
{
    public class GameSelectionPipeline : Pipeline<IEnumerable<GameDTO>>
    {
        public override IEnumerable<GameDTO> Process(IEnumerable<GameDTO> input)
        {
            foreach (var filter in Filters)
            {
                input = filter.Execute(input);
            }
            return input;
        }
    }
}