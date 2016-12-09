using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text combatLogText;
    public Text resultsLogText;
    public Text statsText;
    public GameObject popupMenu;
    public ScrollRect logScroll;
    public Text autoCombatButtonText;
    public InputField AutoCombatIncrements;

    List<Combatant> combatants = new List<Combatant>();
    CharacterCreator characterCreator;
    Combatant activeCombatant;
    Combatant nextCombatant;
    int roundNumber = 0;
    int combatCount = 0;
    int roundStartIndex = 0;
    bool autoCombatToggle = false;
    int ACIncrement = 100;

    void Awake()
    {
        characterCreator = GetComponent<CharacterCreator>();
        //resultsManager = new ResultsManager(resultsLogText, combatants, roundNumber);
        debugCombatants();
    }

    private void debugCombatants()
    {
        combatants.Add(new Combatant("Annie", 8, 2, 2, 4, 0, 0));
        combatants.Add(new Combatant("Belle", 8, 2, 2, 4, 0, 0));
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

    void setNextCombatant(bool printToCombatLog)
    {
        int i = combatants.IndexOf(activeCombatant);
        if (i == roundStartIndex)
        {
            roundNumber++;
            if (printToCombatLog) combatLogText.text += "ROUND " + roundNumber + "!\n";
        }
        if (i + 1 < combatants.Count)
        {
            nextCombatant = combatants[i + 1];
        }
        else
        {
            nextCombatant = combatants[0];
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
        setNextCombatant(printToCombatLog);
        if (activeCombatant.attackTarget == null)
        {
            activeCombatant.attackTarget = nextCombatant;
        }
        Combatant attacker = activeCombatant;
        Combatant defender = activeCombatant.attackTarget;
        int toHit = attacker.attack();
        int defence = defender.defend();        



        String hitEffects = effectuateAttack(toHit - defence, attacker, defender);
        if (defender.isDead) endCombat(attacker, printToCombatLog);
        if (printToCombatLog) printCombatLog(attacker.name, defender.name, toHit, defence, hitEffects);        

        activeCombatant = nextCombatant;
    }

    private string effectuateAttack(int rollResult, Combatant attacker, Combatant defender)
    {
        String boost = "";
        if (attacker.gotBoostAsAttacker && attacker.hasBoost)
        {
            boost = attacker.name + " uses boost!\n";
            rollResult += 2;
            attacker.hasBoost = false;
        } else if (attacker.hasBoost && rollResult >= -2)
        {
            boost = attacker.name + " uses boost!\n";
            rollResult += 2;
            attacker.hasBoost = false;
        }
        if (defender.hasBoost && !defender.gotBoostAsAttacker)
        {
            boost = defender.name + " uses boost!\n";
            rollResult += -2;
            defender.hasBoost = false;
        } else if (defender.hasBoost && rollResult >= 0)
        {
            boost = defender.name + " uses boost!\n";
            rollResult += -2;
            defender.hasBoost = false;
        }


        int damage = attacker.damage(rollResult);
        if (rollResult < -2)
        {
            defender.hasBoost = true;
            defender.gotBoostAsAttacker = false;
            return boost + defender.name + " defends with such style that she gets a boost!\n";
        }
        else if (rollResult < 0)
        {
            return boost + defender.name + " defends.\n";
        }
        else if (rollResult == 0)
        {
            attacker.hasBoost = true;
            attacker.gotBoostAsAttacker = true;
            return boost + defender.name + " succeeds in her defence, but just barely. " + attacker.name + " gets a boost!\n";
        }
        else if (rollResult > 2)
        {
            return boost + "Beautiful hit! " + attacker.name + " hits with style and graze and deals " + damage + " shifts of damage to " + defender.name + "!\n" + defender.soak(damage);
        }
        else
        {
            return boost + "Hit! " + attacker.name + " hits and deals " + damage + " shifts of damage to " + defender.name + "!\n" + defender.soak(damage);
        }
    }

    public void doNextRound(bool printToCombatLog)
    {
        do
        {
            doNextTurn(printToCombatLog);
        } while (combatants.IndexOf(activeCombatant) != roundStartIndex);
    }

    public void doNextCombat()
    {
        int combatCountNow = combatCount;
        while (combatCountNow == combatCount)
        {
            doNextRound(false);
        }
        showResults();
    }

    public void do100Combat()
    {
        for (int i = 0; i < 100; i++)
        {
            int combatCountNow = combatCount;
            while (combatCountNow == combatCount)
            {
                doNextRound(false);
            }
        }
        showResults();
    }

    public void autoResolveCombats()
    {
        if (!autoCombatToggle)
        {
            autoCombatToggle = true;
            autoCombatButtonText.text = "Stop Auto Combat";
            StartCoroutine(autoCombat());            
        }
        else
        {
            //StopCoroutine(autoCombat());
            autoCombatButtonText.text = "Start Auto Combat";
            autoCombatToggle = false;
        }
    }

    public void writeResultsToFile()
    {
        String filename = "";
        foreach (Combatant i in combatants)
        {
            filename += i.name;
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename + ".txt", true)) // "D:\Sälää\ropejuttuja\" + 
        {
            foreach (Combatant i in combatants)
            {
                file.Write("Name\t At\t Sk\t Stress\t Conseqs\t A+\t D+\t AV\t W\t\r\n");
                file.Write(i.printStats().Replace("\n","\r\n"));
                file.Write("\r\nBattles fought:\t" + combatCount + "\r\n");
                file.Write(i.printResults(combatCount).Replace("\n", "\r\n"));
                file.Write("\r\n");
            }

        }
    }

    IEnumerator autoCombat()
    {

        while(autoCombatToggle)
        {
            for (int i = 0; i < ACIncrement; i++)
            {
                doNextCombat();
            }            
            showResults();
            yield return null;
        }
        
    }

    public void checkACIncrement()
    {
        int value;
        if (Int32.TryParse(AutoCombatIncrements.text, out value))
        {
            if (value >= 500) value = 500;
            ACIncrement = value;
            //warningText.text = "";
            //createButton.interactable = true;
        }
        else
        {
            //warningText.text = "Modifiers can only be whole numbers.";
            //createButton.interactable = false;
            ACIncrement = 1;
            return;
        }
    }

    void endCombat(Combatant winner, bool printToCombatLog)
    {
        //Mark down the results
        combatCount++;
        winner.win(roundNumber);
        if (printToCombatLog) showResults();

        //Reset combatants and turn counters etc.
        roundNumber = 0;
        roundStartIndex = combatCount % 2;
        nextCombatant = combatants[roundStartIndex];
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

    void showStats()
    {
        statsText.text = "";
        foreach (Combatant i in combatants)
        {
            statsText.text += i.printStats() + "\n";
        }
    }

    void showResults()
    {
        combatLogText.text = "Results:\n";

        combatLogText.text += "Battles fought: " + combatCount + "\n";

        foreach (Combatant i in combatants)
        {
            combatLogText.text += i.printResults(combatCount);            
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