using System.ComponentModel.DataAnnotations;

namespace PCStoreApi.Domain.Entities
{
    public class UserInfo
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public required string FullName { get; set; }
        [Required]
        public required string Address { get; set; }
        public PCBuild? PCBuild { get; set; }
        public User User { get; set; } = null!;
    }
}
