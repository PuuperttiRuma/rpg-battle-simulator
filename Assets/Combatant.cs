using UnityEngine;
using System.Collections;
using System;

public class Combatant {

    public Combatant attackTarget;
    public bool hasBonus = false;
    public string name { get; internal set; }
    public bool isDead { get; internal set; }

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

    public String soak(int damage)
    {
        damage -= armorValue;
        if (damage < 1)
        {
            return "Fortunately for " + name + " her armor is good enough to stop the damage.\n";
        }
        // Check whether there is a way to soak damage without overcompensation.
        for (int c = 0; c < consequences.Length; c++)
        {
            for (int s = 0; s < stress.Length; s++)
            {
                if (damage == stress[s]+consequences[c])
                {
                    String output = name + " soaks the damage with " + stress[s] + " stress and " + consequences[c] + " consequence.\n";
                    stress[s] = 0;
                    consequences[c] = 0;
                    return output;
                }
            }
        }
        // Check whether there is a way to soak damage at all.
        for (int c = 0; c < consequences.Length; c++)
        {
            for (int s = 0; s < stress.Length; s++)
            {
                if (damage <= stress[s] + consequences[c])
                {
                    String output = name + " soaks the damage with " + stress[s] + " stress and " + consequences[c] + " consequence.\n";
                    stress[s] = 0;
                    consequences[c] = 0;
                    return output;
                }
            }
        }
        //Because soaking the damage isn't possible, the defender is taken out.
        isDead = true;
        return name + " can't soak the damage and is taken out!!\n";
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
        string stats = name + " \t\t " + fightAttribute  + " \t " + fightSkill + " \t " + printStress()
            + " \t " + printConsequences() + " \t\t\t " + attackBonus + " \t " + defenceBonus + " \t " + armorValue + " \t " + weaponDamage;
        return stats;
    }

    private String printStress()
    {
        String output = "";
        for (int i = 0; i < stress.Length; i++)
        {
            if (stress[i] == 0)
            {
                output += "[x]";
            }
            else
            {
                output += "[  ]";
            }
        }
        return output;
    }

    private String printConsequences()
    {
        String output = "";
        for (int i = 1; i < consequences.Length; i++)
        {
            if (consequences[i] == 0)
            {
                output += "[x]";
            }
            else
            {
                output += "[" + consequences[i] + "]";
            }
        }
        return output;
    }

}
