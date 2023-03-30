using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementData movementData;

    private PlayerInput _playerInput;
    public PlayerInput PlayerInput => _playerInput == null ? GetComponent<PlayerInput>() : _playerInput;

    private Player _player;
    public Player Player => _player == null ? GetComponent<Player>() : _player;

    private const float START_POINT_MOVE_DURATION = 2f;

    private void OnEnable()
    {
        Player.OnReachFinish.AddListener(MoveToStartPoint);
    }

    private void OnDisable()
    {
        Player.OnReachFinish.RemoveListener(MoveToStartPoint);
    }

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

    private void MoveToStartPoint()
    {
        transform.DOMove(LevelSystem.Instance.NextLevelStartPosition, START_POINT_MOVE_DURATION)
            .OnComplete(() => GameManager.Instance.CompleteLevel(true));
    }
}