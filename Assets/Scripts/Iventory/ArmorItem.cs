using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "ArmorItem", menuName = "MoreMountains/InventoryEngine/ArmorItem", order = 2)]
	[Serializable]
	/// <summary>
	/// Demo class for an example armor item
	/// </summary>
	public class ArmorItem : InventoryItem 
	{
		[Header("Armor")]
		public int ArmorIndex;
		public int BonusHealth;

        /// <summary>
        /// What happens when the armor is equipped
        /// </summary>
        public override bool Equip(string playerID)
        {
            base.Equip(playerID);
           // MainGameScript.instance.SetPlayerStats(); // `this` is the equipped ArmorItem
            return true;
        }

        public override bool UnEquip(string playerID)
        {
            base.UnEquip(playerID);

            //MainGameScript.instance.SetPlayerStats(); // No armor equipped
            return true;
        }

    }
}