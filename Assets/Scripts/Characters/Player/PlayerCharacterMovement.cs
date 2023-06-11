
using UnityEngine;

public class PlayerCharacterMovement : CharacterMovement
{
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private ParticleSystem particleSystem;
    public override void Move(Vector2 direction)
    {
        //store original position
        Vector3 position =transform.position;
        
        //call base movement
        base.Move(direction);
        
        //get new position
        Vector3 newPosition = transform.position;
        
        //add distance to player statistics
        float distance = (newPosition - position).magnitude;
        playerStatistics.totalDistance += distance;
        playerStatistics.steps = Mathf.RoundToInt(playerStatistics.totalDistance);

        if (distance > 0)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
    }
}
