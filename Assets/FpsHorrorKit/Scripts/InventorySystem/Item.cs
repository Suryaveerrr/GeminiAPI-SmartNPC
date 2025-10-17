namespace FpsHorrorKit
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item Properties")]
        public string itemName;
        // For values like battery percentage for a flashlight, or bullet count in a weapon's magazine.
        public float energyLevel;
        public Sprite icon;
        public bool isStackable;
        public int maxStackSize;

        [Header("Bool Control")]
        public bool hasItem;
        public bool canUseItem;
        public bool isUsingItem;
        public bool isEnergyEnough;
        public Action useItemAction;
    }
}