using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AG.Skills.Filtering {
    [CreateAssetMenu(fileName = "Tag Filter", menuName = ("Arcane Guardian/Filtering Strategy/Tag Filter"))]
    public class TagFilter : FilterStrategy {
        [SerializeField]
        string[] tags = null;

        public override IEnumerable<GameObject> FilterTargets(IEnumerable<GameObject> targets) {
            return targets.Where(target => tags.Contains(target.tag));
        }
    }
}