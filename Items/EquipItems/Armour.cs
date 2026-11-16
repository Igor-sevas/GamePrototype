using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public sealed class Armour : EquipItem
    {
        public Armour(uint defence, uint durability, string name) : base(durability, name) => Defence = defence;

        public uint Defence { get; private set; }

        public override EquipSlot Slot => EquipSlot.Armour;

        public void HandleDurabilityDepletion()
        {
            if (Durability == 0)
            {
                Console.WriteLine($"Armour {Name} has been destroyed");
                return;
            }

            Durability--;
            Console.WriteLine($"The armor’s strength is {Durability}");
        }
    }
}
