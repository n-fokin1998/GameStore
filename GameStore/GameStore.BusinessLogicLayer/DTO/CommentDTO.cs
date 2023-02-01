namespace GameStore.BusinessLogicLayer.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public int GameId { get; set; }

        public int? ParentCommentId { get; set; }

        public int? QuoteCommentId { get; set; }

        public bool IsDeleted { get; set; }
    }
}