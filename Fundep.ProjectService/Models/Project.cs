using System.Runtime.Serialization;

namespace Fundep.ProjectService.Models
{
    [DataContract]
    public class Project
    {
        [DataMember(IsRequired = true)]
        public string ProjectNumber { get; set; }

        [DataMember(IsRequired = true)]
        public string SubProjectNumber { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string CoordinatorId { get; set; }

        [DataMember(IsRequired = true)]

        public string CoordinatorName { get; set; }

        [DataMember(IsRequired = true)]
        public decimal Balance { get; set; }
    }
}
