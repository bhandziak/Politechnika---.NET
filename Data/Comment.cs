namespace CarWorkshopProjekt.Data
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int ServiceOrderId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime TimestampComment { get; set; }

    }
}
