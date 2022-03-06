using DG.Tweening;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class FightAnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator[] fighterAnimators;
        [SerializeField] private ParticleSystem[] bloodEffects;
        [SerializeField] private FightSettings fightSettings;
        
        [SerializeField] private float awarenessDuration = 0.5f;
        [Range(0f,1f)][SerializeField] private float defenceProbability = 0.5f;

        private (int attacker, int defender) _fighterInd = (0,1);
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
                    var fighter = fighterAnimators[_fighterInd.attacker];
                    var combination = fightSettings.AnimationsCombinations[CurrentAnimationIndex];
                    fighter.Play(combination.AttackAnimationHash);
                })
                .AppendInterval(awarenessDuration)
                .AppendCallback(() =>
                {
                    DefendIfPossible();
                    CurrentAnimationIndex++;
                });
        }

        public void SwitchFighters()
        {
            _fighterInd = (_fighterInd.defender, _fighterInd.attacker);
        }

        private bool CheckIfDefencePossible() => Random.Range(0f, 1f) < defenceProbability;

        private void DefendIfPossible()
        {
            var defenceIsPossible = CheckIfDefencePossible();
            var hashKey = defenceIsPossible ? 
                fightSettings.AnimationsCombinations[CurrentAnimationIndex].DefenceAnimationHash 
                : fightSettings.AnimationsCombinations[CurrentAnimationIndex].HitAnimationHash;
            fighterAnimators[_fighterInd.defender].Play(hashKey);
            if (!defenceIsPossible) bloodEffects[_fighterInd.defender].Play();
        }
    }
}
