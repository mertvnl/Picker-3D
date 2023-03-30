using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementData movementData;

    private PlayerInput _playerInput;
    public PlayerInput PlayerInput => _playerInput == null ? GetComponent<PlayerInput>() : _playerInput;

    private Player _player;
    public Player Player => _player == null ? GetComponent<Player>() : _player;

    private void FixedUpdate()
    {
        if (!Player.IsControlable)
            return;

        Move();
    }

    private void Update()
    {
        if (!Player.IsControlable)
            return;

        HandleClamping();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(x: movementData.BaseSwerveSpeed * PlayerInput.InputX, y: Player.Rigidbody.velocity.y, z: movementData.BaseMovementSpeed) * Time.fixedDeltaTime;
        Player.Rigidbody.velocity = direction;
    }

    private void HandleClamping()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -movementData.ClampValueX, movementData.ClampValueX);
        transform.position = pos;
    }
}