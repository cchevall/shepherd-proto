using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] public float gameSpeedToTileOffsetRatio {
        get { return _gameSpeedToTileOffsetRatio; }
        set {
            if (value != 0f) {
                _gameSpeedToTileOffsetRatio = value;
            } else {
                Debug.LogError("Invalid gameSpeedToTileOffsetRatio: Cannot set ratio to 0f.");
            }
        }
    }

    [SerializeField] Material _groundMaterial;
    float _gameSpeedToTileOffsetRatio = 130f;

    // Update is called once per frame
    void Update()
    {
        MoveTileOffset();
    }



    void MoveTileOffset()
    {
        if (!GameManager.isLoaded())
        {
            return;
        }
        float groundSpeed = GameManager.Instance.gameSpeed / _gameSpeedToTileOffsetRatio;
        _groundMaterial.mainTextureOffset -= new Vector2(0f, groundSpeed * Time.deltaTime);
    }
}
