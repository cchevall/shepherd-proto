using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

enum Direction
{
    forward,
    left,
    right
}

public class PlayerController : MonoBehaviour, IHealth
{
    public int currentHP {
        get {
            return _currentHP;
        }
    }
    public int damageOnCollision {
        get {
            return _damageOnCollision;
        }
    }

    [SerializeField] int _currentHP = 100;
    [SerializeField] int _damageOnCollision = 0;
    [SerializeField] GameObject aimAxis;
    [SerializeField] GameObject smokeParticles;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject playerMesh;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform projectileSpawnPos;
    [SerializeField] Animator simpleCharacterAnimator;
    [SerializeField] TextMeshProUGUI lifeText;


    // Movements tuning
    private float rotationSpeed = 120.0f;
    private float moveSpeed = 50.0f;
    private float xAxisMaxRotation = .32f;
    private float yAxisMaxRotation = .35f;
    private float dashAmplitude = 20f;

    // Movements states
    private Vector2 leftStickDirection = Vector2.zero;
    private string latestActionMap = "PlayerOnGround";
    private Coroutine landOnGroundCoroutine;
    private bool landOnGroundCoroutineIsActive = false;
    private bool takeOffCoroutineIsActive = false;
    private bool isDashing = false;
    private bool isGrounded = true;

    void Awake()
    {
        if (lifeText == null) {
            var UI = GameObject.Find("UI");
            lifeText = UI.transform.Find("Hud Canvas/Life Text").GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        if (GameManager.isLoaded())
        {
            GameManager.Instance.InitGame();
            lifeText.SetText(_currentHP.ToString());
        }
    }

    public void ApplyDamage(int amount) {
        _currentHP = _currentHP - amount < 0 ? 0 : _currentHP - amount;
        lifeText.SetText(_currentHP.ToString());
        simpleCharacterAnimator.SetTrigger("Get_Hit_t");
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        MovePlayer();
    }


    private void OnJump()
    {
        if (GameManager.isLoaded() && GameManager.Instance.isGameOver)
        {
            return;
        }
        if (landOnGroundCoroutineIsActive)
        {
            StopCoroutine(landOnGroundCoroutine);
            landOnGroundCoroutineIsActive = false;
        }
        Vector2 latestLeftStickDir = leftStickDirection;
        playerInput.SwitchCurrentActionMap("PlayerFlying");
        leftStickDirection = latestLeftStickDir; // switching resets stickDirection for some reason;
        smokeParticles.gameObject.SetActive(true);
    }

    private void OnLand()
    {
        if (GameManager.isLoaded() && GameManager.Instance.isGameOver)
        {
            return;
        }
        playerInput.SwitchCurrentActionMap("PlayerOnGround");
        smokeParticles.gameObject.SetActive(false);
        LandOnGround();
    }

    private void OnMove(InputValue value)
    {
        leftStickDirection = value.Get<Vector2>();
    }

    private void OnBoost(InputValue value)
    {
        if (GameManager.isLoaded() && GameManager.Instance.isGameOver)
        {
            return;
        }
        if (value.isPressed) {
            GameManager.Instance.BoostGameSpeed(4f);
        } else {
            GameManager.Instance.BoostGameSpeed(1f);
        }
    }

    private void OnDashLeft()
    {
        if ((GameManager.isLoaded() && GameManager.Instance.isGameOver) || isDashing)
        {
            return;
        }
        StartCoroutine(DashCoroutine(Direction.left));
    }

    private void OnDashRight()
    {
        if ((GameManager.isLoaded() && GameManager.Instance.isGameOver) || isDashing)
        {
            return;
        }
        StartCoroutine(DashCoroutine(Direction.right));
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
        Vector3 upFront = transform.position + Vector3.forward;
        if (IsInForest())
        {
            // Look Forward to dodge trees
            transform.LookAt(upFront);
        }
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
                Vector3 rotation = new Vector3(-rotateX, 0f, 0f);
                transform.Rotate(rotation, Space.World); // this one is important to prevent z axis rotation
            }
        }

        // it is important to split rotation in to method call to prevent cross-contamination
        if (ShouldRotateY(horAxis))
        {
            if (IsEnteringInForest(horAxis))
            {
                // Smoothly rotate to look up front
                Quaternion targetRotation = Quaternion.LookRotation(upFront - transform.position);
                // // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
            }
            else
            {
                Vector3 rotation = new Vector3(0f, rotateY, 0f);
                transform.Rotate(rotation, Space.Self); // this one is important to prevent z axis rotation
            }
        }
    }

    private bool ShouldEaseLandingRotation(float verAxis)
    {
        return transform.position.y < 10f && verAxis < 0f && transform.rotation.x > 0f;
    }

    private bool IsEnteringInForest(float horAxis)
    {
        float boundEaseOffset = 10f;
        float leftForestEaseBound = -LevelConfig.xForestBound + boundEaseOffset;
        float rightForestBound = LevelConfig.xForestBound - boundEaseOffset;
        if (transform.position.x < leftForestEaseBound && transform.position.x > -LevelConfig.xForestBound)
        {
            return true;
        }
        else if (transform.position.x > rightForestBound && transform.position.x < LevelConfig.xForestBound)
        {
            return true;
        }
        return false;
    }

    private bool IsInForest()
    {
        return transform.position.x < -LevelConfig.xForestBound || transform.position.x > LevelConfig.xForestBound;
    }

    private bool ShouldRotateX(float verAxis)
    {
        if (IsInForest())
        {
            return false;
        }
        else if (ShouldEaseLandingRotation(verAxis))
        {
            return true;
        }
        else if (transform.rotation.x > -xAxisMaxRotation && transform.rotation.x < xAxisMaxRotation)
        {
            return true;
        }
        else if (transform.rotation.x < -xAxisMaxRotation)
        {
            return verAxis <= 0;
        }
        else if (transform.rotation.x > xAxisMaxRotation)
        {
            return verAxis >= 0;
        }
        return false;
    }

    private bool ShouldRotateY(float horAxis)
    {
        if (IsInForest())
        {
            return false;
        }
        else if (IsEnteringInForest(horAxis))
        {
            return true;
        }
        else
        {
            return YRotationInBound(transform, HorizontalAxisToDirection(horAxis));
        }
    }

    private bool YRotationInBound(Transform toTest, Direction direction)
    {
        switch (direction)
        {
            case Direction.left:
                return toTest.rotation.y > -yAxisMaxRotation;
            case Direction.right:
                return toTest.rotation.y < yAxisMaxRotation;
            default:
                return true;
        }
    }

    private Direction HorizontalAxisToDirection(float yAxis)
    {
        if (yAxis == 0f)
        {
            return Direction.forward;
        }
        else if (yAxis < 0f)
        {
            return Direction.left;
        }
        else
        {
            return Direction.right;
        }
    }

    private void HandlePlayerMovement(float horAxis, float verAxis)
    {
        float moveOnYAxis = verAxis * Time.deltaTime * moveSpeed;
        float moveOnXAxis = horAxis * Time.deltaTime * moveSpeed;
        transform.Translate(new Vector3(moveOnXAxis, moveOnYAxis, 0f), Space.World);
        float xPos = Mathf.Clamp(transform.position.x, -LevelConfig.xBound, LevelConfig.xBound);
        float yPos = Mathf.Clamp(transform.position.y, LevelConfig.yBottomBound, LevelConfig.yTopBound);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(other);
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
            isGrounded = true;
            HandleGroundState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
            HandleGroundState();
        }
    }

    private void HandleGroundState()
    {
        if (isGrounded)
        {
            simpleCharacterAnimator.SetBool("Grounded_b", true);
        }
        else
        {
            if (takeOffCoroutineIsActive)
            {
                return;
            }
            StartCoroutine(TakeOffCoroutine());
        }
    }

    private void HandleObstacleCollision(Collider other)
    {
        Debug.Log("Player Collided with Obstacle");
        Die();
    }

    private void HandleEnemyCollision(Collider other)
    {
        Debug.Log("Player Collided with Enemy");
    }

    private void HandlePowerupCollision(Collider other)
    {
        Debug.Log("Player Collided with Powerup");
        Destroy(other.gameObject);
    }

    private void Die()
    {
        playerInput.SwitchCurrentActionMap("GameOver");
        aimAxis.gameObject.SetActive(false);
        simpleCharacterAnimator.SetBool("Death_b", true);
        smokeParticles.gameObject.SetActive(false);
        Rigidbody playerRB = playerMesh.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.FreezeRotationX;
        playerRB.isKinematic = false;
        playerRB.useGravity = true;
        playerRB.AddForce(new Vector3(0f, -20000f, -20000f));
        if (GameManager.isLoaded())
        {
            GameManager.Instance.GameOver();
        }
    }

    private void LandOnGround()
    {
        if (landOnGroundCoroutineIsActive)
        {
            return;
        }
        Vector3 positionOnGround = new Vector3(transform.position.x, LevelConfig.yBottomBound, 0f);
        landOnGroundCoroutine = StartCoroutine(LandOnGroundCoroutine(transform, transform.position, positionOnGround));
    }
    private IEnumerator LandOnGroundCoroutine(Transform objectToMove, Vector3 from, Vector3 to)
    {
        landOnGroundCoroutineIsActive = true;
        float speed = moveSpeed + 20f; // gives the player a small advantage over flying while dodging objects
        float progress = 0;
        while (!isGrounded)
        {
            float step = speed / (from - to).magnitude * Time.deltaTime;
            progress += step; // Goes from 0 to ~1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(from, to, progress); // Move objectToMove closer to b
            yield return new WaitForEndOfFrame(); // Leave the routine and return here in the next frame
        }
        objectToMove.position = to;
        landOnGroundCoroutineIsActive = false;
    }

    private IEnumerator TakeOffCoroutine()
    {
        takeOffCoroutineIsActive = true;
        simpleCharacterAnimator.SetBool("Grounded_b", false);
        yield return new WaitForSeconds(0.5f);
        takeOffCoroutineIsActive = false;
    }

    private IEnumerator DashCoroutine(Direction direction)
    {
        isDashing = true;
        float speed = moveSpeed * 3f;
        float progress = 0;
        Vector3 from = transform.position;
        Vector3 to = direction == Direction.left ? transform.position - new Vector3(dashAmplitude, 0f) : transform.position + new Vector3(dashAmplitude, 0f);
        while (progress <= 1.0f)
        {
            float step = speed / (from - to).magnitude * Time.deltaTime;
            progress += step; // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(from, to, progress); // Move objectToMove closer to b
            yield return new WaitForEndOfFrame(); // Leave the routine and return here in the next frame
        }
        transform.position = to;
        isDashing = false;
        simpleCharacterAnimator.SetBool("Dash_Left_b", false);
    }
}
