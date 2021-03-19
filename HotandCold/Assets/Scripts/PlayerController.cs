using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    HotandColdInputs controls;
    Vector3 move;
    Vector3 rotate;
    Transform shipTransform;
    Transform goldTransform;
    public float distanceToGold;
    float maxDistance = 40.0f;
    float timer = 0.0f;

    void Awake()
    {
        controls = new HotandColdInputs();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Player.Rotate.canceled += ctx => rotate = Vector2.zero;

        shipTransform = GameObject.FindWithTag("Player").transform;
        goldTransform = GameObject.FindWithTag("Gold").transform;
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

        if(distanceToGold == maxDistance)
        {
            Debug.Log("You literally couldn't be further off course...");
        }
        else if (timer >= 60.0f)
        {
            Debug.Log("It's been a whole minute... I'm getting bored!");
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
