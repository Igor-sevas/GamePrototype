using GamePrototype.Items.EconomicItems;

namespace GamePrototype.Units
{
    public sealed class Inventory
    {
        private readonly uint _capacity;
        private readonly List<Item> _items = new List<Item>();
        public IReadOnlyList<Item> Items => _items;
       

        public Inventory(uint capacity) => _capacity = capacity;

        public bool TryAdd(Item item) 
        {
            if (_items.Count == _capacity) 
            {
                return false;
            }
            
            _items.Add(item);
            return true;
        }

        public bool TryRemove(Item item) 
        {
            if ( _items.Count == 0 || !_items.Contains(item)) 
            {
                return false;
            }
            _items.Remove(item);
            return true;
        }

        public void ShowInventory()
        {
            Console.WriteLine("=== ИНВЕНТАРЬ ===");
            if (_items.Count == 0)   
            {
                Console.WriteLine("(пусто)");
            }
            else
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    var item = _items[i];
                    string amount = item.Stackable ? $" x{item.Amount}" : "";
                    Console.WriteLine($"[{i + 1}] {item.Name}{amount}");
                }
            }
            Console.WriteLine($"Занято: {_items.Count}/{_capacity}");
        }

    }
}
