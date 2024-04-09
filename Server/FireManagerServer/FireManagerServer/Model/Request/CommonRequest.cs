using FireManagerServer.Common;

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
}
