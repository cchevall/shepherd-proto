using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera/Scanlines Effect")]
public class ScanlinesEffect : MonoBehaviour {
  public Shader shader;
  private Material _material;

  [Range(0, 10)]
  private float lineWidth = .5f;

  [Range(0, 1)]
  private float hardness = 0.8f;

  [Range(0, 1)]
  private float displacementSpeed = 0f;

  protected Material material {
    get {
      if (_material == null) {
        _material = new Material(shader);
        _material.hideFlags = HideFlags.HideAndDontSave;
      }
      return _material;
    }
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination) {
    if (shader == null)
      return;
    material.SetFloat("_LineWidth", lineWidth);
    material.SetFloat("_Hardness", hardness);
    material.SetFloat("_Speed", displacementSpeed);
    Graphics.Blit(source, destination, material, 0);
  }

  void OnDisable() {
    if (_material) {
      DestroyImmediate(_material);
    }
  }
}