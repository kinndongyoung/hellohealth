using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzlePattern
{
    public List<int> getPatternByStage(List<PuzzleParts> PuzzleList, int nStage) {
        List<int> returnValue = new List<int>();
        List<int> numberList = new List<int>();
        for (int i = 0; i < PuzzleList.Count; i++)
        {
            var puzzle = PuzzleList[i];
            if (puzzle.BeaconID >= 0 && puzzle.Contain == true)
                numberList.Add(i);
        }
        // Sorting Numbers.
        for (int i = 0; i < numberList.Count; i++)
        {
            int temp = numberList[i];
            int randomIndex = Random.Range(i, numberList.Count);
            numberList[i] = numberList[randomIndex];
            numberList[randomIndex] = temp;
        }

        if(nStage == 1)
        {
            returnValue.Add(numberList[0]);
            returnValue.Add(numberList[1]);
            returnValue.Add(numberList[2]);
        }
        else if(nStage == 2)
        {
            returnValue.Add(numberList[0]);
            returnValue.Add(numberList[1]);
            returnValue.Add(numberList[2]);
            returnValue.Add(numberList[3]);
            returnValue.Add(numberList[4]);
        }
        else if(nStage == 3)
        {
            returnValue.Add(numberList[0]);
            returnValue.Add(numberList[1]);
            returnValue.Add(numberList[2]);
            returnValue.Add(numberList[3]);
            returnValue.Add(numberList[4]);
            returnValue.Add(numberList[5]);
            returnValue.Add(numberList[6]);
        }

        return returnValue;
    }
}
