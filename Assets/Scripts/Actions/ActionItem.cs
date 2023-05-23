// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using UnityEngine;

namespace AG.Actions {
    public abstract class ActionItem : ScriptableObject {
        [Tooltip("Name of item to be displayed to player.")]
        [SerializeField] string displayName = null;
        [Tooltip("Description of item to be displayed to player.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("UI Icon to represent item.")]
        [SerializeField] Sprite icon = null;

        public Sprite GetIcon() {
            return icon;
        }
        
        public string GetDisplayName() {
            return displayName;
        }

        public string GetDescription() {
            return description;
        }

        public virtual void Use(GameObject user)
        {
            throw new NotImplementedException();
        }
    }
}
