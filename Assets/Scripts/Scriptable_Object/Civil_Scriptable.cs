using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Civil", menuName = "ScriptableObject/Civil", order = 1)]
public class Civil_Scriptable : ScriptableObject
{
    public string civilname;
    [Header("Civil Colors")]
    public Material[] colors;
    public Material skinColor;

    [Header("Manikin Colors")]
    public Material[] manikColors;
    public Material manikSkinColor;


}


