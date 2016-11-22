using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Text resultsText;
    public Text statsText;
    public GameObject popupMenu;
    public Scrollbar scrollbar;

    List<Combatant> combatants = new List<Combatant>();
    CharacterCreator characterCreator;
    int[] rolls = new int[6];
    Combatant activeCombatant;
    Combatant nextCombatant;


    void Awake()
    {
        characterCreator = GetComponent<CharacterCreator>();
        debugCombatants();
    }

    private void debugCombatants()
    {
        combatants.Add(new Combatant("Annie", 3, 4, 8, 2));
        combatants.Add(new Combatant("Belle", 3, 4, 8, 2));
        activeCombatant = combatants[0];
        showStats();
    }

    public void CreateCombatant()
    {
        characterCreator.updateTexts();
        characterCreator.checkFields();
        popupMenu.SetActive(true);
        //Combatant dude = new Combatant("Dude", 3, 4, 8, 2);
    }

    public void disablePopupMenu()
    {
        combatants.Add(new Combatant(characterCreator));
        activeCombatant = combatants[0];
        popupMenu.SetActive(false);
        showStats();
    }

    void setNextCombatant()
    {
        int i = combatants.IndexOf(activeCombatant);
        if (i+1 < combatants.Count)
        {
            nextCombatant = combatants[i+1];
        }
        else
        {
            nextCombatant = combatants[0];
        }
    }

    public void doNextTurn()
    {
        //TODO: varmista että voi hyökätä vain jos väh. 2 taistelijaa!

        setNextCombatant();
        if (activeCombatant.attackTarget == null)
        {
            activeCombatant.attackTarget = nextCombatant;
        }
        Combatant attacker = activeCombatant;
        Combatant defender = activeCombatant.attackTarget;
        int toHit = attacker.attack();
        int defence = defender.defend();

        //TODO defender käyttää bonuksen vain jos on tulossa osuma
        int result = toHit - defence;
        attacker.hasBonus = false;
        defender.hasBonus = false;

        resultsText.text += attacker.name + " rolls " + toHit + " for attack.\n";        
        resultsText.text += defender.name + " rolls " + defence + " for defence.\n";

        if (result < -2)
        {
            defender.hasBonus = true;
            resultsText.text += defender.name + " succeeds in her defence with style and gets a boost!\n";
        }
        else if (result < 0)
        {
            resultsText.text += defender.name + " succeeds in her defence.\n";
        }
        else if (result == 0)
        {
            attacker.hasBonus = true;
            resultsText.text += defender.name + " succeeds in her defence, but just barely. " + attacker.name + " gets a boost!\n";
        }
        else if (result > 0)
        {
            int damage = (attacker.damage(result));
            
            if (result > 2)
            {
                attacker.hasBonus = true;
                resultsText.text += attacker.name + " succeeds with style in her attack and gets a boost!\n";
            }
            else
            {
                resultsText.text += attacker.name + " succeeds in her attack!\n";
            }
            //defender.soak(damage);
            resultsText.text += defender.name + " suffers " + damage + " shifts of damage! Ouch!\n";
        }
        Canvas.ForceUpdateCanvases();
        scrollbar.value = 0;
        activeCombatant = nextCombatant;
    }

    //Debugging method, not in use anymore
    public void rollDie(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            rolls[Roller.roll(6) - 1]++;
        }
        showResults();
    }

    int sumOfArray(int[] array)
    {
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }

    void showStats()
    {
        statsText.text = "Name \t AT \t Sk \t St \t C \t A+ \t D+ \t AV \t W\n";
        foreach (Combatant i in combatants)
        {
            statsText.text += i.printStats() + "\n";
        }
    }

    void showResults()
    {
        resultsText.text = "";
        int rollsSummed = sumOfArray(rolls);
        for (int i = 0; i < rolls.Length; i++)
        {
            float result = (float)rolls[i] / (float)rollsSummed;
            resultsText.text += i + 1 + ": " + result.ToString("P") + "\n";
        }

        String rollsText = rollsSummed.ToString();
        if (rollsSummed > 1000000)
        {
            rollsSummed /= 1000000;
            rollsText = rollsSummed + "M";
        }
        if (rollsSummed > 1000) {
            rollsSummed /= 1000;
            rollsText = rollsSummed + "K";
        }
        
        resultsText.text += "\nRolled the dice " + rollsText + " times.";
    }

}
