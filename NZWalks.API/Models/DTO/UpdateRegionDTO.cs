using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Has to be minimum of 3 chars")]
        [MaxLength(3, ErrorMessage = "Has to be max of 3 chars")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Has to be max of 100 chars")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
