using UnityEngine;
using System.Collections;
using System;

public class Combatant {

    string combatantName;
    int[] stress;
    int[] consequences;
    int fightAttribute;
    int fightSkill;
    int attackBonus;
    int defenseBonus;
    int armorValue;
    int weaponDamage;
    bool hasBonus = false;

    public Combatant(CharacterCreator creator)
    {
        combatantName = creator.combatantNameField.text;
        populateStress((int)Math.Round(creator.stressSlider.value));
        populateConseqs((int)Math.Round(creator.consequencesSlider.value));
        fightAttribute = (int)Math.Round(creator.fightAttributeSlider.value);
        fightSkill = (int)Math.Round(creator.fightSkillSlider.value); ;
        Int32.TryParse(creator.attackBonusField.text, out attackBonus);
        Int32.TryParse(creator.defenseBonusField.text, out defenseBonus);
        Int32.TryParse(creator.armorValueField.text, out armorValue);
        Int32.TryParse(creator.weaponDamageField.text, out weaponDamage);
    }

    public Combatant(string combatantName, int stress, int conseqs, int fightAttribute, int fightSkill)
    {
        this.combatantName = combatantName;
        populateStress(stress);
        populateConseqs(conseqs);
        this.fightAttribute = fightAttribute;
        this.fightSkill = fightSkill;
    }

    void populateStress(int length)
    {
        stress = new int[length];
        for (int i = 0; i < stress.Length; i++)
        {
            stress[i] = i + 1;
        }
    }

    void populateConseqs(int length)
    {
        consequences = new int[length/2+1];
        for (int i = 0; i < consequences.Length; i++)
        {
            consequences[i] = i * 2;
        }
    }

    public String printStats()
    {
        string stats = combatantName + "\n" + fightAttribute  + "\n" + fightSkill + "\n" + stress.Length
            + "\n" + (consequences.Length-1)*2 + "\n" + attackBonus + "\n" + defenseBonus + "\n" + armorValue + "\n" + weaponDamage;
        return stats;
    }

}
