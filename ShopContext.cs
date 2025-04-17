using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TextRPG
{
    [Serializable]
    class ShopContext
    {
        private static JsonSerializerSettings serializerSettings;

        public List<Item> Items;
        public Dictionary<string, bool> ItemDict; // 아이템 아이디, 구매 여부

        public ShopContext()
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public void Initialize()
        {
            Items = new List<Item>();
            Items.Add(new Item(Guid.NewGuid().ToString(), "수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Def, 5f }
            }, EquipmentType.Body, 1000L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Def, 9f }
            }, EquipmentType.Body, 2200L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Def, 15f }
            }, EquipmentType.Body, 3500L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Atk, 2f }
            }, EquipmentType.Hand, 600L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "청동 도끼", "어디선가 사용되었던 것 같은 도끼입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Atk, 5f }
            }, EquipmentType.Hand, 1500L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Atk, 7f }
            }, EquipmentType.Hand, 3500L));

            Items.Add(new Item(Guid.NewGuid().ToString(), "공격력 주문서", "장착 시 공격력이 증가하지만, 방어력이 감소합니다.", new Dictionary<ItemEffect, float>
            {
                { ItemEffect.Atk, 10f },
                { ItemEffect.Def, -4f }
            }, EquipmentType.Item, 2000L));

            ItemDict = new Dictionary<string, bool>();
            foreach (Item item in Items)
            {
                ItemDict.Add(item.Id, false);
            }
        }

        public void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented, serializerSettings);
                File.WriteAllText("shop.json", json);

                Console.WriteLine("상점 정보가 저장되었습니다.");
            }
            catch (Exception e)
            {
                Console.WriteLine("상점 정보 저장에 실패했습니다.");
                Console.WriteLine(e);
            }
        }

        public static ShopContext Load()
        {
            ShopContext data = null;
            try
            {
                if (!File.Exists("shop.json"))
                {
                    return null;
                }

                string json = File.ReadAllText("shop.json");
                data = JsonConvert.DeserializeObject<ShopContext>(json, serializerSettings);

                Console.WriteLine("상점 정보를 불러왔습니다.");
            }
            catch (Exception e)
            {
                Console.WriteLine("상점 정보 로딩에 실패했습니다.");
                Console.WriteLine(e);
            }

            return data;
        }
    }
}
