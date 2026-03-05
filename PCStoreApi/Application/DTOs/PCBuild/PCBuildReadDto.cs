namespace PCStoreApi.Application.DTOs.PCBuild
{
    public class PCBuildReadDto
    {
        public int PCBuildId { get; set; }
        public string Processor { get; set; } = null!;
        public int RamInGB { get; set; }
        public string? GraphicsCard { get; set; }
        public string Storage { get; set; } = null!;
        public int UserID {  get; set; }
    }
}
