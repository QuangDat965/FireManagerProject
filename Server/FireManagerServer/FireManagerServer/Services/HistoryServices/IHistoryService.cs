using FireManagerServer.Model.HistoryModel;

namespace FireManagerServer.Services.HistoryServices
{
    public interface IHistoryService
    {
        Task<List<HistoryDisplayDto>> GetAll();
        Task<List<HistoryDisplayDto>> GetByCondition();
        Task<HistoryDisplayDto> Create(HistoryCreateDto historyCreateDto);
    }
}
