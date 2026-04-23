using System.ComponentModel.DataAnnotations;

namespace PPL3_Banhangonline.Models
{
    public class RescueCampaign
    {
        [Key]
        public int CampaignID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nông sản")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại")]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lý do giải cứu")]
        public string Reason { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tối thiểu")]
        public int MinQuantity { get; set; }

        public DateTime? ExpectedHarvestDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Active"; // Active, Closed

        public int ShopID { get; set; }
        public virtual Shop? Shop { get; set; }

        public virtual ICollection<RescueRegistration>? Registrations { get; set; }
    }
}