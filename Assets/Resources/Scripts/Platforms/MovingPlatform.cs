using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class MovingPlatform : PlatformBase
{
    private Vector3 _startPos;
    private float _minX;
    private float _maxX;

    private void OnEnable()
    {
        _startPos = transform.position;
        CalculateScreenBounds();
        SpecialAction();
    }
    private void OnDisable()
    {
        StopCoroutine(Moving());
    }

    public override void OnJump()
    {
        
    }

    protected override void SpecialAction()
    {
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        var direction = 1; 
        while (true)
        {
            transform.position += new Vector3(0.1f * direction, 0, 0);

            if (direction > 0 && transform.position.x >= _maxX + _startPos.x)
            {
                direction = -1;
            }

            else if (direction < 0 && transform.position.x <= _startPos.x + _minX)
            {
                direction = 1; 
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void CalculateScreenBounds()
    {
        // ѕолучаем размер камеры по горизонтали
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // –ассчитываем минимальную и максимальную координату X
        _minX = Camera.main.transform.position.x - cameraHalfWidth;
        _maxX = Camera.main.transform.position.x + cameraHalfWidth;

        if (_startPos.x - _maxX > 2)
            _maxX = 2;
        if (Mathf.Abs(_startPos.x - _minX) > 2)
            _minX = -2;


        Debug.Log($"Screen bounds: MinX = {_minX}, MaxX = {_maxX}");
    }
}
