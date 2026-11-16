using GamePrototype.Items.EquipItems;
using GamePrototype.Units;
namespace GamePrototype.Items.EconomicItems

{
    public sealed class Grindstone : EconomicItem
    {
        public override bool Stackable => false;
        public uint WeaponRestore => 4;

        public override uint Amount { get; protected set; }

        public Grindstone(string name) : base(name)
        {
        }
        //реализовать метод добавляющий прочность оружию, если оно в инвентаре игрока 
        public void UseOnWeapon(Weapon weapon, Inventory inventory)
        {
            if (weapon == null)
                throw new ArgumentNullException(nameof(weapon));

            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            if (!inventory.Items.Contains(this))
                throw new InvalidOperationException("Grindstone is not in inventory.");

            if (!inventory.Items.Contains(weapon))
                throw new InvalidOperationException("Weapon is not in inventory.");

            weapon.Repair(WeaponRestore);
            Console.WriteLine($"Used {Name} on {weapon.Name}. Durability: {weapon.Durability}");
            inventory.TryRemove(this);
        }

        
    }
}
