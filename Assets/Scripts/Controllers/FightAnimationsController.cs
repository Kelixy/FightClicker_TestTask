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
        private float _fightAnimLength;
        private int _currentAnimationIndex;
        private int CurrentAnimationIndex
        {
            get => _currentAnimationIndex;
            set => _currentAnimationIndex = value >= fightSettings.AnimationsCombinations.Length ? 0 : value;
        }
        
        public bool AnimationsArePlaying { get; set; }

        public void Hit()
        {
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    AnimationsArePlaying = true;
                    var fighter = fighterAnimators[_fighterInd.attacker];
                    var combination = fightSettings.AnimationsCombinations[CurrentAnimationIndex];
                    fighter.Play(combination.AttackAnimationHash);
                })
                .AppendInterval(awarenessDuration)
                .AppendCallback(() =>
                {
                    DefendIfPossible();
                    _fightAnimLength = GetAnimationDuration();
                    CurrentAnimationIndex++;
                })
                .InsertCallback(_fightAnimLength, () =>
                {
                    AnimationsArePlaying = false;
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

        private float GetAnimationDuration()
        {
            var attackerClipInfo = fighterAnimators[_fighterInd.attacker].GetCurrentAnimatorClipInfo(0);
            var defenderClipInfo = fighterAnimators[_fighterInd.defender].GetCurrentAnimatorClipInfo(0);
            var attackerClipLength = attackerClipInfo[0].clip.length;
            var defenderClipLength = defenderClipInfo[0].clip.length;
            
            return attackerClipLength > defenderClipLength ? attackerClipLength : defenderClipLength;
        }
    }
}
