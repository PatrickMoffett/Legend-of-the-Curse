using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player Variables
    private PlayerControls _playerControls;
    private PlayerCharacter _playerCharacter;
    private PlayerMovement _playerMovement;

    // used to swap between mouse aim and thumbstick during game
    private bool hasTwinStickedRecently = false;

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
        if (Input.GetMouseButton(0)) {
            hasTwinStickedRecently = false; // start ignoring gamepad again
        }

        //Rotate Player towards mouse
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDir = mousePos - transform.position;
        mouseDir.z = transform.position.z;
        if (!hasTwinStickedRecently) _playerMovement.Rotate(mouseDir);

        //unless left gamepad is active (twin-stick aiming mode)
        Vector2 twinstick_aim = _playerControls.Player.TwinstickAiming.ReadValue<Vector2>();
        // fixme: allow a deadzone for gamepads with drift don't interfere with mouse
        if (twinstick_aim.x != 0 || twinstick_aim.y != 0) {
            //Debug.Log("twin-stick aiming: "+twinstick_aim.x+","+twinstick_aim.y);
            _playerMovement.Rotate(twinstick_aim);
            hasTwinStickedRecently = true;
        }
        
        //Move Player towards input
        Vector2 input = _playerControls.Player.Move.ReadValue<Vector2>();
        _playerMovement.Move(input);

        if (_playerControls.Player.Attack.ReadValue<float>() > 0f)
        {
            _playerCharacter.PerformBasicAttack();
        }
    }
}
