using System.ComponentModel.DataAnnotations;

namespace MagicvVilla_VilaAPI.Models.Dto
{
    public class VillaNumberUpdateDTO
    {
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }

    }
}
 