namespace PCStoreApi.Application.DTOs.User
{
    public class UserUpdateDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;          
        public string Address { get; set; } = null!; 
    }
}
