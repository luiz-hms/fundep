using System.Runtime.Serialization;

namespace Fundep.ProjectService.Models
{
    [DataContract]
    public class Coordinator
    {
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
