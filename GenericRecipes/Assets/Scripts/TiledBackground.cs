using UnityEngine;

// Grabbed from Wilderness2. It's still a little rough but it's pretty reusable
// now.
//
// Object hierarchy should be:
// TiledBackground
// parent of SpriteAnchor
// parent of Sprite (with renderer component)
//
public class TiledBackground: MonoBehaviour {
  [SerializeField] Transform _spriteAnchor;
  [SerializeField] SpriteRenderer _spriteRenderer;

  Camera _mainCamera;
  float _textureWidth;
  float _textureHeight;

  void Awake() {
    var spriteBounds = _spriteRenderer.sprite.bounds;
    _textureWidth = spriteBounds.size.x;
    _textureHeight = spriteBounds.size.y;
  }

  void Start() {
    _mainCamera = Camera.main; // TODO logic
    AdjustSpriteSize();
  }

  void Update() {
    AdjustSpritePosition();
  }

  void AdjustSpritePosition() {
    var cameraPosition = _mainCamera.transform.position;
    var position = transform.position;
    var positionDeltaFromCamera = position - cameraPosition;
    transform.position = new Vector3(
      cameraPosition.x + positionDeltaFromCamera.x % _textureWidth,
      cameraPosition.y + positionDeltaFromCamera.y % _textureHeight,
      position.z
    );
  }

  void AdjustSpriteSize() {
    float viewportHeight = _mainCamera.orthographicSize * 2;
    float viewportWidth = viewportHeight * _mainCamera.aspect;
    float newSpriteWidth = viewportWidth * 2f + _textureWidth;
    float newSpriteHeight = viewportHeight * 2f + _textureHeight;
    _spriteRenderer.size = new Vector2(newSpriteWidth, newSpriteHeight);

    var anchorOffset = Vector3.zero;
    anchorOffset.x = (((newSpriteWidth / _textureWidth) - 1f) % 2f) * 0.5f * _textureWidth;
    anchorOffset.y = (((newSpriteHeight / _textureHeight) - 1f) % 2f) * 0.5f * _textureHeight;
    _spriteAnchor.localPosition = anchorOffset;
  }
}
