using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAnimationsController : MonoBehaviour
{
    private enum AnimationType
    {
        Idle = 0,
        AttackEasy = 1,
        AttackHard,
        AttackShield,
        Dodge,
        Defence
    }
    
    [SerializeField] private Animator[] fighterAnimators;
    [SerializeField] private Transform[] fightersSwords;

    private int _defenceSwordKey = Animator.StringToHash("defence");
    private int _defenceComboKey = Animator.StringToHash("Defence_breaked");
    private int _attackSwordKey = Animator.StringToHash("attack_0");
    private int _attackComboKey = Animator.StringToHash("Combo_04_3");

    private (int attacker, int defender) _fighterInd = (0,1);

    public void Hit()
    {
        fighterAnimators[_fighterInd.attacker].Play(_attackComboKey);
    }

    private void SwitchFighters()
    {
        _fighterInd = (_fighterInd.defender, _fighterInd.attacker);
    }

    private float GetDistanceToSword()
    {
        var defenderPos = fighterAnimators[_fighterInd.defender].transform.position;
        var attackerSwordPos = fightersSwords[_fighterInd.attacker].position;
        return (attackerSwordPos - defenderPos).magnitude;
    }

    private void DefendIfPossible()
    {
        if (GetDistanceToSword() < 2f)
            fighterAnimators[_fighterInd.defender].Play(_defenceComboKey);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchFighters();
            Hit();
        }

        DefendIfPossible();
    }
}
