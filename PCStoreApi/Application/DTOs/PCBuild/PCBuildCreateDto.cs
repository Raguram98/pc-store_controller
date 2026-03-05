namespace PCStoreApi.Application.DTOs.PCBuild
{
    public class PCBuildCreateDto
    {
        public string Processor { get; set; } = null!;
        public int RamInGB { get; set; }
        public string? GraphicsCard {  get; set; }
        public string Storage { get; set; } = null!;
        public int UserID { get; set; }
    }
}
