using System;
using System.Collections.Generic;
using System.Linq;
using AG.Control;
using AG.Skills.Filtering;
using UnityEngine;

namespace AG.Skills.Core {
    public class SkillData {
        private GameObject user;
        private IEnumerable<GameObject> targets;
        private PlayerController playerController;
        private TurretController turretController;
        private Vector3 targetPosition;
        private float radius = 0;
        private FilterStrategy filterStrategy = null;
        private Skill skill = null;

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

        public void SetSkill(Skill skill) {
            this.skill = skill;
        }

        public Skill GetSkill() {
            return skill;
        }

        public void SetFilterStrategy(FilterStrategy filterStrategy) {
            this.filterStrategy = filterStrategy;
        }

        public FilterStrategy GetFilterStrategy() {
            return this.filterStrategy;
        }
    }
}