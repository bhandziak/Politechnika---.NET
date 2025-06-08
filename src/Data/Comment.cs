namespace CarWorkshopProjekt.Data
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime TimestampComment { get; set; }

        // nawigacja do ServiceOrder
        public ServiceOrder ServiceOrder { get; set; }
        // nawigacja do użytkownika, który dodał komentarz
        public User User { get; set; }  

    }
}
