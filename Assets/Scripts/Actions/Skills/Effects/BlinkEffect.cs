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

        private void Blink(SkillData data)
        {
            GameObject user = data.GetUser();
            PlayerController pc = data.GetPlayerController();

            Vector3 direction = data.GetTargetPosition() - user.transform.position;
            direction.y = 0f;

            Ray ray = new Ray(user.transform.position, direction);

            //Kollision mit Objekten prüfen
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                float collisionDistance = Mathf.Max(hit.distance - stoppingDistance, 0f);

                // Setze die Zielposition kurz vor der Kollision
                destination = ray.origin + ray.direction * collisionDistance;
            }
            else
            {
                destination = user.transform.position + ray.direction * distance;
            }

            destination.y = user.transform.position.y;

            pc.StartCoroutine(BlinkToTargetPosition(user));
        }

        private IEnumerator BlinkToTargetPosition(GameObject user)
        {
            Movement movement = user.GetComponent<Movement>();

            while (Vector2.Distance(new Vector2(user.transform.position.x, user.transform.position.z), new Vector2(destination.x, destination.z)) > 0.5f)
            {
                // Berechnen die nächste Position
                Vector3 curMovement = Vector3.MoveTowards(user.transform.position, destination, Time.deltaTime * speed);

                // Führen Raycast von der nächsten Position aus
                RaycastHit hit;
                bool hasHit = Physics.Raycast(curMovement + Vector3.up * 10, Vector3.down, out hit, 100, LayerMask.GetMask("Map"));

                // Debug.DrawRay(curMovement + Vector3.up * 10, Vector3.down * 100, Color.red, 1f);
                //Nur bewegen, wenn kein Wasser getroffen wurde.
                if (hasHit && hit.point.y > 0.1f)
                {
                    user.transform.position = curMovement;
                    movement.DoMovement(user.transform.position);
                    Debug.Log("Blinking" + curMovement);
                }
                //Abbrechen, wenn hit.point.y <= 0.1f (Wasser beginnt bei 0, wir können auf Wasser nicht laufen)
                else
                {
                    break;
                }

                yield return new WaitForFixedUpdate();
            }

            // Coroutine beenden
            yield return null;
        }
    }
}
