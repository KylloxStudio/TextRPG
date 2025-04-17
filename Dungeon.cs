using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Dungeon
    {
        public int LevelUpStack { get; private set; }
        public Dictionary<DungeonDifficulty, float> Difficulties { get; private set; } // 난이도, 권장 방어력

        public Dungeon()
        {
            LevelUpStack = 0;
            Difficulties = new Dictionary<DungeonDifficulty, float>
            {
                { DungeonDifficulty.Easy, 5f },
                { DungeonDifficulty.Normal, 11f },
                { DungeonDifficulty.Hard, 17f },
                { DungeonDifficulty.Extreme, 33f },
                { DungeonDifficulty.Insane, 77f }
            };
        }

        public void OpenDungeonTab()
        {
            if (Game.Player.Context.IsDead)
            {
                Console.WriteLine("전투 불능 상태에서는 던전에 입장할 수 없습니다.");
                return;
            }

            while (!Game.Player.Context.IsDead)
            {
                Console.WriteLine("\r\n던전입장\r\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\r\n");

                int idx = 0;
                foreach (KeyValuePair<DungeonDifficulty, float> diffPair in Difficulties)
                {
                    idx++;
                    Console.WriteLine($"{idx}. {diffPair.Key + " 던전",-10} | {"방어력 " + diffPair.Value + " 이상 권장",10}");
                }

                Console.WriteLine("\r\n0. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice < 0 || choice > Difficulties.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }

                    if (choice == 0)
                    {
                        break;
                    }

                    Enter(Difficulties.ElementAt(choice - 1).Key);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        public void Enter(DungeonDifficulty difficulty)
        {
            float diff = Game.Player.Def - Difficulties[difficulty];
            double chance = (double)Utils.Clamp(0.9f + (diff * 0.05f), 0.005f, 0.95f); // 난이도에 따른 클리어 확률
            if (new Random().NextDouble() < chance)
            {
                Clear(difficulty);
            }
            else
            {
                Failed(difficulty);
            }
        }

        public void Clear(DungeonDifficulty difficulty)
        {
            Game.Player.Hp -= GetHpLoss(difficulty);
            Game.Player.Context.Gold += GetClearGold(difficulty);

            if (Game.Player.Context.IsDead)
            {
                return;
            }

            Console.WriteLine($"\r\n던전 클리어\r\n축하합니다!!\r\n{difficulty} 던전을 클리어 하였습니다.\r\n");
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {Game.Player.Hp + GetHpLoss(difficulty)} -> {Game.Player.Hp}");
            Console.WriteLine($"Gold {Game.Player.Context.Gold - GetClearGold(difficulty)} -> {Game.Player.Context.Gold}G");

            Game.Player.GainExp(GetClearExp(difficulty));
        }

        public void Failed(DungeonDifficulty difficulty)
        {
            Game.Player.Hp -= GetHpLoss(difficulty) / 2;

            Console.WriteLine($"{difficulty} 던전 클리어에 실패했습니다.");
            Console.WriteLine($"체력 {Game.Player.Hp + GetHpLoss(difficulty) / 2} -> {Game.Player.Hp}");
        }

        public int GetHpLoss(DungeonDifficulty difficulty)
        {
            return new Random().Next((int)(20f + (Difficulties[difficulty] - Game.Player.Def)), (int)(36f + (Difficulties[difficulty] - Game.Player.Def)));
        }

        public long GetClearGold(DungeonDifficulty difficulty)
        {
            double bonusPercent = Utils.NextDouble((double)Game.Player.Atk, (double)(Game.Player.Atk * 2f)) * 0.1;
            switch (difficulty)
            {
                case DungeonDifficulty.Easy:
                    return (long)(1000.0 * bonusPercent);
                case DungeonDifficulty.Normal:
                    return (long)(1700.0 * bonusPercent);
                case DungeonDifficulty.Hard:
                    return (long)(2500.0 * bonusPercent);
                case DungeonDifficulty.Extreme:
                    return (long)(3300.0 * bonusPercent);
                case DungeonDifficulty.Insane:
                    return (long)(7000.0 * bonusPercent);
                default:
                    return 0L;
            }
        }

        public int GetClearExp(DungeonDifficulty difficulty)
        {
            switch (difficulty)
            {
                case DungeonDifficulty.Easy:
                    return 1;
                case DungeonDifficulty.Normal:
                    return 3;
                case DungeonDifficulty.Hard:
                    return 5;
                case DungeonDifficulty.Extreme:
                    return 10;
                case DungeonDifficulty.Insane:
                    return 18;
                default:
                    return 0;
            }
        }
    }

    enum DungeonDifficulty
    {
        Easy,
        Normal,
        Hard,
        Extreme,
        Insane
    }
}
