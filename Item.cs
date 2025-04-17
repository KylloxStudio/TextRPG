using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TextRPG
{
    class Item
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public Dictionary<ItemEffect, float> ItemEffectDict { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public bool IsEquipped { get; set; }
        public long Price { get; private set; }

        public Item(string id, string name, string desc, Dictionary<ItemEffect, float> effectDict, EquipmentType type, long price)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ItemEffectDict = effectDict;
            EquipmentType = type;
            Price = price;
        }

        public string GetEffectName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<ItemEffect, float> effectPair in ItemEffectDict)
            {
                if (effectPair.Value != 0)
                {
                    switch (effectPair.Key)
                    {
                        case ItemEffect.Atk:
                            sb.Append(" 공격력 ");
                            sb.Append(effectPair.Value < 0f ? "" : "+");
                            sb.Append(effectPair.Value);
                            sb.Append(" ");
                            break;
                        case ItemEffect.Def:
                            sb.Append(" 방어력 ");
                            sb.Append(effectPair.Value < 0f ? "" : "+");
                            sb.Append(effectPair.Value);
                            sb.Append(" ");
                            break;
                        default:
                            sb.Append(" 알 수 없음 ");
                            sb.Append(effectPair.Value < 0f ? "" : "+");
                            sb.Append(effectPair.Value);
                            sb.Append(" ");
                            break;
                    }
                }
            }
            
            return sb.ToString();
        }
    }

    enum ItemEffect
    {
        Atk,
        Def
    }

    enum EquipmentType
    {
        Hand,
        Head,
        Body,
        Legs,
        Foots,
        Item
    }
}
