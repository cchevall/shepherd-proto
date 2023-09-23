using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnimation : Rotate
{
    protected Vector2 direction = Vector2.up;
    protected float moveSpeed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
         axis = Vector3.up;
         rate = 1.5f;
         StartCoroutine(ChangeDirectionCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        RotateSelf();
        Move();
    }

    protected void Move()
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);
    }

    protected IEnumerator ChangeDirectionCoroutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            yield return new WaitForSeconds(.5f);
            direction *= -1; // switch from up to down and vice versa
        }
    }
}
