using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultsManager
{
    Text textOutput;

    public ResultsManager(Text textWindow)
    {
        textOutput = textWindow;
    }

    /*
    public void addResult(string winner, int round, string health)
    {

        winners.Add(new Winner(winner, round, health));
    }

    void showResults()
    {
        textOutput.text = "";
        textOutput.text += "Battles fought: " + results.Count;
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
    */

}
