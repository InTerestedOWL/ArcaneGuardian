using System.Collections.Generic;
using System.Linq;
using AG.Skills.Core;
using UnityEngine;

namespace AG.Skills.Filtering {
    [CreateAssetMenu(fileName = "Tag Filter", menuName = ("Arcane Guardian/Filtering Strategy/Tag Filter"))]
    public class TagFilter : FilterStrategy {
        [SerializeField]
        string[] tags = null;

        public override void FilterTargets(SkillData skillData) {
            skillData.SetTargets(skillData.GetTargets()?.Where(target => tags.Contains(target.tag)));
        }

        public override IEnumerable<GameObject> FilterTargets(IEnumerable<GameObject> targets) {
            return targets.Where(target => tags.Contains(target.tag));
        }
    }
}