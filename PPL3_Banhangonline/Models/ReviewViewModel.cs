using System.ComponentModel.DataAnnotations;

namespace PPL3_Banhangonline.Models.Viewmodels
{
    public class ReviewViewModel
    {
        public int ProductID { get; set; }
        public int OrderID { get; set; }

        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn số sao")]
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Bình luận tối đa 500 ký tự")]
        public string? Comment { get; set; }
    }
}