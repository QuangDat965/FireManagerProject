using FireManagerServer.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class DeviceEntity: BaseDate
    {
        [Key]
        public string Id { get; set; }
        public string Topic { get; set; }
        public DeviceType Type { get; set; }
        [ForeignKey("ModuleId")]
        public string ModuleId { get; set; }
        public Module Module { get; set; }
        public string Port { get; set; }

    }
}
