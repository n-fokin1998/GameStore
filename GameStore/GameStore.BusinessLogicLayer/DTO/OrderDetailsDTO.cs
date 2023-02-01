namespace GameStore.BusinessLogicLayer.DTO
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int GameId { get; set; }

        public string GameName { get; set; }

        public int OrderId { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
    }
}