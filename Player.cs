using System;
using System.Collections.Generic;

namespace TextRPG
{
    class Player
    {
        public PlayerContext Context;

        public int ExpToNextLevel => Context.Level;
        public float Atk
        {
            get
            {
                return Context.Stats["Atk"].basic + Context.Stats["Atk"].bonus;
            }
        }
        public float Def
        {
            get
            {
                return Context.Stats["Def"].basic + Context.Stats["Def"].bonus;
            }
        }
        public int Hp
        {
            get
            {
                return Context.Hp;
            }
            set
            {
                if (value <= 0)
                {
                    Context.Hp = 0;
                }
                else if (value > Context.MaxHp)
                {
                    Context.Hp = Context.MaxHp;
                }
                else
                {
                    Context.Hp = value;
                }
            }
        }

        public Player()
        {
            PlayerContext context = PlayerContext.Load();
            if (context == null)
            {
                Console.Write("원하시는 이름을 설정해주세요.\r\n>> ");
                Context = new PlayerContext();
                Context.Initialize(Console.ReadLine());
            }
            else
            {
                Context = context;
            }
        }

        public string GetJobName()
        {
            switch (Context.Job)
            {
                case Job.Warrior:
                    return "전사";
                default:
                    return "알 수 없음";
            }
        }

        public void PrintInfo()
        {
            while (true)
            {
                Console.WriteLine("\r\n상태 보기\r\n캐릭터의 정보가 표시됩니다.\r\n");

                Console.WriteLine("Lv. {0:D2}", Context.Level);
                Console.WriteLine("이름: {0}", Context.Name);
                Console.WriteLine("직업: {0}", GetJobName());
                Console.WriteLine("공격력: {0} (+{1})", Atk, Context.Stats["Atk"].bonus);
                Console.WriteLine("방어력: {0} (+{1})", Def, Context.Stats["Def"].bonus);
                Console.WriteLine("체력: {0} / {1}", Context.Hp, Context.MaxHp);
                Console.WriteLine("Gold: {0}G", Context.Gold);

                Console.WriteLine("\r\n0. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                string choice = Console.ReadLine();
                if (choice == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        public void EquipItem(Item item)
        {
            if (Context.EquippedItems[item.EquipmentType] != null)
            {
                UnEquipItem(Context.EquippedItems[item.EquipmentType]);
            }

            item.IsEquipped = true;
            Context.EquippedItems[item.EquipmentType] = item;

            foreach (KeyValuePair<ItemEffect, float> effectPair in item.ItemEffectDict)
            {
                switch (effectPair.Key)
                {
                    case ItemEffect.Atk:
                        Context.Stats["Atk"] = (Context.Stats["Atk"].basic, Context.Stats["Atk"].bonus + effectPair.Value);
                        break;
                    case ItemEffect.Def:
                        Context.Stats["Def"] = (Context.Stats["Def"].basic, Context.Stats["Def"].bonus + effectPair.Value);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("{0}(이)가 장착되었습니다.", item.Name);
        }

        public void UnEquipItem(Item item)
        {
            item.IsEquipped = false;
            Context.EquippedItems[item.EquipmentType] = null;

            foreach (KeyValuePair<ItemEffect, float> effectPair in item.ItemEffectDict)
            {
                switch (effectPair.Key)
                {
                    case ItemEffect.Atk:
                        Context.Stats["Atk"] = (Context.Stats["Atk"].basic, Context.Stats["Atk"].bonus - effectPair.Value);
                        break;
                    case ItemEffect.Def:
                        Context.Stats["Def"] = (Context.Stats["Def"].basic, Context.Stats["Def"].bonus - effectPair.Value);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("{0}(이)가 장착 해제되었습니다.", item.Name);
        }

        public void GainExp(int amount)
        {
            Context.Exp += amount;

            while (Context.Exp >= ExpToNextLevel)
            {
                Context.Exp -= ExpToNextLevel;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Context.Level++;

            Context.Stats["Atk"] = (Context.Stats["Atk"].basic + 0.5f, Context.Stats["Atk"].bonus);
            Context.Stats["Def"] = (Context.Stats["Def"].basic + 1f, Context.Stats["Def"].bonus);

            Console.WriteLine($"Level UP! {Context.Level - 1} -> {Context.Level}");
        }

        public void Death()
        {
            Context.IsDead = true;
            Console.WriteLine("\r\n전투 불능 상태가 되었습니다.");
        }
    }
}
