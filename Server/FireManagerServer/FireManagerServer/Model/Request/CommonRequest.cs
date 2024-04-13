using FireManagerServer.Common;
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
        public string Desc { get; set; }
        public bool isActive { get; set; }
        public string NameCompare { get; set; }
        public string Threshold { get; set; }
        public string TopicWrite { get; set; }
        public string Status { get; set; }
        public string Port { get; set; }
        public string ModuleId { get; set; }
    }
}
