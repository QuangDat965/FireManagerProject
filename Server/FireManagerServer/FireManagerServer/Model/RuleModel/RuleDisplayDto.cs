using FireManagerServer.Common;
using FireManagerServer.Database.Entity;

namespace FireManagerServer.Model.RuleModel
{
    public class RuleDisplayDto
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public string ModuleId { get; set; }
        public List<TopicThreshholdDisplayDto>? TopicThreshholds { get; set; }
        public TypeRule TypeRule { get; set; }
        public bool isActive { get; set; }
        public bool isFireRule { get; set; }
    }
    public class TopicThreshholdDisplayDto
    {
        public string? DeviceId { get; set; }
        public string? RuleId { get; set; }
        public int? ThreshHold { get; set; }
        public int? Value { get; set; }
        public TypeCompare TypeCompare { get; set; }
    }
}
