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
    class PlayerContext
    {
        private static JsonSerializerSettings serializerSettings;

        public int Exp;
        public int Level;
        public string Name;
        public Job Job;
        public Dictionary<string, (float basic, float bonus)> Stats;
        
        public int MaxHp;
        public int Hp;
        public long Gold;
        public bool IsDead;

        public Inventory Inventory;
        public Dictionary<EquipmentType, Item> EquippedItems;

        public PlayerContext()
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public void Initialize(string name)
        {
            Level = 1;
            Name = name;
            Stats = new Dictionary<string, (float basic, float bonus)>
            {
                { "Atk", (10f, 0) },
                { "Def", (5f, 0) }
            };
            MaxHp = 100;
            Hp = MaxHp;
            Job = Job.Warrior;
            Gold = 1500L;
            Inventory = new Inventory();
            EquippedItems = new Dictionary<EquipmentType, Item>
            {
                { EquipmentType.Hand, null },
                { EquipmentType.Head, null },
                { EquipmentType.Body, null },
                { EquipmentType.Legs, null },
                { EquipmentType.Foots, null },
                { EquipmentType.Item, null }
            };
            IsDead = false;
        }

        public void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented, serializerSettings);
                File.WriteAllText("player.json", json);

                Console.WriteLine("플레이어 정보가 저장되었습니다.");
            }
            catch (Exception e)
            {
                Console.WriteLine("플레이어 정보 저장에 실패했습니다.");
                Console.WriteLine(e);
            }
        }

        public static PlayerContext Load()
        {
            PlayerContext data = null;
            try
            {
                if (!File.Exists("player.json"))
                {
                    return null;
                }

                string json = File.ReadAllText("player.json");
                data = JsonConvert.DeserializeObject<PlayerContext>(json, serializerSettings);

                Console.WriteLine("플레이어 정보를 불러왔습니다.");
            }
            catch (Exception e)
            {
                Console.WriteLine("플레이어 정보 로딩에 실패했습니다.");
                Console.WriteLine(e);
            }

            return data;
        }
    }
}
