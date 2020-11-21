using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AuctionCars.ViewModel
{
    public class CreateLotViewModel
    {
        [MaxLength(31, ErrorMessage = "NameLength")]
        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }

        [MaxLength(1023, ErrorMessage = "DescriptionLength")]
        public string Desc { get; set; }

        [Range(1900, 2019, ErrorMessage = "WrongYear")]
        [Required(ErrorMessage = "YearRequired")]
        public ushort Year { get; set; }

        [Required]
        public string Transmission { get; set; }

        [Required]
        public string Fuel { get; set; }

        [Required]
        public string Body { get; set; }

        [Required(ErrorMessage = "EngineVolumeRequired")]
        [RegularExpression(@"^(\d)+(,\d{1})?$", ErrorMessage = "WrongEngineVolume")]
        public string EngineVolume { get; set; }

        [Required]
        public string Drive { get; set; }

        [Required(ErrorMessage = "MilleageRequired")]
        public uint Milleage { get; set; }

        [Required(ErrorMessage = "PriceRequired")]
        [Range(0, 4294967295, ErrorMessage = "WrongPrice")]
        public uint Price { get; set; }

        [Required(ErrorMessage = "DurationRequired")]
        [Range(1, 30, ErrorMessage = "WrongDuration")]
        public uint Duration { get; set; }

        [Required(ErrorMessage = "ImageRequired")]
        public IFormFile Image { get; set; }
    }
}
