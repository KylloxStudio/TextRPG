using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Rest
    {
        public long Price { get; private set; }

        public Rest(long price)
        {
            Price = price;
        }

        public void OpenRestTab()
        {
            while (true)
            {
                Console.WriteLine($"\r\n휴식하기\r\n{Price}G를 내면 체력을 회복할 수 있습니다. (보유 골드: {Game.Player.Context.Gold}G)\r\n");

                Console.WriteLine("\r\n1. 휴식하기\r\n2. 나가기\r\n");
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");

                bool isQuit = false;
                bool isInvalidChoice = false;
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        StartRest();
                        isQuit = true;
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

        public void StartRest()
        {
            if (Game.Player.Context.Gold < Price)
            {
                Console.WriteLine("Gold가 부족합니다.");
                return;
            }

            if (Game.Player.Hp >= Game.Player.Context.MaxHp)
            {
                Console.WriteLine("체력이 이미 완전히 회복되어 있습니다.");
                return;
            }

            Game.Player.Context.Gold -= Price;
            Game.Player.Hp = Game.Player.Context.MaxHp;
            Game.Player.Context.IsDead = false;
            
            Console.WriteLine("충분히 휴식했습니다. 체력이 완전히 회복되었습니다.");
        }
    }
}
