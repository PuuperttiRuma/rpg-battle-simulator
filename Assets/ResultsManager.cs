using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultsManager
{
    Text textOutput;
    List<Combatant> combatants;
    int roundNumber;

    public ResultsManager(Text textWindow, List<Combatant> combatants, int roundNumber)
    {
        textOutput = textWindow;
        this.combatants = combatants;

    }

    void showResults()
    {
        textOutput.text = "";
        textOutput.text += "Battles fought: " + roundNumber;
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

}
