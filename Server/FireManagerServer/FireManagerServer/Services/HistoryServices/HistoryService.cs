using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.HistoryModel;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.HistoryServices
{
    public class HistoryService : IHistoryService
    {
        private readonly FireDbContext _dbContext;

        public HistoryService(FireDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<HistoryDisplayDto> Create(HistoryCreateDto historyCreateDto)
        {
            var entity = new HistoryData()
            {
                Id = Guid.NewGuid().ToString(),
                DeviceName = historyCreateDto.DeviceName,
                DateRetrieve = DateTime.Now,
                DeviceId = historyCreateDto.DeviceId,
                DeviceType = historyCreateDto.DeviceType,
                IsSuccess = historyCreateDto.IsSuccess,
                UserId = historyCreateDto.UserId,
                Value = historyCreateDto.Value,
            };
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
      
            return new HistoryDisplayDto() { Value = entity.Value, DateRetrieve = entity.DateRetrieve, DeviceId = entity.DeviceId,
                DeviceName = entity.DeviceName, DeviceType = entity.DeviceType,UserId =entity.UserId,IsSuccess = entity.IsSuccess} ;
        }

        public async Task<List<HistoryDisplayDto>> GetAll()
        {
            var entities = await _dbContext.HistoryDatas.OrderBy(x=>x.DateRetrieve).Take(100).ToListAsync();
            var result = new List<HistoryDisplayDto>();
            foreach(var entity in entities)
            {
                var data = new HistoryDisplayDto()
                {
                    Value = entity.Value,
                    DateRetrieve = entity.DateRetrieve,
                    DeviceId = entity.DeviceId,
                    DeviceName = entity.DeviceName,
                    DeviceType = entity.DeviceType,
                    UserId = entity.UserId,
                    IsSuccess = entity.IsSuccess
                };
                result.Add(data);
            }
            return result;
        }

        public Task<List<HistoryDisplayDto>> GetByCondition()
        {
            throw new NotImplementedException();
        }
    }
}
