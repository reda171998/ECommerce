namespace Basket.Api.Entities
{
    public class BasketCart
    {
        public string  UserName { get; set; }

        public List<BasketCartItem> Items { get; set; }= new List<BasketCartItem>() { };

        public BasketCart()
        {
                
        }

        public BasketCart(string userName )
        {
            UserName = userName;
        }

        public decimal TotalPrice 
        {
            get
            {
                decimal totlaPrice = 0;
                foreach (var item in Items)
                {
                    totlaPrice += item.Price * item.Quantity;
                }
                return totlaPrice;
            }
        }
    }
}
