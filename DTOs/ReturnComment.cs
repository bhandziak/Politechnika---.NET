namespace CarWorkshopProjekt.DTOs
{
    public class ReturnComment
    {
        public Guid CommentId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime TimestampComment { get; set; }

    }
}
