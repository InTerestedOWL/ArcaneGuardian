// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System.Collections.Generic;
using UnityEngine;

/*
 * Strategy to determine how to filter a skill (e.g. only enemies, only allies, etc.)
 */
namespace AG.Skills.Filtering {
    public abstract class FilterStrategy : ScriptableObject {
        public abstract IEnumerable<GameObject> FilterTargets(IEnumerable<GameObject> targets);
    }
}