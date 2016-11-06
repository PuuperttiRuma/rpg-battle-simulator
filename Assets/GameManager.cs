using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Text resultsText;
    public Text statsText;
    public GameObject popupMenu;

    List<Combatant> combatants = new List<Combatant>();
    CharacterCreator newCombatant;
    int[] rolls = new int[6];

    void Awake()
    {
        newCombatant = GetComponent<CharacterCreator>();
    }

    public void CreateCombatant()
    {
        newCombatant.updateTexts();
        newCombatant.checkFields();
        popupMenu.SetActive(true);
        //Combatant dude = new Combatant("Dude", 3, 4, 8, 2);
    }

    public void disablePopupMenu()
    {
        combatants.Add(new Combatant(newCombatant));
        popupMenu.SetActive(false);
        showStats();
    }

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
        statsText.text = combatants[0].printStats();
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
