using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Death_Trigger : MonoBehaviour {

    public GameObject deathcam;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            deathcam.SetActive(true);
            Level_Manager.instance.Death();
        }
    }
}
