using System;
using System.Linq;
using AG.Combat;
using AG.Skills.Core;
using AG.Control;
using AG.MovementCore;
using UnityEngine;
using System.Collections;

namespace AG.Skills.Effects
{
    [CreateAssetMenu(fileName = "Blink Effect", menuName = ("Arcane Guardian/Effect Strategy/Blink Effect"))]
    public class BlinkEffect : EffectStrategy
    {
        [SerializeField] int distance;
        [SerializeField] float speed;
        // Stopping distance is the distance the user stops before colliding with an object
        [SerializeField] float stoppingDistance;
        [SerializeField] LayerMask layerMask;

        private Vector3 destination;

        public override void ApplyEffect(SkillData data)
        {
            Blink(data);
        }

        private void Blink(SkillData data) {
            GameObject user = data.GetUser();
            PlayerController pc = data.GetPlayerController();

            Vector3 direction = data.GetTargetPosition() - user.transform.position;

            Ray ray = new Ray(user.transform.position, direction);

            //Kollision mit Objekten prüfen
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, distance, layerMask)) {
                float collisionDistance = Mathf.Max(hit.distance - stoppingDistance, 0f);

                // Setze die Zielposition kurz vor der Kollision
                destination = ray.origin + ray.direction * collisionDistance;
            }
            else {
                destination = user.transform.position + ray.direction * distance;
            }

            destination.y = user.transform.position.y;

            pc.StartCoroutine(BlinkToTargetPosition(user));
        }

        private IEnumerator BlinkToTargetPosition(GameObject user) {
            var dist = Vector3.Distance(user.transform.position, destination);
            Movement movement = user.GetComponent<Movement>();

            while(dist > 0.5f) {
                user.transform.position = Vector3.MoveTowards(user.transform.position, destination, Time.deltaTime * speed);
                movement.DoMovement(user.transform.position);
                dist = Vector3.Distance(user.transform.position, destination);
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }
    }
}
