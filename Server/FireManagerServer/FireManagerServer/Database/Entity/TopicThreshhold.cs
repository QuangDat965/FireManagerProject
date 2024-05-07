using FireManagerServer.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class TopicThreshhold
    {
        public string? DeviceId { get; set; }
        public DeviceEntity? Device { get; set; }
        public string? RuleId { get; set; }
        public RuleEntity? Rule { get; set; }
        public int? ThreshHold { get; set; }
        public int? Value { get; set; }
        public TypeCompare TypeCompare { get; set; }
    }
}
