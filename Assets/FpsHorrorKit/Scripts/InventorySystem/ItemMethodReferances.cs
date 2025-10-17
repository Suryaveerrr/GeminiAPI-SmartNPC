namespace FpsHorrorKit
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class ItemMethodReferences : MonoBehaviour
    {
        [Tooltip("A specific event can be assigned for each item.")]
        [SerializeField] private ItemEventPair[] itemEventPairs; // Item and event pairs

        [Serializable]
        public class ItemEventPair
        {
            public Item item;
            public UnityEvent onUseItem;
        }

        private void OnEnable()
        {
            foreach (ItemEventPair pair in itemEventPairs)
            {
                if (pair.item != null && pair.onUseItem != null)
                {
                    pair.item.useItemAction += pair.onUseItem.Invoke;
                }
            }
        }

        private void OnDisable()
        {
            foreach (ItemEventPair pair in itemEventPairs)
            {
                if (pair.item != null && pair.onUseItem != null)
                {
                    pair.item.useItemAction -= pair.onUseItem.Invoke;
                }
            }
        }
    }
}