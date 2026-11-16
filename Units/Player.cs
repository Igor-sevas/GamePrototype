using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Utils;
using System.Text;

namespace GamePrototype.Units
{
    public sealed class Player : Unit
    {
        private readonly Dictionary<EquipSlot, EquipItem> _equipment = new();

        public Player(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {            
        }

        public override uint GetUnitDamage()
        {
            if (_equipment.TryGetValue(EquipSlot.Weapon, out var item) && item is Weapon weapon) 
            {
                return BaseDamage + weapon.Damage;
            }
            return BaseDamage;
        }

        public override void HandleCombatComplete()
        {
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                if (items[i] is EconomicItem economicItem) 
                {
                    UseEconomicItem(economicItem);
                    Inventory.TryRemove(items[i]);
                }
            }
        }

        public override void AddItemToInventory(Item item)
        {
            if (item is EquipItem equipItem)
            {
                if (_equipment.ContainsKey(equipItem.Slot))
                {
                    Console.WriteLine($"Слот {equipItem.Slot} занят: {_equipment[equipItem.Slot].Name}");
                    Console.WriteLine($"Надеть {equipItem.Name}? (1 - да, 2 - в инвентарь)");

                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        TryEquip(equipItem);
                        return;
                    }
                }
                else
                {
                    TryEquip(equipItem);
                    return;
                }
            }

            base.AddItemToInventory(item);
        }

        private void UseEconomicItem(EconomicItem economicItem)
        {
            if (economicItem is HealthPotion potion)
                Health += potion.HealthRestore;
            else if (economicItem is Grindstone stone)
            {
               
                var weapon = Inventory.Items.OfType<Weapon>().FirstOrDefault();
                if (weapon != null)
                    stone.UseOnWeapon(weapon, Inventory);
            }
        }

        protected override uint CalculateAppliedDamage(uint damage)
        {
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour)
            {
              
                uint reduced = damage - (uint)(damage * (armour.Defence / 100f));

                
                armour.HandleDurabilityDepletion();

              
                if (armour.Durability == 0)
                {
                    _equipment.Remove(EquipSlot.Armour);
                    //Console.WriteLine($"Броня {armour.Name} разрушена!");
                }

                return reduced;
            }

            return damage;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Name);
            builder.AppendLine($"Health {Health}/{MaxHealth}");
            builder.AppendLine("Loot:");
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                builder.AppendLine($"[{items[i].Name}] : {items[i].Amount}");
            }
            return builder.ToString();
        }

        public bool TryEquip(EquipItem item)
        {
            if (item == null) return false;

            if (_equipment.TryGetValue(item.Slot, out var oldItem))
            {
               
                _equipment[item.Slot] = item;
                Console.WriteLine($"Снято: {oldItem.Name}, надето: {item.Name}");

               
                if (Inventory.TryAdd(oldItem))
                    Console.WriteLine($"{oldItem.Name} перемещено в инвентарь");
                else
                    Console.WriteLine($"Инвентарь заполнен — {oldItem.Name} потеряно");

                return true;
            }

           
            _equipment[item.Slot] = item;
            Console.WriteLine($"Надето: {item.Name}");
            return true;
        }

        public bool TryEquipFromInventory(int index)
        {
            var items = Inventory.Items;
            if (index < 0 || index >= items.Count)
            {
                Console.WriteLine("Неверный индекс");
                return false;
            }

            if (items[index] is EquipItem equipItem)
            {
                Inventory.TryRemove(equipItem);
                return TryEquip(equipItem);
            }

            Console.WriteLine("Это не экипировка");
            return false;
        }

        


    }
}
