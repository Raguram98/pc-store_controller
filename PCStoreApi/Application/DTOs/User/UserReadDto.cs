namespace PCStoreApi.Application.DTOs.User
{
    public class UserReadDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
