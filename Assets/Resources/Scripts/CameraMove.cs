using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f;

    private CatMoving _cat;
    private float yOffset;
    private bool _isFollowing = false;

    private void Start()
    {
        _cat = FindFirstObjectByType<CatMoving>();

        yOffset = Camera.main.orthographicSize / 3;

        _isFollowing = false;
    }

    private void Update()
    {
        if (!_isFollowing && _cat.transform.position.y > transform.position.y)
        {
            _isFollowing = true;
        }

        CameraMovement();
    }

    private void CameraMovement()
    {
        if (_isFollowing)
        {
            float targetY = _cat.transform.position.y - yOffset;

            // Проверяем, что камера не опускается ниже начальной позиции
            if (targetY > transform.position.y)
            {
                Vector3 newPosition = transform.position;
                newPosition.y = targetY; // Двигаем камеру только по Y вверх
                transform.position = newPosition;
            }
        }
    }
}
