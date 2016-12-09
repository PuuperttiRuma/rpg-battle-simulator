using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Combatant
{

    public Combatant attackTarget;
    public bool hasBoost = false;
    public bool gotBoostAsAttacker = false;
    public string name { get; internal set; }
    public bool isDead { get; internal set; }
    public int wins { get; internal set; }

    Dictionary<int, int> winRounds = new Dictionary<int, int>();
    Dictionary<string, int> winHealths = new Dictionary<string, int>();

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
        isDead = false;
    }

    public Combatant(string combatantName, int fightAttribute, int fightSkill, int stress, int conseqs, int weaponDamage, int armorValue)
    {
        this.name = combatantName;
        populateStress(stress);
        populateConseqs(conseqs);
        this.fightAttribute = fightAttribute;
        this.fightSkill = fightSkill;
        this.armorValue = armorValue;
        this.weaponDamage = weaponDamage;
        isDead = false;
    }

    public int attack()
    {
        int toHit = 0;
        toHit += Roller.rollPool(fightAttribute, 6, 5);
        toHit += Roller.rollPool(fightSkill, 6, 3);
        toHit += attackBonus;
        return toHit;
    }

    public int defend()
    {
        int defence = 0;
        defence += Roller.rollPool(fightAttribute, 6, 5);
        defence += Roller.rollPool(fightSkill, 6, 3);
        defence += defenceBonus;
        return defence;
    }

    public int damage(int rawdamage, bool weaponsAreDice)
    {
        if (weaponsAreDice)
        {
            return rawdamage + Roller.rollPool(weaponDamage, 6, 4);
        }
        return rawdamage + weaponDamage;
    }

    public String soak(int damage)
    {
        String output = "";
        if (damage <= armorValue)
        {
            return "Fortunately for " + name + " her armor is good enough to stop the damage.\n";
        }
        if (armorValue > 0)
        {
            damage -= armorValue;
            output = name + "'s armor negates " + armorValue + " shifts of damage.\n";
        }
        // Check whether there is a way to soak damage without overcompensation.
        for (int c = 0; c < consequences.Length; c++)
        {
            for (int s = 0; s < stress.Length; s++)
            {
                if (damage == stress[s] + consequences[c])
                {
                    output += name + " soaks the damage with " + stress[s] + " stress and " + consequences[c] + " consequence.\n";
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
                    output = name + " soaks the damage with " + stress[s] + " stress and " + consequences[c] + " consequence.\n";
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

    internal void resetHealth()
    {
        populateStress(stress.Length);
        populateConseqs((consequences.Length - 1) * 2);
        isDead = false;
    }

    public void win(int round)
    {
        //Update win count
        wins++;

        //Update winning round numbers
        if (winRounds.ContainsKey(round))
        {
            winRounds[round]++;
        }
        else
        {
            winRounds.Add(round, 1);
        }

        //Update winning health stats
        if (winHealths.ContainsKey(printConsequences()))
        {
            winHealths[printConsequences()]++;
        }
        else
        {
            winHealths.Add(printConsequences(), 1);
        }

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
        consequences = new int[length / 2 + 1];
        for (int i = 0; i < consequences.Length; i++)
        {
            consequences[i] = i * 2;
        }
    }

    public String printStats()
    {
        string stats = name + " \t" + fightAttribute + " \t" + fightSkill + " \t" + printStress()
            + " \t" + printConsequences() + " \t" + attackBonus + " \t" + defenceBonus + " \t" + armorValue + " \t" + weaponDamage;
        return stats;
    }

    public String printStress()
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
                output += "[ ]";
            }
        }
        return output;
    }

    public String printConsequences()
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

    internal string printResults(int combatCount)
    {
        string result = "";
        
        result += name + " won: \t" + wins + " \t" + Math.Round(wins / (double)combatCount * 100, 5) + "%\n";

        var list = winRounds.Keys.ToList();
        list.Sort();
        result += name + " won on rounds:\n";
        foreach (var key in list)
        {
            result += key + ": \t" + winRounds[key]+ " \t" + Math.Round(winRounds[key] / (double)wins * 100, 2) + "%\n";
        }       

        var list2 = winHealths.Keys.ToList();
        list2.Sort();
        result += name + " won with health:\n";
        foreach (var key2 in list2)
        {
            result += key2 + ":\t" + winHealths[key2] + " \t" + Math.Round(winHealths[key2] / (double)wins * 100, 2) + "%\n";
        }
        return result;
    }
}
