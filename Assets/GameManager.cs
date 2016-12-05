using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Text combatLogText;
    public Text resultsLogText;
    public Text statsText;
    public GameObject popupMenu;
    public ScrollRect logScroll;

    List<Combatant> combatants = new List<Combatant>();
    CharacterCreator characterCreator;
    ResultsManager resultsManager;
    Combatant activeCombatant;
    Combatant nextCombatant;
    int roundNumber = 1;
    int combatCount = 0;


    void Awake()
    {
        characterCreator = GetComponent<CharacterCreator>();
        resultsManager = new ResultsManager(resultsLogText);
        debugCombatants();
    }

    private void debugCombatants()
    {
        combatants.Add(new Combatant("Annie", 8, 2, 2, 6, 0, 0));
        combatants.Add(new Combatant("Belle", 8, 2, 4, 4, 0, 0));
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
            combatLogText.text += "ROUND " + roundNumber + "!\n";
        }

        if (i + 1 < combatants.Count)
        {
            nextCombatant = combatants[i + 1];
        }
        else
        {
            nextCombatant = combatants[0];
            roundNumber++;
        }
    }

    public void scrollDownLog()
    {
        Canvas.ForceUpdateCanvases();
        logScroll.verticalNormalizedPosition = 0.0f;
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
        if (defender.isDead) endCombat(attacker);
        if (printToCombatLog) printCombatLog(attacker.name, defender.name, toHit, defence, hitEffects);        

        activeCombatant = nextCombatant;
    }

    void endCombat(Combatant winner)
    {
        //Mark down the results
        winner.win(combatCount);

        //Reset combatants and turn counters etc.
        combatCount++;
        roundNumber = 0;
        nextCombatant = combatants[0];
        foreach (Combatant combatant in combatants)
        {
            combatant.resetHealth();
        }

    }

    void printCombatLog(String attackerName, String defenderName, int toHit, int defence, String hitEffects)
    {
        combatLogText.text += attackerName + " rolls " + toHit + " for attack.\n";
        combatLogText.text += defenderName + " rolls " + defence + " for defence.\n";
        combatLogText.text += hitEffects;
        showStats();
        scrollDownLog();
    }
    
    public void doNextRound(bool printToCombatLog)
    {
        int roundNumberNow = roundNumber;
        while (roundNumber == roundNumberNow)
        {
            doNextTurn(printToCombatLog);
        }
    }

    private string effectuateAttack(int rollResult, Combatant attacker, Combatant defender)
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
            return "Beautiful hit! " + attacker.name + " hits with style and graze and deals " + damage + " shifts of damage to " + defender.name + "!\n" + defender.soak(damage);
        }
        else
        {
            return "Hit! " + attacker.name + " hits and deals " + damage + " shifts of damage to " + defender.name + "!\n" + defender.soak(damage);
        }
    }

    void showStats()
    {
        statsText.text = "";
        foreach (Combatant i in combatants)
        {
            statsText.text += i.printStats() + "\n";
        }
    }

    /*Debugging method, not in use anymore
    public void rollDie(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            rolls[Roller.roll(6) - 1]++;
        }
        showResults();
    }*/

}