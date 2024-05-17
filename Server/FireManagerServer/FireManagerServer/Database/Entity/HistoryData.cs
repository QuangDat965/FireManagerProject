namespace FireManagerServer.Database.Entity
{
    public class HistoryData
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string Value { get; set; }
        public bool? IsSuccess { get; set; }
        public DateTime DateRetrieve { get; set; }
        public string DeviceId { get; set; }
        public DeviceEntity DeviceEntity { get; set; }
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
