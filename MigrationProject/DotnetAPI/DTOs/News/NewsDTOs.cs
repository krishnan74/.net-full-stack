namespace DotnetAPI.DTOs.News
{
    public class AddNewsDTO
    {
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
    }
    public class UpdateNewsDTO
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
    }
}
