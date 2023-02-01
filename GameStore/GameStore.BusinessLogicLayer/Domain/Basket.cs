using System.Collections.Generic;
using System.Linq;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.BusinessLogicLayer.Domain
{
    public class Basket
    {
        private readonly List<OrderDetailsDTO> _productCollection = new List<OrderDetailsDTO>();

        public void AddItem(GameDTO product)
        {
            var line = _productCollection.FirstOrDefault(o => o.GameId == product.Id);
            if (line != null)
            {
                line.Quantity++;
                line.Price += product.Price;
                return;
            }

            _productCollection.Add(new OrderDetailsDTO
            {
                Price = product.Price,
                GameId = product.Id,
                GameName = product.NameEn,
                Quantity = 1,
                Discount = 0
            });
        }

        public void RemoveItem(int id)
        {
            _productCollection.RemoveAll(p => p.GameId == id);
        }

        public void RemoveItems(List<int> items)
        {
            foreach (var i in items)
            {
                _productCollection.RemoveAll(p => p.GameId == i);
            }
        }

        public IEnumerable<OrderDetailsDTO> GetItems()
        {
            return _productCollection;
        }

        public void Clear()
        {
            _productCollection.Clear();
        }

        public decimal CalculateTotalValue()
        {
            return _productCollection.Sum(p => p.Price * p.Quantity);
        }
    }
}