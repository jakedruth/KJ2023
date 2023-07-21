using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    BASE,
    MELEE,
    RANGE,
    SPECIAL,
    NULL
}

[System.Serializable]
public class Card
{
    public string name;
    public Card_SO Card_SO;

    public Card(Card_SO card)
    {
        Card_SO = card;
        name = card.name;
    }
}

[System.Serializable]
public class CreatureCard : Card
{
    public Creature_SO Creature_SO { get { return Card_SO as Creature_SO; } }

    public int currentHP;

    public CreatureCard(Creature_SO card) : base(card)
    { }

    public void PlayCard()
    {
        currentHP = Creature_SO.GetHP();
    }

    public int GetHPValue()
    {
        return Creature_SO.GetHP();
    }

    public int GetAttackValue(AttackType type)
    {
        return Creature_SO.GetAttack(type);
    }

    public int GetDefenseValue(AttackType type)
    {
        return Creature_SO.GetDefense(type);
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
    public ActionCard(Card_SO card) : base(card)
    { }
}

[System.Serializable]
public class AttackCard : ActionCard
{
    public Attack_SO Attack_SO { get { return Card_SO as Attack_SO; } }

    public AttackCard(Attack_SO card) : base(card)
    { }

    public void Use(CreatureCard attacker, CreatureCard defender)
    {
        int att = attacker.GetAttackValue(Attack_SO.type);
        int def = defender.GetDefenseValue(Attack_SO.type);

        int damage = att - def;
        if (damage <= 0)
            damage = 1;

        bool defenderIsDead = defender.TakeDamage(damage);

        // TODO: Handle if defender is dead
    }
}
