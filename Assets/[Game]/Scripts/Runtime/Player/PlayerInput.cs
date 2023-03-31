using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _inputX;
    public float InputX { get { return _inputX; } }

    private const float INPUT_LIMIT = 10;
    private const float INPUT_SMOOTHNESS = 15f;

    private float _smoothX;
    private Vector2 _lastInputPosition;

    private void Update()
    {
        GetSwerveInput();
    }

    private void GetSwerveInput()
    {
        if (Input.GetMouseButtonDown(0))
            _lastInputPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            _smoothX = Mathf.Clamp(Input.mousePosition.x - _lastInputPosition.x, -INPUT_LIMIT, INPUT_LIMIT);

            _lastInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
            _smoothX = 0;

        _inputX = Mathf.Lerp(_inputX, _smoothX, Time.deltaTime * INPUT_SMOOTHNESS);
    }
}