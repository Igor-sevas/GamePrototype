using GamePrototype.Combat;
using GamePrototype.Dungeon;
using GamePrototype.Units;
using GamePrototype.Utils;

namespace GamePrototype.Game
{
    public sealed class GameLoop
    {
        private Unit _player;
        private DungeonRoom _dungeon;
        private readonly CombatManager _combatManager = new CombatManager();

        public void StartGame()
        {
            Initialize();
            Console.WriteLine("Entering the dungeon");
            StartGameLoop();
        }

        #region Game Loop

        private void Initialize()
        {
            Console.WriteLine("Welcome, player!");
            _dungeon = DungeonBuilder.BuildDungeon();
            Console.WriteLine("Enter your name");
            _player = UnitFactoryDemo.CreatePlayer(Console.ReadLine());
            Console.WriteLine($"Hello {_player.Name}");
        }

        private void StartGameLoop()
        {
            var currentRoom = _dungeon;

            while (currentRoom.IsFinal == false)
            {
                StartRoomEncounter(currentRoom, out var success);
                if (!success) { Console.WriteLine("Game over!"); return; }

                _player.ShowInventory();
                DisplayRouteOptions(currentRoom);

                while (true)                                          
                {
                    Console.WriteLine("Команды: -inventory, -inventory <слот>, -info, -quit");
                    string input = Console.ReadLine();                

                    if (HandleCommand(input))
                        continue;                                    

                    if (Enum.TryParse<Direction>(input, out var direction)
                        && currentRoom.Rooms.TryGetValue(direction, out var nextRoom))
                    {
                        currentRoom = nextRoom;
                        break;                                        
                    }

                    Console.WriteLine("Wrong direction!");
                    DisplayRouteOptions(currentRoom);
                }
            }

            Console.WriteLine($"Congratulations, {_player.Name}");
            Console.WriteLine("Result: ");
            Console.WriteLine(_player.ToString());

        }
        private void StartRoomEncounter(DungeonRoom currentRoom, out bool success)
        {
            success = true;
            if (currentRoom.Loot != null)
            {
                _player.AddItemToInventory(currentRoom.Loot);
            }
            if (currentRoom.Enemy != null)
            {
                if (_combatManager.StartCombat(_player, currentRoom.Enemy) == _player)
                {
                    _player.HandleCombatComplete();
                    LootEnemy(currentRoom.Enemy);
                }
                else
                {
                    success = false;
                }
            }

            void LootEnemy(Unit enemy)
            {
                _player.AddItemsFromUnitToInventory(enemy);
            }
        }

        private void DisplayRouteOptions(DungeonRoom currentRoom)
        {
            Console.WriteLine("Where to go?");
            foreach (var room in currentRoom.Rooms)
            {
                Console.Write($"{room.Key} - {(int)room.Key}\t");
            }
        }

        private bool HandleCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            
            if (input == "-inventory")
            {
                _player.ShowInventory();
                return true;
            }

            
            if (input.StartsWith("-inventory "))
            {
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[1], out int slot))
                {
                    if (_player is Player player)
                        player.TryEquipFromInventory(slot);
                }
                else
                {
                    Console.WriteLine("Формат: -inventory <индекс>");
                }
                return true;
            }

            if (input == "-info")
            {
                Console.WriteLine(_player.ToString());
                return true;
            }

            if (input == "-quit")
            {
                Console.WriteLine("Выход...");
                Environment.Exit(0);
            }

            return false;
        }
        #endregion
    }
}
