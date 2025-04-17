using System;
using System.Collections.Generic;

namespace TextRPG
{
    class Inventory
    {
        public List<Item> Items;

        public Inventory()
        {
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void AddItem(string id, string name, string desc, Dictionary<ItemEffect, float> buffDict, EquipmentType type, long price)
        {
            Items.Add(new Item(id, name, desc, buffDict, type, price));
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
        }

        public void ShowInventory()
        {
            while (true)
            {
                Console.WriteLine("\r\n인벤토리\r\n보유 중인 아이템을 관리할 수 있습니다.\r\n");

                Console.WriteLine("[아이템 목록]");
                foreach (Item item in Items)
                {
                    Console.Write("- ");
                    if (item.IsEquipped)
                    {
                        Console.Write("[E]");
                    }

                    Console.WriteLine($"{item.Name,-10} | {item.GetEffectName(),5} | {item.Desc,5}");
                }

                Console.Write("\r\n1. 장착 관리\r\n2. 나가기\r\n\r\n원하시는 행동을 입력해주세요.\r\n>> ");

                bool isQuit = false;
                bool isInvalidChoice = false;
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowEquipment();
                        break;
                    case "2":
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

        public void ShowEquipment()
        {
            while (true)
            {
                Console.WriteLine("\r\n인벤토리 - 장착 관리\r\n보유 중인 아이템을 관리할 수 있습니다.\r\n");

                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.Write("- {0}. ", i + 1);
                    if (Items[i].IsEquipped)
                    {
                        Console.Write("[E]");
                    }

                    Console.WriteLine($"{Items[i].Name,-10} | {Items[i].GetEffectName(),5} | {Items[i].Desc,5}");
                }

                Console.WriteLine("\r\n0. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice < 0 || choice > Items.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if (choice == 0)
                    {
                        break;
                    }

                    Item item = Items[choice - 1];
                    if (!item.IsEquipped)
                    {
                        Game.Player.EquipItem(item);
                    }
                    else
                    {
                        Game.Player.UnEquipItem(item);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
