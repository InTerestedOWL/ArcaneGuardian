using System;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Skills.Targeting {
    [CreateAssetMenu(fileName = "New Targeting Strategy", menuName = ("Arcane Guardian/Targeting Strategy/Targeting Strategy Stub"))]
    public class TargetingStrategyStub : TargetingStrategy {
        public override void DeclareTargets(GameObject user, Action<IEnumerable<GameObject>> callback) {
            throw new System.NotImplementedException();
        }
    }
}