using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/Player/MovementData")]
public class PlayerMovementData : ScriptableObject
{
    [field: SerializeField] public float BaseMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float BaseSwerveSpeed { get; private set; } = 3f;
    [field: SerializeField] public float ClampValueX { get; private set; } = 1.5f;
}