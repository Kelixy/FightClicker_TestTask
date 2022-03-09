using System;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Fight_Settings", menuName = "Fight_Settings", order = 51)]
    public class FightSettings : ScriptableObject
    {
        [Serializable]
        public struct AnimationCombinations
        {
            [SerializeField] private string attackAnimationName;
            [SerializeField] private string defenceAnimationName;
            [SerializeField] private string hitAnimationName;

            private int _attackAnimationHash;
            private int _defenceAnimationHash;
            private int _hitAnimationHash;

            public int AttackAnimationHash => _attackAnimationHash == default
                ? _attackAnimationHash = Animator.StringToHash(attackAnimationName)
                : _attackAnimationHash;
            
            public int DefenceAnimationHash => _defenceAnimationHash == default
                ? _defenceAnimationHash = Animator.StringToHash(defenceAnimationName)
                : _defenceAnimationHash;
            
            public int HitAnimationHash => _hitAnimationHash == default
                ? _hitAnimationHash = Animator.StringToHash(hitAnimationName)
                : _hitAnimationHash;
        }
        
        [SerializeField] private AnimationCombinations[] animationsCombinations;

        public AnimationCombinations[] AnimationsCombinations => animationsCombinations;
    }
}
