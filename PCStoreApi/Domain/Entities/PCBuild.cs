using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace PCStoreApi.Domain.Entities
{
    public class PCBuild
    {
        [Key]
        public Guid PCBuildId { get; set; }

        [Required]
        public required string Processor { get; set; }

        public int RamInGB { get; set; }

        public string? GraphicsCard { get; set; }

        [Required]
        public string? Storage { get; set; }

        public Guid UserId {  get; set; }

        public UserInfo? User { get; set; }
    }
}
