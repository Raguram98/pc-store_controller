using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;

namespace PCStoreApi.Application.DTOs.PCBuild
{
    public class PCBuildUpdateDto
    {
        public string Processor { get; set; } = null!;
        public int RamInGB { get; set; }
        public string? GraphicsCard { get; set; }
        public string Storage { get; set; } = null!;
    }
}
