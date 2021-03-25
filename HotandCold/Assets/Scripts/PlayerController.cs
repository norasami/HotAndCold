using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    HotandColdInputs controls;
    Vector3 move, rotate;
    Transform shipTransform, goldTransform;
    public float lowVolRange, highVolRange;
    float distanceToGold, animalSoundTimer = 0.0f, animalSoundChance, wasDistant;
    public AudioClip[] angrySeagull, aWarm, aCold;
    public AudioClip dolphin, seagulls, seals, whaleLow, whaleHigh;
    AudioSource shipSource;
    public Text winText;
    public bool victory = false;

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
        wasDistant = distanceToGold;
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
        animalSoundTimer += Time.deltaTime;
        if (animalSoundTimer >= 10.0f)
        {
            StartCoroutine(AnimalSound());
            animalSoundTimer = 0.0f;
        }

        if (distanceToGold <= .5f)
        {
            winText.text = "You have found the gold";
            victory = true;
        }
    }

    IEnumerator AnimalSound()
    {
        animalSoundChance = Random.Range(0.00f, 1.00f);
        if (animalSoundChance <= 0.50f)
        {
            shipSource.PlayOneShot(seagulls, lowVolRange);
            Debug.Log("Seagulls");
        }
        else if (animalSoundChance > 0.50f && animalSoundChance <= 0.55f)
        {
            int i = Random.Range(0, 5);
            shipSource.PlayOneShot(angrySeagull[i], lowVolRange);
            Debug.Log("Angry seagull");
        }
        else if (animalSoundChance > 0.55f && animalSoundChance <= 0.70f)
        {
            shipSource.PlayOneShot(dolphin, lowVolRange);
            Debug.Log("Dolphin");
        }
        else if (animalSoundChance > 0.70f && animalSoundChance <= 0.80f)
        {
            shipSource.PlayOneShot(seals, lowVolRange);
            Debug.Log("Seal");
        }
        else if (animalSoundChance > 0.80f && animalSoundChance <= 0.90f)
        {
            shipSource.PlayOneShot(whaleHigh, lowVolRange);
            Debug.Log("Whale High");
        }
        else
        {
            shipSource.PlayOneShot(whaleLow, lowVolRange);
            Debug.Log("Whale Low");
        }
        yield return new WaitForSeconds(3.0f);
        if (victory == false)
        {
            if(wasDistant - distanceToGold <= 0)
            {
                PlayCold();
                Debug.Log("Colder");
                wasDistant = distanceToGold;
            }
            else
            {
                PlayWarm();
                Debug.Log("Warmer");
                wasDistant = distanceToGold;
            }
        }
        yield return null;
    }

    void PlayWarm()
    {
        int i = Random.Range(0, 8);
        shipSource.PlayOneShot(aWarm[i], highVolRange);
    }

    void PlayCold()
    {
        int i = Random.Range(0, 8);
        shipSource.PlayOneShot(aCold[i], highVolRange);
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
