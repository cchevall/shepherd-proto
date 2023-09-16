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
    [SerializeField] float _gameSpeedToTileOffsetRatio = 180f; // helps tile to fit env movements
    [SerializeField] float _progress = 0f; // tile offset progress


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
        Vector2 from = Vector2.zero;
        Vector2 to = new Vector2(0f, 1f);
        float groundSpeed = GameManager.Instance.gameSpeed / _gameSpeedToTileOffsetRatio;
        float step = groundSpeed / (from - to).magnitude * Time.deltaTime;
        _progress += step;
        _groundMaterial.mainTextureOffset = new Vector2(0f, -_progress);
        _progress = _progress % 1f;
    }
}
