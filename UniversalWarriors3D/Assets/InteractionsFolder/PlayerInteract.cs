using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private LocomotionInputAction _inputAction;
    public LayerMask layerMask;

    private void Awake()
    {
        _inputAction = new LocomotionInputAction();
        _inputAction.Locomotion.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputAction.Locomotion.Interaction.WasPressedThisFrame())
        {
            Debug.Log("button pressed");
            float interactRange = 4f;            
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                Debug.Log(collider);
                if(collider.TryGetComponent(out NPCInteractable npcInteractable))                
                {
                    Debug.Log("GotComponnent");
                    npcInteractable.Interact();
                }
                
            }

        }
    }
}

/*

    public class Player : MonoBehaviour
    {
        
        private UpgradedLegacyInput _inputActions;
        


        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();

            if (_controller == null)
                Debug.LogError("No Character Controller Present");

            _anim = GetComponentInChildren<Animator>();

            if (_anim == null)
                Debug.Log("Failed to connect the Animator");

            _inputActions = new UpgradedLegacyInput();
            _inputActions.Player.Enable();


        }

        private void Update()
        {
            _move = _inputActions.Player.Move.ReadValue<Vector2>();

            if (_canMove == true)
                CalcutateMovement();

        }

        private void CalcutateMovement()
        {
            _playerGrounded = _controller.isGrounded;
            //float h = Input.GetAxisRaw("Horizontal");
            float h = _move.x;
            //float v = Input.GetAxisRaw("Vertical");
            float v = _move.y;

            transform.Rotate(transform.up, h);

            var direction = transform.forward * v;
            var velocity = direction * _speed;


            _anim.SetFloat("Speed", Mathf.Abs(velocity.magnitude));


            if (_playerGrounded)
                velocity.y = 0f;
            if (!_playerGrounded)
            {
                velocity.y += -20f * Time.deltaTime;
            }

            _controller.Move(velocity * Time.deltaTime);

        }

        private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            switch (zone.GetZoneID())
            {
                case 1: //place c4
                    _detonator.Show();
                    break;
                case 2: //Trigger Explosion
                    TriggerExplosive();
                    break;
            }
        }

        private void ReleasePlayerControl()
        {
            _canMove = false;
            _followCam.Priority = 9;
        }

        private void ReturnPlayerControl()
        {
            _model.SetActive(true);
            _canMove = true;
            _followCam.Priority = 10;
        }

        private void HidePlayer()
        {
            _model.SetActive(false);
        }

        private void TriggerExplosive()
        {
            _detonator.TriggerExplosion();
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
            Laptop.onHackComplete -= ReleasePlayerControl;
            Laptop.onHackEnded -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= ReleasePlayerControl;
            Forklift.onDriveModeExited -= ReturnPlayerControl;
            Forklift.onDriveModeEntered -= HidePlayer;
            Drone.OnEnterFlightMode -= ReleasePlayerControl;
            Drone.onExitFlightmode -= ReturnPlayerControl;
        }

    }
}

*/