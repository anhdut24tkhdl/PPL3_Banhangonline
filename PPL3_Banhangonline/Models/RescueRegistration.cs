using System.ComponentModel.DataAnnotations;

namespace PPL3_Banhangonline.Models
{
    public class RescueRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        public int CampaignID { get; set; }
        public virtual RescueCampaign? Campaign { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Confirmed
    }
}