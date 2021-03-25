using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    HotandColdInputs controls;
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

    public AudioClip[] AWarm;
    public AudioClip[] ACold;
    public AudioSource audioSource;
    


    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        controls = new HotandColdInputs();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Player.Rotate.canceled += ctx => rotate = Vector2.zero;

        shipTransform = GameObject.FindWithTag("Player").transform;
        goldTransform = GameObject.FindWithTag("Gold").transform;
        WasDistant = distanceToGold;
        WasTimer = 0f;


        AWarm = new AudioClip[]
        {
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm1"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm2"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm3"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm4"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm5"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm6"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm7"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm8"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Warm9")
        };
         ACold = new AudioClip[]
        {
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold1"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold2"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold3"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold4"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold5"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold6"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold7"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold8"),
            (AudioClip)Resources.Load<AudioClip>("Assets/Audio/Cold9")
         };

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

        timer += Time.deltaTime;

       

        if (distanceToGold <= .5f)
        {
            winText.text = "You have found the gold";
            Victory = true;
        }

        if (Victory == false)
        {


            DeltaDistance = Mathf.Abs(distanceToGold - WasDistant);

            if (((DeltaDistance*2) + timer - WasTimer) >= 15)
            {
                WasDistant = distanceToGold;
                WasTimer = timer;
                Debug.Log(distanceToGold + " " + WasTimer);

                if(distanceToGold - WasDistant <= 0)
                {
                    playCold();
                
                }
                else
                {
                    playWarm();
                  
                }
            }

        }


    }

    void playWarm()
    {
        audioSource.clip = AWarm[Random.Range(0, 8)];
        audioSource.Play();
    }

    void playCold()
    {
        audioSource.clip = ACold[Random.Range(0, 8)];
        audioSource.Play();
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
