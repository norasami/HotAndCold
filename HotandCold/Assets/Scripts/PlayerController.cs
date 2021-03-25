using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    HotandColdInputs controls;
    Vector3 move, rotate;
    Transform shipTransform, goldTransform;
    public float distanceToGold, lowPitchRange, highPitchRange, lowVolRange, highVolRange;
    float maxDistance = 40.0f, gameTimer = 0.0f, animalSoundTimer = 0.0f, animalSoundChance, animalSoundVol;
    public AudioClip[] angrySeagull;
    public AudioClip dolphin, seagulls, seals, whaleLow, whaleHigh;
    AudioSource shipSource;
    Vector3 move;
    Vector3 rotate;
    Transform shipTransform;
    Transform goldTransform;
    public float distanceToGold;
    float timer = 0.0f;
    public float WasDistant;
    public float WasTimer;
    public float DeltaDistance;
    public Text winText;
    public bool Victory = false;

    // public AudioClip[] AWarm;
    // public AudioClip[] ACold;
    // public AudioSource WarmSound;
    // public AudioSource ColdSound;



    void Awake()
    {
        controls = new HotandColdInputs();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Player.Rotate.canceled += ctx => rotate = Vector2.zero;

        shipTransform = GameObject.FindWithTag("Player").transform;
        goldTransform = GameObject.FindWithTag("Gold").transform;

        shipSource = GetComponent<AudioSource>();
        WasDistant = distanceToGold;
        WasTimer = 0f;

        /*AWarm = new AudioClip[]
        {
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load()
        };
        ACold = new AudioClip[]
        {
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load(),
            (AudioClip)Resources.Load()
        };*/

    }

    void Update()
    {
        Vector3 m = new Vector3(move.x, move.y, 0) * Time.deltaTime;
        transform.Translate(m, Space.World);

        Vector3 shipPosition = shipTransform.position;
        if ((shipPosition.x + m.x) < -18.0f)
        {
            move.x = -18.0f - shipPosition.x;
        }
        else if ((shipPosition.x + m.x) > 18.0f)
        {
            move.x = 18.0f - shipPosition.x;
        }
        else
        {
            m = new Vector3(move.x, move.y, 0) * Time.deltaTime;
        }

        if ((shipPosition.y + m.y) < -9.5f)
        {
            move.y = -9.5f - shipPosition.y;
        }
        else if ((shipPosition.y + m.y) > 9.5f)
        {
            move.y = 9.5f - shipPosition.y;
        }
        else
        {
            m = new Vector3(move.x, move.y, 0) * Time.deltaTime;
        }

        Vector3 r = new Vector3(0, 0, -rotate.x) * 100f * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(r, m);

        distanceToGold = Vector2.Distance(shipPosition, goldTransform.position);

        gameTimer += Time.deltaTime;
        animalSoundTimer += Time.deltaTime;

        if (animalSoundTimer >= 10.0f)
        {
            StartCoroutine(AnimalSound());
            animalSoundTimer = 0.0f;
        }

        /*if(distanceToGold == maxDistance)
       

        if (distanceToGold <= .5f)
        {
            winText.text = "You have found the gold";
            Victory = true;
        }
        else if (gameTimer >= 60.0f)
        {
            Debug.Log("It's been a whole minute... I'm getting bored!");
        }*/
    }

    IEnumerator AnimalSound()
    {
        animalSoundChance = Random.Range(0.00f, 1.00f);
        shipSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        animalSoundVol = Random.Range(lowVolRange, highVolRange);

        if (animalSoundChance <= 0.50f)
        {
            shipSource.PlayOneShot(seagulls, animalSoundVol);
            Debug.Log("Seagulls");
        }
        else if (animalSoundChance > 0.50f && animalSoundChance <= 0.55f)
        {
            for (int i = 0; i < angrySeagull.Length; i++)
            {
                shipSource.PlayOneShot(angrySeagull[i], animalSoundVol);
            }
            Debug.Log("Angry seagull");
        }
        else if (animalSoundChance > 0.55f && animalSoundChance <= 0.70f)
        {
            shipSource.PlayOneShot(dolphin, animalSoundVol);
            Debug.Log("Dolphin");
        }
        else if (animalSoundChance > 0.70f && animalSoundChance <= 0.80f)
        {
            shipSource.PlayOneShot(seals, animalSoundVol);
            Debug.Log("Seal");
        }
        else if (animalSoundChance > 0.80f && animalSoundChance <= 0.90f)
        {
            shipSource.PlayOneShot(whaleHigh, animalSoundVol);
            Debug.Log("Whale High");
        }
        else
        {
            shipSource.PlayOneShot(whaleLow, animalSoundVol);
            Debug.Log("Whale Low");
        }
        yield return null;

        if (Victory == false)
        {


            DeltaDistance = Mathf.Abs(distanceToGold - WasDistant);

            if (((DeltaDistance*2) + timer - WasTimer) >= 15)
            {
                WasDistant = distanceToGold;
                WasTimer = timer;
                Debug.Log(distanceToGold + " " + WasTimer);
              /*  if(distanceToGold - WasDistant <= 0)
                {
                    ColdSound.clip = ACold[Random.Range(0, 10)];
                    ColdSound.play();
                    CallAudio();
                };
                else
                {
                    WarmSound.clip = AWarm[Random.Range(0, 10)];
                    WarmSound.play();
                    CallAudio();
                };*/
            }

        }


    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }
}
