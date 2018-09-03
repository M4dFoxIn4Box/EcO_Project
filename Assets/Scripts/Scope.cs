using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scope : MonoBehaviour
{
    public Animator scopeAnimator;
    public AudioSource charlieSnd;
    public GameObject scopeImg;
    public GameObject weaponCam;
    public GameObject crossHair;
    public Camera mainCamera;

    [Header("Audios")]
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;

    [Header("Weapon Settings")]
    public float impactForce;
    private float nextTimeToShoot;
    public float scopeFOV = 15f;
    public GameObject impactVFX;

    private int shotValue = 1;
    private float normalFOV;
    private bool isScoped = false;

    public void Start()
    {
        
        normalFOV = mainCamera.fieldOfView;
      
    }

    public void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            crossHair.transform.localScale -= new Vector3(transform.position.x / 2, transform.position.y / 2, transform.position.z / 2);
            StartCoroutine("Shoot");
            //muzzleFlash.Play();
            

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {

                Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green, 100f);
                Debug.Log("Did Hit");

                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("You Win");
                    charlieSnd.PlayOneShot(correctAnswer, 1f);
                    Destroy(hit.collider);
                    Destroy(hit.transform.gameObject, 3f);
                    gameObject.GetComponent<Scope>().enabled = false;
                }
                else if(hit.collider.CompareTag("Civil"))
                {
                    Level_Manager.instance.ShotsChallenge(shotValue);
                    Debug.Log("You Lose");
                    charlieSnd.PlayOneShot(wrongAnswer, 1f);
                    //Destroy(hit.collider);
                    //Destroy(hit.transform.gameObject, 3f);
                    //gameObject.GetComponent<Scope>().enabled = false;

                }

                if (hit.collider.CompareTag("Secret_Target"))
                {
                    Level_Manager.instance.TargetsChallenge(1);
                    Destroy(hit.collider.gameObject);
                }

                //GameObject impactGO = Instantiate(impactVFX, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(impactGO, 2f);

            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red, 100f);
                Debug.Log("Did not Hit");
            }
        }

        if (Input.GetButtonDown("Zoom"))
        {
            crossHair.SetActive(true);
            isScoped = !isScoped;
            scopeAnimator.SetBool("IsScoped", isScoped);
            StartCoroutine("OnScoped");
        }

        if(Input.GetButtonUp("Zoom") && isScoped)
        {

            isScoped = !isScoped;
            scopeAnimator.SetBool("IsScoped", isScoped);
            StartCoroutine("OnUnScoped");
            crossHair.SetActive(false);
        }
    }


    //Fire rate system
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.15f);
        scopeAnimator.SetBool("CanShoot", false);
        crossHair.transform.localScale += new Vector3(transform.position.x / 2, transform.position.y / 2, transform.position.z / 2);

        yield return new WaitForSeconds(2f);


    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);

        
        mainCamera.fieldOfView = scopeFOV;
        weaponCam.SetActive(false);
        scopeImg.SetActive(true);
    }

   IEnumerator OnUnScoped()
    {
        yield return new WaitForSeconds(0.15f);

        mainCamera.fieldOfView = normalFOV;
        weaponCam.SetActive(true);
        scopeImg.SetActive(false);
    }


}
