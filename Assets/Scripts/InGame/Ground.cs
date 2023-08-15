using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public Material groundMaterial;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float groundSpeed = gameManager.gameSpeed / 130f;
        groundMaterial.mainTextureOffset -= new Vector2(0f, groundSpeed * Time.deltaTime);
    }
}
