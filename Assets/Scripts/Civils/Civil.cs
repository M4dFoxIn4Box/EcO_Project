using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Civil : MonoBehaviour {

    public MeshRenderer[] clotheParts;
    public MeshRenderer[] bodyParts;

    private int bodyPartsIdx;
    public int[] clothePartsIdx;
    public string[] clothePartsStg;
    private string bodyPartsStg;
    public Text[] clotheTargetInfos;
    public Text bodyTargetInfos;
    private bool check = false;

    [Header("Intruder Color List")]

    public Material[] skinColorList;
    public Material[] clotheColorList;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < clotheParts.Length; i++)
        {
            clothePartsIdx[i] = Random.Range(0, clotheColorList.Length);
            clotheParts[i].material = clotheColorList[clothePartsIdx[i]];
            clothePartsStg[i] = clotheColorList[clothePartsIdx[i]].name + " " + clotheParts[i].transform.name.ToString() ; 
        }

        bodyPartsIdx = Random.Range(0, skinColorList.Length);

        for (int g = 0; g < bodyParts.Length; g++)
        {
            bodyParts[g].material = skinColorList[bodyPartsIdx];
            bodyPartsStg = skinColorList[bodyPartsIdx].name;
        }

    }
	
	void Update ()
    {
        if (gameObject.CompareTag("Target") && !check)
        {
            for (int j = 0; j < clotheTargetInfos.Length; j++)
            {
                clotheTargetInfos[j].text = clothePartsStg[j];
            }

            bodyTargetInfos.text = bodyPartsStg;
            check = !check;
            Debug.Log("");          
        }
    }

}
