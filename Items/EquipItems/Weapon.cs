using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public class Weapon : EquipItem
    {
        public Weapon(uint damage, uint durability, string name) : base(durability, name) => Damage = damage;

        public uint Damage { get; }

        public override EquipSlot Slot => EquipSlot.Weapon;
        
        public bool HandleDurabilityDepletion()
        {
            if (Durability <= 0)
            {
                Console.WriteLine($"Weapon {Name} has been destroyed");
                return false;
            }
            Durability--;
            Console.WriteLine($"The weapon’s strength is {Durability}");
            return true;
        }

        
    }
}
