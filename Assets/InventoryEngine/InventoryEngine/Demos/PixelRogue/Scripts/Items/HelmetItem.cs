using UnityEngine;
using System;

namespace MoreMountains.InventoryEngine
{
    [CreateAssetMenu(fileName = "HelmetItem", menuName = "MoreMountains/InventoryEngine/HelmetItem", order = 2)]
    [Serializable]
    /// <summary>
    /// Demo class for an example armor item
    /// </summary>
    public class HelmetItem : InventoryItem
    {
        [Header("Helmet")]
        public int ArmorIndex;
        public int BonusHealth;

        /// <summary>
        /// What happens when the armor is equipped
        /// </summary>
        public override bool Equip(string playerID)
        {
            base.Equip(playerID);
            //TargetInventory(playerID).TargetTransform.GetComponent<InventoryDemoCharacter>().SetArmor(ArmorIndex);
            return true;
        }

        /// <summary>
        /// What happens when the armor is unequipped
        /// </summary>
        public override bool UnEquip(string playerID)
        {
            base.UnEquip(playerID);
            //TargetInventory(playerID).TargetTransform.GetComponent<InventoryDemoCharacter>().SetArmor(0);
            return true;
        }
    }
}