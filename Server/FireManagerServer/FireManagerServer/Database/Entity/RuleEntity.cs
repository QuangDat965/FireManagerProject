using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class RuleEntity
    {
        [Key]
        public string Id { get; set; }
        public string Desc { get; set; }
        public bool isActive { get; set; }
        public string NameCompare { get; set; }
        public string Threshold { get; set; }
        public string TopicWrite { get; set; }
        public string Status { get; set; }
        public string Port { get; set; }
        [ForeignKey("ModuleId")]
        public string ModuleId { get; set; }
        public Module?  Module { get; set; }
    }
}
