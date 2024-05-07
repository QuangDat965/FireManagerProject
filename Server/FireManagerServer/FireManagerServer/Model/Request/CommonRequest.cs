using FireManagerServer.Common;
using FireManagerServer.Database.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Model.Request
{
    public class CommonRequest
    {
        public string? Id { get; set; }
        public string? UnitId { get; set; }
        public string? ModuleId { get; set; }
        public string? SearchKey { get; set; }
    }
    public class UnitFilter
    {
        public string? Id { get; set; }
        public string? SearchKey { get; set; }
        public OrderType? OrderBy { get; set; } = OrderType.ByName;
    }

    public class ApartmentUpdateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
    public class RuleAddDto
    {
        public List<TopicThreshhold> TopicThreshholds { get; set; }
        public string Desc { get; set; }
        public bool isActive { get; set; }
        public bool isFire { get; set; }
        public TypeRule TypeRule { get; set; }
    }
    public class TopicThreshHoldDto
    {
        public string DeviceId { get; set; }
        public string RuleId { get; set; }
        public int? ThreshHold { get; set; }
        public int? Value { get; set; }
        public TypeCompare TypeCompare { get; set; }
    }
}
