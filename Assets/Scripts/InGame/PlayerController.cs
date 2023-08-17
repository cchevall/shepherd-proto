using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform playerMeshTransform;

    // Movements tuning
    private float rotationSpeed = 150.0f;
    private float moveSpeed = 15.0f;
    private float xAxisMaxRotation = .20f;
    private float yAxisMaxRotation = .35f;

    // Movements bounderies
    private float yAxisTopBound = 25f;
    private float yAxisBottomBound = 1f;
    private float xAxisBound = 35f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (!GameManager.isLoaded())
        {
            return;
        }
        GameManager.Instance.Pause();
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, transform.position + Vector3.forward * 3, transform.rotation);
    }

    private void MovePlayer()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        HandlePlayerRotation(horizontalAxis, verticalAxis);
    }

    private void HandlePlayerRotation(float horAxis, float verAxis)
    {
        float rotateX = verAxis * Time.deltaTime * rotationSpeed * -1;
        float rotateY = horAxis * Time.deltaTime * rotationSpeed;
        if (ShouldRotateX(verAxis))
        {
            // it is important to split rotation in to method call to prevent cross-contamination
            Vector3 rotation = new Vector3(rotateX, 0f, 0f);
            transform.Rotate(rotation, Space.World); // this one is important to prevent z axis rotation
        }
            
        if (ShouldRotateY(horAxis))
        {
            // it is important to split rotation in to method call to prevent cross-contamination
            Vector3 rotation = new Vector3(0f, rotateY, 0f);
            transform.Rotate(rotation, Space.Self); // this one is important to prevent z axis rotation
        }
        HandlePlayerMovement(0f, verAxis);
        HandlePlayerMovement(horAxis, 0f);
    }

    private bool ShouldRotateX(float verAxis)
    {
        if (transform.rotation.x > -xAxisMaxRotation && transform.rotation.x < xAxisMaxRotation)
        {
            return true;
        }
        if (transform.rotation.x < -xAxisMaxRotation)
        {
            return verAxis < 0;
        }
        if (transform.rotation.x > xAxisMaxRotation)
        {
            return verAxis > 0;
        }
        return false;
    }

    private bool ShouldRotateY(float horAxis)
    {
        if (transform.rotation.y > -yAxisMaxRotation && transform.rotation.y < yAxisMaxRotation)
        {
            return true;
        }
        if (transform.rotation.y < -yAxisMaxRotation)
        {
            return horAxis > 0;
        }
        if (transform.rotation.y > yAxisMaxRotation)
        {
            return horAxis < 0;
        }
        return false;
    }

    private void HandlePlayerMovement(float horAxis, float verAxis)
    {
        float moveOnYAxis = verAxis * Time.deltaTime * moveSpeed;
        float moveOnXAxis = horAxis * Time.deltaTime * moveSpeed;
        transform.Translate(new Vector3(moveOnXAxis, moveOnYAxis, 0f), Space.World);
        float xPos = Mathf.Clamp(transform.position.x, -xAxisBound, xAxisBound);
        float yPos = Mathf.Clamp(transform.position.y, yAxisBottomBound, yAxisTopBound);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(other);
        }
        else if (other.CompareTag("Projectile"))
        {
            HandleProjectileCollision(other);
        }
        else if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        else if (other.CompareTag("Powerup"))
        {
            HandlePowerupCollision(other);
        }
    }

    private void HandleObstacleCollision(Collider other)
    {
        Debug.Log("Collided with Obstacle");
    }

    private void HandleProjectileCollision(Collider other)
    {
        Debug.Log("Collided with Projectile");
    }

    private void HandleEnemyCollision(Collider other)
    {
        Debug.Log("Collided with Enemy");
    }

    private void HandlePowerupCollision(Collider other)
    {
        Debug.Log("Collided with Powerup");
        Destroy(other.gameObject);
    }
}
