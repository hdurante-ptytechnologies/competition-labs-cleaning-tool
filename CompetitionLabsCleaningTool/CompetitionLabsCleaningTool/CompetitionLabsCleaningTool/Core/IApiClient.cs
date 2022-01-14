using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompetitionLabsCleaningTool.Core
{
    public interface IApiClient
    {
        void SetApiKey(string apiKey);
        public Task<List<string>> Get(CompetiotionLabsSpace space, int limit, int skip);
        public Task<bool> Delete(CompetiotionLabsSpace space, string id);

    }
}
