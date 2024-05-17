using FireManagerServer.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class DeviceEntity: BaseDate
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public DeviceType Type { get; set; }
        public string ModuleId { get; set; }
        public Module Module { get; set; }
        public string Port { get; set; }
        public string Unit { get; set; }
        public List<TopicThreshhold>? TopicThreshholds { get; set; }
        public List<HistoryData>? HistoryDatas { get; set; }

    }
}
