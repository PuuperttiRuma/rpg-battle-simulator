using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public Text resultsText;
    public Text statsText;
    public GameObject popupMenu;
    public Scrollbar scrollbar;

    List<Combatant> combatants = new List<Combatant>();
    CharacterCreator characterCreator;
    int[] rolls = new int[6];
    Combatant activeCombatant;
    Combatant nextCombatant;
    int roundCounter = 1;

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
        if (i == 0)
        {
            resultsText.text += "Round number " + roundCounter + " starts.\n";
        }

        if (i + 1 < combatants.Count)
        {
            nextCombatant = combatants[i + 1];
        }
        else
        {
            nextCombatant = combatants[0];
            roundCounter++;
        }
    }

    //TODO: varmista että voi hyökätä vain jos väh. 2 taistelijaa!
    //TODO defender käyttää bonuksen vain jos on tulossa osuma
    public void doNextTurn(bool printToCombatLog)
    {
        setNextCombatant();
        if (activeCombatant.attackTarget == null)
        {
            activeCombatant.attackTarget = nextCombatant;
        }
        Combatant attacker = activeCombatant;
        Combatant defender = activeCombatant.attackTarget;
        int toHit = attacker.attack();
        int defence = defender.defend();

        attacker.hasBonus = false;
        defender.hasBonus = false;

        String hitEffects = effectuateAttack(toHit - defence, attacker, defender);

        if (printToCombatLog)
        {
            resultsText.text += attacker.name + " rolls " + toHit + " for attack.\n";
            resultsText.text += defender.name + " rolls " + defence + " for defence.\n";
            resultsText.text += hitEffects;
        }
        activeCombatant = nextCombatant;
    }

    public void scrollDownLog()
    {
        Canvas.ForceUpdateCanvases();
        scrollbar.value = 0;
    }

    public void doNextRound(bool printToCombatLog)
    {
        int roundNumberNow = roundCounter;
        while (roundCounter == roundNumberNow)
        {
            doNextTurn(printToCombatLog);
        }
    }

    private String effectuateAttack(int rollResult, Combatant attacker, Combatant defender)
    {

        int damage = attacker.damage(rollResult);

        if (rollResult < -2)
        {
            defender.hasBonus = true;
            return defender.name + " defends with such style that she gets a boost!\n";
        }
        else if (rollResult < 0)
        {
            return defender.name + " defends.\n";
        }
        else if (rollResult == 0)
        {
            attacker.hasBonus = true;
            return defender.name + " succeeds in her defence, but just barely. " + attacker.name + " gets a boost!\n";
        }
        else if (rollResult > 2)
        {
            defender.soak(damage);
            return "Beautiful hit! " + attacker.name + " hits with style and graze and deals " + damage + " shifts of damage to " + defender.name + "!\n";
        }
        else
        {
            defender.soak(damage);
            return "Hit! " + attacker.name + " hits and deals " + damage + " shifts of damage to " + defender.name + "!\n";
        }
    }

    void showStats()
    {
        statsText.text = "Name \t\t AT \t Sk \t St \t C \t A+ \t D+ \t AV \t W\n";
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
        if (rollsSummed > 1000)
        {
            rollsSummed /= 1000;
            rollsText = rollsSummed + "K";
        }

        resultsText.text += "\nRolled the dice " + rollsText + " times.";
    }

    //Debugging method, not in use anymore
    int sumOfArray(int[] array)
    {
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
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

}
