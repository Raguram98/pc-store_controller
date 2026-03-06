namespace PCStoreApi.Application.DTOs.Auth
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
