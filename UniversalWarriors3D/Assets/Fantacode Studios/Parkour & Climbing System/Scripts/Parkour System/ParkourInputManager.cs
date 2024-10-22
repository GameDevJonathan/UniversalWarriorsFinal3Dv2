using System;
using UnityEngine;

namespace FS_ParkourSystem
{
    public partial class ParkourInputManager : MonoBehaviour
    {
        [Header("Keys")]
        [SerializeField] KeyCode jumpKey = KeyCode.Space;
        [SerializeField] KeyCode dropKey = KeyCode.E;
        [SerializeField] KeyCode jumpFromHangKey = KeyCode.Q;
        [SerializeField] KeyCode parkourKey = KeyCode.L;


        [Header("Buttons")]
        [SerializeField] string jumpButton;
        [SerializeField] string dropButton;
        [SerializeField] string jumpFromHangButton;
        [SerializeField] string parkourButton;

        public bool Jump { get; set; }
        public bool JumpKeyDown { get; set; }
        public bool Drop { get; set; }
        public bool JumpFromHang { get; set; }

        public bool Parkour { get; set; }

#if inputsystem
        ParkourInputAction input;
        private void OnEnable()
        {
            input = new ParkourInputAction();
            input.Enable();
        }
        private void OnDisable()
        {
            input.Disable();
        }
#endif

        private void Update()
        {
            //Jump
            HandleJump();

            //JumpKeyDown
            HandleJumpKeyDown();

            //Drop
            HandleDrop();

            //JumpFromHang
            HandleJumpFromHang();

            HandleParkour();
        }

        void HandleJump()
        {
            Jump = false;

#if !UNITY_ANDROID && !UNITY_IOS
            Jump = Input.GetKey(jumpKey) || (String.IsNullOrEmpty(jumpButton) ? false : Input.GetButton(jumpButton));
#endif

#if inputsystem
            Jump = Jump || input.Parkour.Jump.inProgress;
#endif
        }

        void HandleJumpKeyDown()
        {
            JumpKeyDown = false;

#if !UNITY_ANDROID && !UNITY_IOS
            JumpKeyDown = Input.GetKeyDown(jumpKey) || (String.IsNullOrEmpty(jumpButton) ? false : Input.GetButtonDown(jumpButton));
#endif

#if inputsystem
            JumpKeyDown = JumpKeyDown || input.Parkour.Jump.WasPressedThisFrame();
#endif
        }

        void HandleDrop()
        {
            Drop = false;

#if !UNITY_ANDROID && !UNITY_IOS
            Drop = Input.GetKey(dropKey) || (String.IsNullOrEmpty(dropButton) ? false : Input.GetButton(dropButton));
#endif

#if inputsystem
            Drop = Drop || input.Parkour.Drop.inProgress;
#endif
        }

        void HandleJumpFromHang()
        {
            JumpFromHang = false;

#if !UNITY_ANDROID && !UNITY_IOS
            JumpFromHang = Input.GetKey(jumpFromHangKey) || (String.IsNullOrEmpty(jumpFromHangButton) ? false : Input.GetButton(jumpFromHangButton));
#endif
#if inputsystem
            JumpFromHang = JumpFromHang || input.Parkour.JumpFromHang.inProgress;
#endif
        }

        void HandleParkour()
        {
            Parkour = false;

            Parkour = Parkour || input.Parkour.Parkour.inProgress;
            Debug.Log($"Parkour: {Parkour}");

        }


    }
}
