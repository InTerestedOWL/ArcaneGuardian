// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Actions {
    public abstract class ActionItem : ScriptableObject {
        [Tooltip("Name of item to be displayed to player.")]
        [SerializeField] string displayName = null;
        [Tooltip("Description of item to be displayed to player.")]
        [SerializeField][TextArea] string description = null;
        [Tooltip("UI Icon to represent item.")]
        [SerializeField] Sprite icon = null;
        [SerializeField] protected float cooldown = 0;

        [NonSerialized]
        protected float currentCooldown = 0;
        [NonSerialized]
        protected List<SlotCooldownUI> cooldownUIs = new List<SlotCooldownUI>();
        [NonSerialized]
        protected bool isOnCooldown = false;

        public Sprite GetIcon() {
            return icon;
        }
        
        public string GetDisplayName() {
            return displayName;
        }

        public string GetDescription() {
            return description;
        }

        public float GetMaxCooldown() {
            return cooldown;
        }

        public float GetCurrentCooldown() {
            return currentCooldown;
        }

        public bool IsItemOnCooldown() {
            return isOnCooldown;
        }

        public void AddNewCooldownUI(SlotCooldownUI cooldownUI) {
            if (cooldownUI != null) {
                cooldownUIs.Add(cooldownUI);
            }
        }

        public void RemoveCooldownUI(SlotCooldownUI cooldownUI) {
            cooldownUIs.Remove(cooldownUI);
        }

        public virtual void Use(GameObject user) {
            throw new NotImplementedException();
        }

        public virtual IEnumerator StartCooldown() {
            yield return new WaitForFixedUpdate();
        }
    }
}
