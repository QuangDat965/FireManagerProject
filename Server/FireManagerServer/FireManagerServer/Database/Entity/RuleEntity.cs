using FireManagerServer.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class RuleEntity
    {
        [Key]
        public string Id { get; set; }
        public string Desc { get; set; }
        public string ModuleId { get; set; }
        public Module? Module { get; set; }
        public List<TopicThreshhold>? TopicThreshholds { get; set; }
        public TypeRule TypeRule { get; set; }
        public bool isActive { get; set; }
        public bool isFireRule { get; set; }
    }
}
