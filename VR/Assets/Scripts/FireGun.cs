using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{

    [SerializeField] GameObject gunVFX; 
    [SerializeField] List<GameObject> vfx = new List<GameObject>();
    [SerializeField] Transform gunpoint;

    [SerializeField] GameObject enemydeathVFX;

    private GameObject effectToSpawn;

    [SerializeField] GameObject impactParticle;
   

    AudioSource audioSource;
    public AudioClip hitSound;

    public GameManager gameManager;

    // public Transform trailParent;

    RaycastHit hit;
    public LayerMask mask;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        effectToSpawn = vfx[0];
    }

    public void Fire()
    {
       
        //instantiate the particles
        GameObject _projectileVFX = Instantiate(effectToSpawn, gunpoint.position, gunpoint.rotation);
        GameObject _gunVFX = Instantiate(gunVFX, gunpoint.position, gunpoint.rotation);

        

        if (Physics.Raycast(gunpoint.position, gunpoint.forward, out hit, 1000, mask))
        {
            Debug.Log(hit.collider.name);
            

            if (hit.collider.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rigidbody = hit.collider.attachedRigidbody;
                rigidbody.AddForce(gunpoint.forward * 5);
                audioSource.PlayOneShot(hitSound);               
            }

            if (hit.collider.gameObject.tag == "Asteroid")
            {
                Destroy(hit.collider.gameObject);
                GameManager.playerScore += 500;
            }
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Instantiate(enemydeathVFX, hit.collider.transform.position, Quaternion.identity);
                Destroy(hit.collider.gameObject);
            } 

            if (hit.collider.gameObject.tag == "Button")
            {
                if (hit.collider.name == "StartCollisionDetect")
                {
                    audioSource.PlayOneShot(hitSound);
                    Debug.Log("I'm hitting the Start");
                    gameManager.StartGame();

                }
                if (hit.collider.name == "RestartCollisionDetect")
                {
                    audioSource.PlayOneShot(hitSound);
                    Debug.Log("I'm hitting the Restart");
                    gameManager.RestartGame();
                }                                

            }           

            //&& hit.collider.gameObject.tag != "Button"
                      
            Instantiate(impactParticle, hit.point, Quaternion.identity);                  

           if (hit.collider.gameObject.tag != "Button")
            {
                GameManager.AsteroidHit();
            }

            if (hit.collider.gameObject.tag == "ChangeButton")
            {
                Debug.Log("Changing Gun");
            }

        }              

    }

    private void Update()
    {
        Debug.DrawRay(gunpoint.position, gunpoint.forward * 100, Color.red);
    }



}

