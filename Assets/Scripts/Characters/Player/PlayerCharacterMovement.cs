
using UnityEngine;

public class PlayerCharacterMovement : CharacterMovement
{
    [SerializeField] private PlayerStatistics playerStatistics;
    public override void Move(Vector2 direction)
    {
        //store original position
        Vector3 position =transform.position;
        
        //call base movement
        base.Move(direction);
        
        //get new position
        Vector3 newPosition = transform.position;
        
        //add distance to player statistics
        playerStatistics.totalDistance += (newPosition - position).magnitude;
        playerStatistics.steps = Mathf.RoundToInt(playerStatistics.totalDistance);
    }
}
