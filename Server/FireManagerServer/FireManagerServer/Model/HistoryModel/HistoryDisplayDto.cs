using FireManagerServer.Common;
using FireManagerServer.Database.Entity;

namespace FireManagerServer.Model.HistoryModel
{
    public class HistoryDisplayDto
    {
        public string DeviceName { get; set; }
        public string Value { get; set; }
        public bool? IsSuccess { get; set; }
        public DateTime DateRetrieve { get; set; } = DateTime.Now;
        public string DeviceId { get; set; }
        public string? UserId { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
