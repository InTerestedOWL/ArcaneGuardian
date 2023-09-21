using System;
using System.Linq;
using AG.Combat;
using AG.Skills.Core;
using AG.Control;
using AG.MovementCore;
using UnityEngine;
using System.Collections;
using AG.Audio.Sounds;
using UnityEngine.TextCore.Text;

namespace AG.Skills.Effects
{
    [CreateAssetMenu(fileName = "Blade Spin Effect", menuName = ("Arcane Guardian/Effect Strategy/Blade Spin Effect"))]
    public class BladeSpinEffect : EffectStrategy
    {
        [SerializeField] int spinCount = 3;
        //Drehungen pro Sekunde
        [SerializeField] float spinsPerSecond = 3;
        [SerializeField] ChargeSounds chargeSounds;

        public override void ApplyEffect(SkillData data)
        {
            GameObject user = data.GetUser();
            PlayerController pc = data.GetPlayerController();
            Movement movement = user.GetComponent<Movement>();

            pc.StartCoroutine(BladeSpin(user, pc, movement));
        }

        private IEnumerator BladeSpin(GameObject user, PlayerController pc, Movement movement) {
            pc.DisableLookAtMousePos();
            movement.SuspendRotation();

            BasicCombat bc = user.GetComponent<BasicCombat>();
            bc.IncreaseAttackStateCounter();

            Animator animator = user.GetComponent<Animator>();
            animator.SetTrigger("bladeSpin");

            if (chargeSounds != null) {
                CharacterAudioController cac = user.GetComponent<CharacterAudioController>();
                if (cac != null) {
                    cac.PlaySound(chargeSounds.GetRandomChargeSound());
                }
            }

            float totalRotation = spinCount * 360f; // Gesamte Drehung in Grad
            float currentRotation = 0f;

            float degreesPerSecond = spinsPerSecond * 360f; // Drehung pro Sekunde

            while (currentRotation < totalRotation) {
                float rotationAmount = degreesPerSecond * Time.fixedDeltaTime;
                user.transform.Rotate(Vector3.up, rotationAmount);
                currentRotation += rotationAmount;
                yield return new WaitForFixedUpdate();
            }

            pc.EnableLookAtMousePos();
            movement.ResumeRotation();
            // animator.ResetTrigger("bladeSpin");
            bc.DecreaseAttackStateCounter();
        }
    }
}
