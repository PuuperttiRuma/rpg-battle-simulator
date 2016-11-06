using UnityEngine;
using System.Collections;

public class Roller {

    /// <summary>
    /// Roll several dice and count how many of them succeeds, ie. is above a threshold value.
    /// </summary>
    /// <param name="amount">How many dice are rolled</param>
    /// <param name="dice">Size of the dice, eg. 6 means d6</param>
    /// <param name="threshold">Above what number roll is seen as a success. Eg. 4 means 4 and all above succeed</param>
    /// <returns></returns>
    public static int rollPool(int amount, int dice, int threshold)
    {
        int result = 0;
        for (int i = 0; i < amount; i++)
        {
            int roll = Random.Range(1, dice + 1);
            if (roll >= threshold) result++;
        }
        return result;
    }

    /// <summary>
    /// Throw a single die
    /// </summary>
    /// <param name="dice">Size of the die, eg. 6 means d6</param>
    /// <returns></returns>
    public static int roll(int dice)
    {
        return Random.Range(1, dice + 1);
    }
	

}
