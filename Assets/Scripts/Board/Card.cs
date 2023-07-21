using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    MELEE,
    RANGE,
    SPECIAL,
    NULL
}

[System.Serializable]
public class Card
{
    public string name;
}

[System.Serializable]
public class CreatureCard : Card
{
    [SerializeField]
    public Creature_SO cardReference;
    public int currentHP;

    public CreatureCard(Creature_SO card)
    {
        cardReference = card;
        name = cardReference.name;
    }

    public void PlayCard()
    {
        currentHP = cardReference.stats.HP;
    }

    public int GetHPValue()
    {
        return cardReference.GetHP();
    }

    public int GetAttackValue(AttackType type)
    {
        return cardReference.GetAttack(type);
    }

    public int GetDefenseValue(AttackType type)
    {
        return cardReference.GetDefense(type);
    }

    public bool TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            // Handle Death
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class ActionCard : Card
{

}

[System.Serializable]
public class AttackCard : ActionCard
{
    public AttackType type;

    public AttackCard(AttackType attackType)
    {
        type = attackType;
    }

    public void Use(CreatureCard attacker, CreatureCard defender)
    {
        int att = attacker.GetAttackValue(type);
        int def = defender.GetDefenseValue(type);

        int damage = att - def;
        if (damage < 0)
            damage = 0;

        bool defenderIsDead = defender.TakeDamage(damage);

        // TODO: Handle if defender is dead
    }
}
