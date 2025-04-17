using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Shop
    {
        public ShopContext Context;

        public Shop()
        {
            ShopContext context = ShopContext.Load();
            if (context == null)
            {
                Context = new ShopContext();
                Context.Initialize();
            }
            else
            {
                Context = context;
            }
        }

        public void OpenShop()
        {
            while (true)
            {
                Console.WriteLine("\r\n상점\r\n필요한 아이템을 얻을 수 있는 상점입니다.\r\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\r\n", Game.Player.Context.Gold);

                Console.WriteLine("[아이템 목록]");
                foreach (KeyValuePair<string, bool> itemPair in Context.ItemDict)
                {
                    Item item = Context.Items.Find((x) => x.Id == itemPair.Key);
                    Console.WriteLine($"- {item.Name,-10} | {item.GetEffectName(),-10} | {item.Desc,-10}");
                }

                Console.WriteLine("\r\n1. 아이템 구매\r\n2. 아이템 판매\r\n3. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                bool isQuit = false;
                bool isInvalidChoice = false;
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        OpenBuyTab();
                        break;
                    case "2":
                        OpenSellTab();
                        break;
                    case "3":
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

        private void OpenBuyTab()
        {
            while (true)
            {
                Console.WriteLine("\r\n상점 - 아이템 구매\r\n필요한 아이템을 얻을 수 있는 상점입니다.\r\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\r\n", Game.Player.Context.Gold);

                Console.WriteLine("[아이템 목록]");
                int idx = 0;
                foreach (KeyValuePair<string, bool> itemPair in Context.ItemDict)
                {
                    idx++;
                    Item item = Context.Items.Find((x) => x.Id == itemPair.Key);
                    Console.WriteLine($"- {idx}. {item.Name,-10} | {item.GetEffectName(),-10} | {item.Desc,-10} | {(itemPair.Value ? "구매 완료" : item.Price.ToString() + "G"),-10}");
                }

                Console.WriteLine("\r\n0. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice < 0 || choice > Context.Items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if (choice == 0)
                    {
                        break;
                    }

                    Item item = Context.Items.ElementAt(choice - 1);
                    bool isPurchased = Context.ItemDict.ElementAt(choice - 1).Value;

                    if (isPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        continue;
                    }

                    if (Game.Player.Context.Gold < item.Price)
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        continue;
                    }

                    BuyItem(item);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        private void OpenSellTab()
        {
            while (true)
            {
                Console.WriteLine("\r\n상점 - 아이템 판매\r\n필요한 아이템을 얻을 수 있는 상점입니다.\r\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\r\n", Game.Player.Context.Gold);

                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Game.Player.Context.Inventory.Items.Count; i++)
                {
                    Item item = Game.Player.Context.Inventory.Items[i];
                    Console.Write("- {0}. ", i + 1);
                    if (item.IsEquipped)
                    {
                        Console.Write("[E]");
                    }

                    Console.WriteLine($"{item.Name,-10} | {item.GetEffectName(),-10} | {item.Desc,-10} | {Math.Round(item.Price * 0.85) + "G",-10}");
                }

                Console.WriteLine("\r\n0. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice < 0 || choice > Context.Items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if (choice == 0)
                    {
                        break;
                    }

                    SellItem(Context.Items.ElementAt(choice - 1));
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        private void BuyItem(Item item)
        {
            Context.ItemDict[item.Id] = true;

            Game.Player.Context.Gold -= item.Price;
            Game.Player.Context.Inventory.AddItem(item);

            Console.WriteLine($"{item.Name}을(를) 구매했습니다. 남은 골드: {Game.Player.Context.Gold}G");
        }

        private void SellItem(Item item)
        {
            Context.ItemDict[item.Id] = false;

            Item playerItem = Game.Player.Context.Inventory.Items.Find((x) => x.Id == item.Id);
            if (playerItem.IsEquipped)
            {
                Game.Player.UnEquipItem(playerItem);
            }

            Game.Player.Context.Inventory.RemoveItem(item);
            Game.Player.Context.Gold += (long)Math.Round(item.Price * 0.85);

            Console.WriteLine($"{item.Name}을(를) 판매했습니다. 남은 골드: {Game.Player.Context.Gold}G");
        }
    }
}
