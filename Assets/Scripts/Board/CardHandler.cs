using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardHandler : MonoBehaviour
{
    public PlayerHandler Owner { get; set; }

    public ColorPalette_SO colorPalette;
    [Header("Transform Dependencies")]
    public UnityEngine.UI.Image cardBackground;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Image heart;
    public TMPro.TMP_Text statText;
    public TMPro.TMP_Text abilityText;
    public TMPro.TMP_Text hpText;

    public Card card;
    private bool testingBool;

    public void SetCard(Card cardData)
    {
        card = cardData;
        image.sprite = card.Card_SO.art;
        heart.gameObject.SetActive(false);

        if (card is CreatureCard cc)
        {
            cardBackground.color = colorPalette.creatureCardColor;
            statText.text = $"<mspace=0.3em> Hp | Att | Def\n{cc.Creature_SO.stats.HP}  |  {cc.Creature_SO.stats.Attack}  |  {cc.Creature_SO.stats.Defense}</mspace>\n------------";
            string aText = "";
            foreach (Ability.Abilities a in cc.Creature_SO.Abilities)
            {
                aText += Ability.GetAbilityDetail(a).Invoke().Title + "\n";
            }
            abilityText.text = aText;
        }
        else if (card is ActionCard ac)
        {
            cardBackground.color = colorPalette.actionCardColor;
            if (ac is AttackCard attCard)
            {
                statText.text = $"Attack\n{attCard.Attack_SO.type} attack";
                abilityText.text = "";
            }
        }
    }

    public void PlayCard()
    {
        if (card is CreatureCard cc)
        {
            cc.PlayCard();
            heart.gameObject.SetActive(true);
            hpText.text = $"<mspace=0.4em>{cc.currentHP}</mspace>";
        }
    }

    void OnMouseDown()
    {
        transform.GetChild(0).DOLocalMove(Vector3.forward * (testingBool ? 0 : 0.25f), 0.2f);
        testingBool = !testingBool;
        if (card is CreatureCard cc)
        {
            PlayCard();
        }
    }
}
