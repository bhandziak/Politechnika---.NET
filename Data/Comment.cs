namespace CarWorkshopProjekt.Data
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int ServiceOrderId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime TimestampComment { get; set; }

        // nawigacja do ServiceOrder
        public ServiceOrder ServiceOrder { get; set; }
        // nawigacja do użytkownika, który dodał komentarz
        public User User { get; set; }  

    }
}
