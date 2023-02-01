namespace GameStore.Domain.Entities
{
    public class OrderDetails : EntityBase
    {
        public decimal Price { get; set; }

        public int GameId { get; set; }

        public virtual Order Order { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public bool IsDeleted { get; set; }
    }
}