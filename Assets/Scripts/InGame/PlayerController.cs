using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform projectileSpawnPos;
    [SerializeField] GameObject smokeParticles;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform playerMeshTransform;
    [SerializeField] Animator simpleCharacterAnimator;


    // Movements tuning
    private float rotationSpeed = 200.0f;
    private float moveSpeed = 30.0f;
    private float xAxisMaxRotation = .20f;
    private float yAxisMaxRotation = .35f;

    // Movements bounderies
    private float yAxisTopBound = 50f;
    private float yAxisBottomBound = 1.5f;
    private float xAxisBound = 35f;

    // Movements states
    [SerializeField] bool canFly = false;
    private Vector2 leftStickDirection = Vector2.zero;
    private string latestActionMap = "PlayerOnGround";

    void Awake()
    {
        if (GameManager.isLoaded())
        {
            GameManager.Instance.InitGame();
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void PauseGame()
    {
        if (!GameManager.isLoaded() || GameManager.Instance.isGameOver)
        {
            return;
        }
        GameManager.Instance.Pause();
    }

    private void Shoot()
    {
        if (GameManager.isLoaded() && (GameManager.Instance.isPaused || GameManager.Instance.isGameOver))
        {
            return;
        }
        Instantiate(projectilePrefab, projectileSpawnPos.position, transform.rotation);
        simpleCharacterAnimator.Play("GrenadeThrow", -1, 0f);
    }

    private void OnJump()
    {
        playerInput.SwitchCurrentActionMap("PlayerFlying");
        smokeParticles.gameObject.SetActive(true);
    }

    private void OnLand()
    {
        playerInput.SwitchCurrentActionMap("PlayerOnGround");
        smokeParticles.gameObject.SetActive(false);
        LandOnGround();
    }

    /**
     * Called by input system
     */
    private void OnMove(InputValue value)
    {
        leftStickDirection = value.Get<Vector2>();
    }

    private void OnFire()
    {
        Shoot();
    }

    private void OnPause()
    {
        latestActionMap = playerInput.currentActionMap.name;
        PauseGame();
        playerInput.SwitchCurrentActionMap("Pause");
    }

    private void OnResume()
    {
        PauseGame();
        playerInput.SwitchCurrentActionMap(latestActionMap);
    }

    private void OnRetry()
    {
        if (GameManager.isLoaded())
        {
            GameManager.Instance.Retry();
        }
    }

    private void OnGoToTitle()
    {
        if (GameManager.isLoaded())
        {
            GameManager.Instance.GoToTitle();
        }
    }

    private void MovePlayer()
    {
        if (GameManager.isLoaded() && (GameManager.Instance.isPaused || GameManager.Instance.isGameOver))
        {
            return;
        }
        float horizontalAxis = leftStickDirection.x;
        float verticalAxis = leftStickDirection.y;
        HandlePlayerRotation(horizontalAxis, verticalAxis);
        if (playerInput.currentActionMap.name == "PlayerFlying")
        {
            HandlePlayerMovement(0f, verticalAxis);
        }
        HandlePlayerMovement(horizontalAxis, 0f);
    }

    private void HandlePlayerRotation(float horAxis, float verAxis)
    {
        float rotateX = verAxis * Time.deltaTime * rotationSpeed;
        float rotateY = horAxis * Time.deltaTime * rotationSpeed;
        if (ShouldRotateX(verAxis))
        {
            // it is important to split rotation in to method call to prevent cross-contamination
            if (ShouldEaseLandingRotation(verAxis))
            {
                Vector3 rotation = new Vector3(rotateX, 0f, 0f);
                transform.Rotate(rotation, Space.World); // this one is important to prevent z axis rotation
            }
            else
            {
                Vector3 rotation = new Vector3(rotateX * -1, 0f, 0f);
                transform.Rotate(rotation, Space.World); // this one is important to prevent z axis rotation
            }
        }

        if (ShouldRotateY(horAxis))
        {
            // it is important to split rotation in to method call to prevent cross-contamination
            Vector3 rotation = new Vector3(0f, rotateY, 0f);
            transform.Rotate(rotation, Space.Self); // this one is important to prevent z axis rotation
        }
    }

    private bool ShouldEaseLandingRotation(float verAxis)
    {
        return transform.position.y < 10f && verAxis < 0f && transform.rotation.x > 0f;
    }

    private bool ShouldRotateX(float verAxis)
    {
        if (ShouldEaseLandingRotation(verAxis))
        {
            return true;
        }
        if (transform.rotation.x > -xAxisMaxRotation && transform.rotation.x < xAxisMaxRotation)
        {
            return true;
        }
        if (transform.rotation.x < -xAxisMaxRotation)
        {
            return verAxis <= 0;
        }
        if (transform.rotation.x > xAxisMaxRotation)
        {
            return verAxis >= 0;
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
        else if (other.CompareTag("Ground"))
        {
            HandleGroundState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            HandleGroundState(false);
        }
    }

    private void HandleGroundState(bool isGrounded)
    {
        if (isGrounded)
        {
            simpleCharacterAnimator.SetBool("Grounded_b", true);
        }
        else
        {
            simpleCharacterAnimator.SetBool("Grounded_b", false);
        }
    }

    private void HandleObstacleCollision(Collider other)
    {
        Debug.Log("Player Collided with Obstacle");
        Die();
    }

    private void HandleProjectileCollision(Collider other)
    {
        Debug.Log("Player Collided with Projectile");
    }

    private void HandleEnemyCollision(Collider other)
    {
        Debug.Log("Player Collided with Enemy");
    }

    private void HandlePowerupCollision(Collider other)
    {
        Debug.Log("Player Collided with Powerup");
        canFly = true;
        Destroy(other.gameObject);
    }

    private void Die()
    {
        simpleCharacterAnimator.SetBool("Death_b", true);
        smokeParticles.gameObject.SetActive(false);
        LandOnGround();
        if (GameManager.isLoaded())
        {
            GameManager.Instance.GameOver();
            playerInput.SwitchCurrentActionMap("GameOver");
        }
    }


    private void LandOnGround()
    {
        StartCoroutine(LandOnGroundCoroutine(transform, transform.position, new Vector3(transform.position.x ,2f , 0f)));
    }
    private IEnumerator LandOnGroundCoroutine(Transform objectToMove, Vector3 a, Vector3 b)
    {
        float speed = 30f;
        float step = speed / (a - b).magnitude * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
    }
}
