using System.ComponentModel.DataAnnotations;

namespace PCStoreApi.Domain.Entities
{
    public class UserInfo
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Address { get; set; }

        public PCBuild? PCBuild { get; set; }   
    }
}
