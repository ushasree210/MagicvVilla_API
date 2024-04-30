using MagicvVilla_VilaAPI.Models.Dto;

namespace MagicvVilla_VilaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList= new List<VillaDTO>
            {
                    new VillaDTO {Id=1,Name="PoolView",Occupancy=4,Sqft=100},
                    new VillaDTO {Id=2,Name="House View",Occupancy=3,Sqft=200}

            };
    }
}
