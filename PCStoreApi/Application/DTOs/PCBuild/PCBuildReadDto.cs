namespace PCStoreApi.Application.DTOs.PCBuild
{
    public class PCBuildReadDto
    {
        public Guid PCBuildId { get; set; }
        public string Processor { get; set; } = null!;
        public int RamInGB { get; set; }
        public string? GraphicsCard { get; set; }
        public string Storage { get; set; } = null!;
        public Guid UserId {  get; set; }
    }
}
