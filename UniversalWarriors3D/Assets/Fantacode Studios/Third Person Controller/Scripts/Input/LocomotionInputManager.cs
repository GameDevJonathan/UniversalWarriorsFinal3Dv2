using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS_ThirdPerson
{
    public class LocomotionInputManager : MonoBehaviour
    {
        [Header("Keys")]
        [SerializeField] KeyCode jumpKey = KeyCode.Space;
        [SerializeField] KeyCode dropKey = KeyCode.E;
        [SerializeField] KeyCode moveType = KeyCode.Tab;
        [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] KeyCode interactionKey = KeyCode.E;


        [Header("Buttons")]
        [SerializeField] string jumpButton;
        [SerializeField] string dropButton;
        [SerializeField] string moveTypeButton;
        [SerializeField] string sprintButton;
        [SerializeField] string interactionButton;

        public bool JumpKeyDown { get; set; }
        public bool Drop { get; set; }
        public Vector2 DirectionInput { get; set; }
        public Vector2 CameraInput { get; set; }
        public bool ToggleRun { get; set; }
        public bool SprintKey { get; set; }
        public bool Interaction { get; set; }

        public event Action OnInteractionPressed;
        public event Action<float> OnInteractionReleased;

        public float InteractionButtonHoldTime { get; set; } = 0f;
        bool interactionButtonDown;

#if inputsystem
        LocomotionInputAction input;
        private void OnEnable()
        {
            input = new LocomotionInputAction();
            input.Enable();
        }
        private void OnDisable()
        {
            input.Disable();
        }
#endif


        private void Update()
        {
            //Horizontal and Vertical Movement
            HandleDirectionalInput();

            //Camera Movement
            HandlecameraInput();

            //JumpKeyDown
            HandleJumpKeyDown();

            //Drop
            HandleDrop();

            //Walk or Run 
            HandleToggleRun();

            //Sprint
            HandleSprint();

            //Interaction
            HandleInteraction();
        }

        void HandleDirectionalInput()
        {
#if inputsystem
            DirectionInput = input.Locomotion.MoveInput.ReadValue<Vector2>();
            if (DirectionInput == Vector2.zero)
#endif

            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                DirectionInput = new Vector2(h, v);
            }
        }

        void HandlecameraInput()
        {
#if !UNITY_ANDROID && !UNITY_IOS
#if inputsystem
            CameraInput = input.Locomotion.CameraInput.ReadValue<Vector2>();
            if (CameraInput == Vector2.zero)
#endif

            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");
                CameraInput = new Vector2(x, y);
            }
#endif
        }

        void HandleJumpKeyDown()
        {
            JumpKeyDown = false;
#if !UNITY_ANDROID && !UNITY_IOS
            JumpKeyDown = Input.GetKeyDown(jumpKey) || (String.IsNullOrEmpty(jumpButton) ? false : Input.GetButtonDown(jumpButton));
#endif

#if inputsystem
            JumpKeyDown = JumpKeyDown || input.Locomotion.Jump.WasPressedThisFrame();
#endif
        }

        void HandleDrop()
        {
            Drop = false;
#if !UNITY_ANDROID && !UNITY_IOS
            Drop = Input.GetKey(dropKey) || (String.IsNullOrEmpty(dropButton) ? false : Input.GetButton(dropButton));
#endif
#if inputsystem
            Drop = Drop || input.Locomotion.Drop.inProgress;
#endif
        }

        void HandleToggleRun()
        {
            ToggleRun = false;

#if !UNITY_ANDROID && !UNITY_IOS
            ToggleRun = Input.GetKeyDown(moveType) || IsButtonDown(moveTypeButton);
#endif

#if inputsystem
            ToggleRun = ToggleRun || input.Locomotion.MoveType.WasPressedThisFrame();
#endif
        }

        void HandleSprint()
        {
            SprintKey = false;

#if !UNITY_ANDROID && !UNITY_IOS
            SprintKey = Input.GetKey(sprintKey) || (String.IsNullOrEmpty(sprintButton) ? false : Input.GetButton(sprintButton));
#endif

#if inputsystem
            SprintKey = SprintKey || input.Locomotion.SprintKey.inProgress;
#endif
        }

        void HandleInteraction()
        {
            interactionButtonDown = false;

            if (
#if !UNITY_ANDROID && !UNITY_IOS
        Input.GetKeyDown(interactionKey) || IsButtonDown(interactionButton) || 
#endif
        IsNewInputInteractionDown())
            {
                interactionButtonDown = true;
                Interaction = true;
            }
            else
            {
                Interaction = false;
            }

            if (interactionButtonDown)
            {
                if ((
#if !UNITY_ANDROID && !UNITY_IOS
            Input.GetKeyUp(interactionKey) || IsButtonUp(interactionButton) || 
#endif
            IsNewInputInteractionUp()))
                {
                    interactionButtonDown = false;
                    InteractionButtonHoldTime = 0f;
                }

                InteractionButtonHoldTime += Time.deltaTime;
            }
        }


        public bool IsButtonDown(string buttonName)
        {
            if (!String.IsNullOrEmpty(buttonName))
                return Input.GetButtonDown(buttonName);
            else
                return false;
        }

        public bool IsButtonUp(string buttonName)
        {
            if (!String.IsNullOrEmpty(buttonName))
                return Input.GetButtonUp(buttonName);
            else
                return false;
        }

        public bool IsNewInputInteractionDown()
        {
#if inputsystem
            return input.Locomotion.Interaction.WasPressedThisFrame();
#else
            return false;
#endif
        }

        public bool IsNewInputInteractionUp()
        {
#if inputsystem
            return input.Locomotion.Interaction.WasReleasedThisFrame();
#else
            return false;
#endif
        }
    }
}