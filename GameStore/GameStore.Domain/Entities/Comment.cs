namespace GameStore.Domain.Entities
{
    public class Comment : EntityBase
    {
        public string Name { get; set; }

        public string Body { get; set; }

        public virtual Game Game { get; set; }

        public int? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public int? QuoteCommentId { get; set; }

        public bool IsDeleted { get; set; }
    }
}