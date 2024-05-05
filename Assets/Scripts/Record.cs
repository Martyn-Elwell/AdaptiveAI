using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : ScriptableObject
{
    public int[] initialAttack = new int[6];
    public int[,] attackTable = new int[6, 6];
}

