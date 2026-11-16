using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;

namespace GamePrototype.Utils
{
    public static class DungeonBuilder
    {
        public static DungeonRoom BuildDungeon()
        {
            // ===== Комнаты =====
            var enter = new DungeonRoom("Enter");                                                    // пустая (старт)
            var emptyRoom = new DungeonRoom("Empty Hall");                                               // пустая
            var monsterRoom1 = new DungeonRoom("Goblin Lair", UnitFactoryDemo.CreateGoblinEnemy());      // монстр
            var monsterRoom2 = new DungeonRoom("Goblin Ambush", UnitFactoryDemo.CreateGoblinEnemy());      // монстр
            var lootGoldRoom = new DungeonRoom("Treasure Room", new Grindstone("Stone"));                              // лут
            var lootWeaponRoom = new DungeonRoom("Armory", new Weapon(10, 15, "Sword"));             // лут
            var battleLootRoom = new DungeonRoom("Battle & Reward", UnitFactoryDemo.CreateGoblinEnemy(),
                                                                       new Weapon(5, 100, "Axe"));               // монстр + лут
            var potionRoom = new DungeonRoom("Alchemy Lab", new HealthPotion("Health Potion"));       // лут
            var stoneRoom = new DungeonRoom("Boss Room", UnitFactoryDemo.CreateGoblinEnemy());                 // монстр
            var finalRoom = new DungeonRoom("Final Chamber", new Gold(100));                                            // финал

            // ===== Связи =====
            enter.TrySetDirection(Direction.Left, emptyRoom);
            enter.TrySetDirection(Direction.Right, monsterRoom1);

            emptyRoom.TrySetDirection(Direction.Forward, lootGoldRoom);
            emptyRoom.TrySetDirection(Direction.Right, potionRoom);

            monsterRoom1.TrySetDirection(Direction.Forward, lootWeaponRoom);
            monsterRoom1.TrySetDirection(Direction.Right, stoneRoom);

            lootGoldRoom.TrySetDirection(Direction.Forward, monsterRoom2);
            potionRoom.TrySetDirection(Direction.Forward, battleLootRoom);

            lootWeaponRoom.TrySetDirection(Direction.Left, battleLootRoom);
            stoneRoom.TrySetDirection(Direction.Forward, monsterRoom2);

            monsterRoom2.TrySetDirection(Direction.Forward, finalRoom);
            battleLootRoom.TrySetDirection(Direction.Forward, finalRoom);

            return enter;
        }
    }
}