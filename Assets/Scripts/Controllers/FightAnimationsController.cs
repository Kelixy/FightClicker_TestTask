using System.Collections.Generic;
using DG.Tweening;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class FightAnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator[] fighterAnimators;
        [SerializeField] private FightSettings fightSettings;
        [SerializeField] private float awarenessDuration = 0.5f;
        [Range(0f,1f)][SerializeField] private float defenceProbability = 0.5f;

        private (int attacker, int defender) _fighterInd = (0,1);
        private int _currentAttackHash;
        private int _currentAnimationIndex;
        private int CurrentAnimationIndex
        {
            get => _currentAnimationIndex;
            set => _currentAnimationIndex = value >= fightSettings.AnimationsCombinations.Length ? 0 : value;
        }

        public void Hit()
        {
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    _currentAttackHash =
                        fightSettings.AnimationsCombinations[CurrentAnimationIndex].AttackAnimationHash;
                    var fighter = fighterAnimators[_fighterInd.attacker];
                    fighter.Play(_currentAttackHash);
                })
                .AppendInterval(awarenessDuration)
                .AppendCallback(() =>
                {
                    DefendIfPossible();
                    CurrentAnimationIndex++;
                });
        }

        private void SwitchFighters()
        {
            _fighterInd = (_fighterInd.defender, _fighterInd.attacker);
        }

        private bool CheckIfDefencePossible() => Random.Range(0f, 1f) < defenceProbability;

        private void DefendIfPossible()
        {
            var hashKey = CheckIfDefencePossible() ? 
                fightSettings.AnimationsCombinations[CurrentAnimationIndex].DefenceAnimationHash 
                : fightSettings.AnimationsCombinations[CurrentAnimationIndex].HitAnimationHash;
            fighterAnimators[_fighterInd.defender].Play(hashKey);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0))
            {
                SwitchFighters();
                Hit();
            }
        }
    }
}
