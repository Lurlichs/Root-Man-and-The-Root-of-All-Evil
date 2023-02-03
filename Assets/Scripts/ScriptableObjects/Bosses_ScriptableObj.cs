using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Scriptable Objects/New Boss")]
public class Bosses_ScriptableObj : ScriptableObject
{
    public int id;
    public string bossName;
    public float baseHealth;
 
}
