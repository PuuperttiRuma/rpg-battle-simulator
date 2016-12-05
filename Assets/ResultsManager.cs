using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultsManager
{
    class Result
    {
        string winner;
        int round;
        string health;

        public Result(string winner, int round, string health)
        {
            this.winner = winner;
            this.round = round;
            this.health = health;
        }
    }

    List<Result> results = new List<Result>();
    Text textOutput;


    public ResultsManager(Text textWindow)
    {
        textOutput = textWindow;
    }

    public void addResult(string winner, int round, string health)
    {
        results.Add(new Result(winner, round, health));
    }

    void showResults()
    {
        textOutput.text = "";
        textOutput.text += "Battles fought: " + results.Count;
        textOutput.text += 
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
