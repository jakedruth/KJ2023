using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CreatureCard", menuName = "kj2023/Create Creature Card", order = 90)]
public class Creature_SO : Card_SO
{
    [SerializeField]
    public Stats stats;
    public List<Ability.Abilities> Abilities;

    public int GetHP()
    {
        int value = stats.HP * 3;
        foreach (Ability.Abilities ability in Abilities)
        {
            Ability.GetAbilityDetail(ability)().ModifyHP.Invoke(ref value, AttackType.BASE);
        }
        return value;
    }

    public int GetAttack(AttackType type)
    {
        int value = stats.Attack;
        foreach (Ability.Abilities ability in Abilities)
        {
            Ability.GetAbilityDetail(ability)().ModifyAttack.Invoke(ref value, type);
        }
        return value;
    }

    public int GetDefense(AttackType type)
    {
        int value = stats.Defense;
        foreach (Ability.Abilities ability in Abilities)
        {
            Ability.GetAbilityDetail(ability)().ModifyDefense.Invoke(ref value, type);
        }
        return value;
    }
}

[System.Serializable]
public struct Stats
{
    public int HP;
    public int Attack;
    public int Defense;
}

public delegate Ability.Detail AbilityDetail();
public delegate void ModifyStat(ref int value, AttackType type);

public class Ability
{
    public enum Abilities
    {
        BASIC,
        MELEE_ATTACKER,
        RANGED_ATTACKER,
        MELEE_DEFENDER,
        RANGED_DEFENDER,
    }

    public class Detail
    {
        public string Title;
        public string Description;
        public ModifyStat ModifyHP;
        public ModifyStat ModifyAttack;
        public ModifyStat ModifyDefense;

        public Detail(bool StructsCantHaveEmptyConstructorsSoThisIsHereForNoReason = true)
        {
            Title = "";
            Description = "";
            ModifyHP = (ref int value, AttackType type) => { };
            ModifyAttack = (ref int value, AttackType type) => { };
            ModifyDefense = (ref int value, AttackType type) => { };
        }
    }

    private static readonly Dictionary<Abilities, AbilityDetail> AbilityDictionary = new()
        {
            {Abilities.BASIC,           Ability.Basic},
            {Abilities.MELEE_ATTACKER,  Ability.MeleeAttacker},
            {Abilities.RANGED_ATTACKER, Ability.RangedAttacker},
            {Abilities.MELEE_DEFENDER,  Ability.MeleeDefender},
            {Abilities.RANGED_DEFENDER, Ability.RangedDefender}
        };

    public static AbilityDetail GetAbilityDetail(Abilities ability)
    {
        return AbilityDictionary[ability];
    }

    public static Detail Basic()
    {
        return new()
        {
            Title = "No Ability",
            Description = "",
            ModifyHP = (ref int value, AttackType type) => { },
            ModifyAttack = (ref int value, AttackType type) => { },
            ModifyDefense = (ref int value, AttackType type) => { },
        };
    }

    public static Detail MeleeAttacker()
    {
        return new()
        {
            Title = "Strong Melee Attacker",
            Description = "Add 3 attack to any melee attack",
            ModifyHP = (ref int value, AttackType type) => { },
            ModifyAttack = (ref int value, AttackType type) => { if (type == AttackType.MELEE) value += 3; },
            ModifyDefense = (ref int value, AttackType type) => { },
        };
    }

    public static Detail RangedAttacker()
    {
        return new()
        {
            Title = "Strong Ranged Attacker",
            Description = "Add 3 attack to any ranged attack",
            ModifyHP = (ref int value, AttackType type) => { },
            ModifyAttack = (ref int value, AttackType type) => { if (type == AttackType.RANGE) value += 3; },
            ModifyDefense = (ref int value, AttackType type) => { },
        };
    }

    public static Detail MeleeDefender()
    {
        return new()
        {
            Title = "Strong Melee Defender",
            Description = "Add 3 defense to any incoming melee attack",
            ModifyHP = (ref int value, AttackType type) => { },
            ModifyAttack = (ref int value, AttackType type) => { },
            ModifyDefense = (ref int value, AttackType type) => { if (type == AttackType.MELEE) value += 3; },
        };
    }

    public static Detail RangedDefender()
    {
        return new()
        {
            Title = "Strong Ranged Defender",
            Description = "Add 3 defense to any incoming ranged attack",
            ModifyHP = (ref int value, AttackType type) => { },
            ModifyAttack = (ref int value, AttackType type) => { },
            ModifyDefense = (ref int value, AttackType type) => { if (type == AttackType.RANGE) value += 3; },
        };
    }
}
