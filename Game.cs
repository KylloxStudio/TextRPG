using System;
using System.Collections.Generic;

namespace TextRPG
{
    class Game
    {
        public static Player Player { get; private set; }
        private static Shop Shop;

        static void Main(string[] args)
        {
            Player = new Player();
            Shop = new Shop();
            Dungeon dungeon = new Dungeon();
            Rest rest = new Rest(500L);

            Console.WriteLine($"\r\n{Player.Context.Name}님, 스파르타 마을에 오신 것을 환영합니다.");

            while (true)
            {
                Console.Write("\r\n스파르타 마을\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n\r\n1. 상태 보기\r\n2. 인벤토리\r\n3. 상점\r\n4. 던전입장\r\n5. 휴식하기\r\n6. 저장하기\r\n7. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>> ");

                bool isQuit = false;
                bool isInvalidChoice = false;
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Player.PrintInfo();
                        break;
                    case "2":
                        Player.Context.Inventory.ShowInventory();
                        break;
                    case "3":
                        Shop.OpenShop();
                        break;
                    case "4":
                        dungeon.OpenDungeonTab();
                        break;
                    case "5":
                        rest.OpenRestTab();
                        break;
                    case "6":
                        Save();
                        break;
                    case "7":
                        isQuit = true;
                        break;
                    default:
                        isInvalidChoice = true;
                        break;
                }

                if (isQuit)
                {
                    break;
                }

                if (isInvalidChoice)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        static void Save()
        {
            Player.Context.Save();
            Shop.Context.Save();
        }
    }
}
