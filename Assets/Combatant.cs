using UnityEngine;
using System.Collections;
using System;

public class Combatant {

    public Combatant attackTarget;
    public bool hasBonus = false;
    public string name { get; internal set; }

    int[] stress;
    int[] consequences;
    int fightAttribute;
    int fightSkill;
    int attackBonus;
    int defenceBonus;
    int armorValue;
    int weaponDamage;

    public Combatant(CharacterCreator creator)
    {
        name = creator.combatantNameField.text;
        populateStress((int)Math.Round(creator.stressSlider.value));
        populateConseqs((int)Math.Round(creator.consequencesSlider.value));
        fightAttribute = (int)Math.Round(creator.fightAttributeSlider.value);
        fightSkill = (int)Math.Round(creator.fightSkillSlider.value); ;
        Int32.TryParse(creator.attackBonusField.text, out attackBonus);
        Int32.TryParse(creator.defenseBonusField.text, out defenceBonus);
        Int32.TryParse(creator.armorValueField.text, out armorValue);
        Int32.TryParse(creator.weaponDamageField.text, out weaponDamage);
    }

    public Combatant(string combatantName, int stress, int conseqs, int fightAttribute, int fightSkill)
    {
        this.name = combatantName;
        populateStress(stress);
        populateConseqs(conseqs);
        this.fightAttribute = fightAttribute;
        this.fightSkill = fightSkill;
    }

    public int attack()
    {
        int toHit = 0;
        toHit += Roller.rollPool(fightAttribute, 6, 5);
        toHit += Roller.rollPool(fightSkill, 6, 3);
        if (hasBonus)
        {
            toHit += 2;
            hasBonus = false;
        }
        toHit += attackBonus;
        return toHit;
    }

    public int defend()
    {
        int defence = 0;
        defence += Roller.rollPool(fightAttribute, 6, 5);
        defence += Roller.rollPool(fightSkill, 6, 3);
        if (hasBonus)
        {
            defence += 2;
            hasBonus = false;
        }
        defence += defenceBonus;
        return defence;
    }

    public void soak(int damage)
    {
    }

    public int damage(int rawdamage)
    {
        return rawdamage + weaponDamage;
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
        string stats = name + " \t  " + fightAttribute  + " \t  " + fightSkill + " \t  " + stress.Length
            + " \t  " + (consequences.Length-1)*2 + " \t  " + attackBonus + " \t  " + defenceBonus + " \t  " + armorValue + " \t  " + weaponDamage;
        return stats;
    }

}
