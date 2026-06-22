using UnityEngine;

// Grabbed from Wilderness2, unfinished and untested. Just didnt wanna forget
// to save it here.
public class TiledBackground: MonoBehaviour {
  Camera _mainCamera;
  SpriteRenderer _spriteRenderer;

  void Awake() {
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  void Start() {
    _mainCamera = Camera.main; // TODO logic
    AdjustSpriteSize();
  }

  void Update() {
    AdjustSpritePosition();
  }

  void AdjustSpritePosition() {
    var positionDeltaFromCamera = transform.position - _mainCamera.transform.position;
    transform.position = _mainCamera.transform.position + new Vector3(
      positionDeltaFromCamera.x % 4.5f, // TODO logic
      positionDeltaFromCamera.y % 6f, // TODO logic
      10f // TODO logic
    );
  }

  void AdjustSpriteSize() {
    _spriteRenderer.size = new Vector2(
      70f,
      _mainCamera.orthographicSize * 3 + 5f
    );
  }
}
