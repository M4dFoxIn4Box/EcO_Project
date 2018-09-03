using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Civil_New : MonoBehaviour {

    [Header("Civil Parts")]
    public MeshRenderer[] civilParts;
    public MeshRenderer[] bodyParts;

    [Header("Manikin Parts")]

    public Image[] manikCivilParts;
    public Image[] manikBodyParts;

    public Text[] civilTxt;
    public Text nameTxt;
    public Text skinTxt;
    public Civil_Scriptable civilComponents;

    public bool check = false;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < civilParts.Length; i++)
        {
            civilParts[i].material = civilComponents.colors[i];
        }

        for(int j = 0; j < bodyParts.Length; j++)
        {
            bodyParts[j].material = civilComponents.skinColor;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.CompareTag("Target") && !check)
        {
            TargetChecking();
        }
    }

    void TargetChecking ()
    {
        nameTxt.text = civilComponents.civilname;
        skinTxt.text = civilComponents.skinColor.name + " Skin";

        for (int i = 0; i < civilParts.Length; i++)
        {
            civilTxt[i].text = civilComponents.colors[i].name + " " + civilParts[i].transform.name;
        }

        for (int g = 0; g < manikCivilParts.Length; g++)
        {
            manikCivilParts[g].material = civilComponents.manikColors[g];
        }

        for (int j = 0; j < manikBodyParts.Length; j++)
        {
            manikBodyParts[j].material = civilComponents.manikSkinColor;
        }

        check = !check;
    }
}


