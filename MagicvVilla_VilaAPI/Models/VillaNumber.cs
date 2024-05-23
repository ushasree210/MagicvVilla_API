using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicvVilla_VilaAPI.Models
{
    public class VillaNumber
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string SpecialDetails {  get; set; }
        public DateTime CreateDate { get; set; }  
        public DateTime UpdatedDate{ get; set;}

    }
}
