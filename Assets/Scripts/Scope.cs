using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scope : MonoBehaviour
{
    public Animator scopeAnimator;
    public AudioSource sniperSnd;
    public AudioClip shootSnd;
    public GameObject scopeImg;
    public GameObject weaponCam;
    public Camera mainCamera;

    [Header("Weapon Settings")]
    public float impactForce;
    private float nextTimeToShoot;
    public ParticleSystem muzzleFlash;
    public float scopeFOV = 15f;
    public GameObject impactVFX;

    private int shotValue = 1;
    private bool canShoot = true;
    private float normalFOV;
    private bool isScoped = false;

    public void Start()
    {
        
        normalFOV = mainCamera.fieldOfView;
      
    }

    public void Update()
    {

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Level_Manager.instance.ShotsChallenge(shotValue);
            scopeAnimator.SetBool("CanShoot", canShoot);
            canShoot = !canShoot;
            StartCoroutine("Shoot");
            sniperSnd.Play();
            muzzleFlash.Play();
            

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {

                Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green, 100f);
                Debug.Log("Did Hit");
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("You Win");  
                    Destroy(hit.collider);
                    Destroy(hit.transform.gameObject, 3f);
                    gameObject.GetComponent<Scope>().enabled = false;
                }
                else if(hit.collider.CompareTag("Civil"))
                {
                    Debug.Log("You Lose");
                    Destroy(hit.collider);
                    Destroy(hit.transform.gameObject, 3f);
                    gameObject.GetComponent<Scope>().enabled = false;

                }

                if (hit.collider.CompareTag("Secret_Target"))
                {
                    Level_Manager.instance.TargetsChallenge(1);
                    Destroy(hit.collider.gameObject);
                }

                GameObject impactGO = Instantiate(impactVFX, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);

            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red, 100f);
                Debug.Log("Did not Hit");
            }
        }

        if (Input.GetButtonDown("Zoom"))
        {
            
            isScoped = !isScoped;
            scopeAnimator.SetBool("IsScoped", isScoped);
            StartCoroutine("OnScoped");
        }

        if(Input.GetButtonUp("Zoom") && isScoped)
        {
            
            isScoped = !isScoped;
            scopeAnimator.SetBool("IsScoped", isScoped);
            StartCoroutine("OnUnScoped");
        }
    }


    //Fire rate system
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.15f);
        scopeAnimator.SetBool("CanShoot", false);

        yield return new WaitForSeconds(2f);
        canShoot = !canShoot;

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
