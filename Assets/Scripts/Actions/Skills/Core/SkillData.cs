using System;
using System.Collections.Generic;
using AG.Control;
using UnityEngine;

namespace AG.Skills.Core {
    public class SkillData {
        private GameObject user;
        private IEnumerable<GameObject> targets;
        private PlayerController playerController;
        private TurretController turretController;
        private Vector3 targetPosition;
        private float radius = 0;

        public SkillData(GameObject user) {
            this.user = user;
        }

        public void SetTargets(IEnumerable<GameObject> targets) {
            this.targets = targets;
        }

        public IEnumerable<GameObject> GetTargets() {
            return targets;
        }

        public void SetTargetPosition(Vector3 targetPosition) {
            this.targetPosition = targetPosition;
        }

        public Vector3 GetTargetPosition() {
            return targetPosition;
        }

        public GameObject GetUser() {
            return user;
        }

        public PlayerController GetPlayerController() {
            if (playerController == null) {
                playerController = user.GetComponent<PlayerController>();
            }
            return playerController;
        }

        public TurretController GetTurretController() {
            if (turretController == null) {
                turretController = user.GetComponent<TurretController>();
            }
            return turretController;
        }

        public void SetRadius(float aoeRadius) {
            radius = aoeRadius;
        }

        public float GetRadius() {
            return radius;
        }
    }
}