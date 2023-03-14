using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player Variables
    private PlayerControls _playerControls;
    private PlayerCharacter _playerCharacter;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        //create controls
        _playerControls = new PlayerControls();
            
        //get PlayerCharacter
        _playerCharacter = GetComponent<PlayerCharacter>();
        
        //get PlayerMovement
        _playerMovement = GetComponent<PlayerMovement>();
        
        ;

#if UNITY_EDITOR
        //Bind Debug Controls
        _playerControls.Player.DebugTeleport.performed += DebugTeleportPressed;
#endif

    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    
#if UNITY_EDITOR
    private void DebugTeleportPressed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
#endif
    private void FixedUpdate()
    {
        //Rotate Player towards mouse
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - transform.position;
        mouseDir.z = transform.position.z;
        _playerMovement.Rotate(mouseDir);
        
        //Move Player towards input
        Vector2 input = _playerControls.Player.Move.ReadValue<Vector2>();
        _playerMovement.Move(input);

        if (_playerControls.Player.Attack.ReadValue<float>() > 0f)
        {
            _playerCharacter.PerformBasicAttack();
        }
    }
}
