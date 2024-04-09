using FireManagerServer.Common;

namespace FireManagerServer.Model.Request
{
    public class ApartmentRequest
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? UserId { get; set; }
    }
    public class ApartmentFilter
    {
        public string? SearchKey { get; set; }
        public OrderType? OrderBy { get; set; } = OrderType.ByName;
    }
}
