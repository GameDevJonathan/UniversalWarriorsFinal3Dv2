using System;
using UnityEngine;
namespace FS_CombatSystem
{
    public class CombatInputManager : MonoBehaviour
    {
        [Header("Keys")]
        [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
        [SerializeField] KeyCode blockKey = KeyCode.Mouse1;
        [SerializeField] KeyCode combatModeKey = KeyCode.F;
        [SerializeField] KeyCode heavyAttackKey = KeyCode.R;
        [SerializeField] KeyCode counterKey = KeyCode.Q;


        [Header("Buttons")]
        [SerializeField] string attackButton;
        [SerializeField] string blockButton;
        [SerializeField] string combatModeButton;
        [SerializeField] string counterButton;
        [SerializeField] string heavyAttackButton;

        public bool Block { get; set; }
        public bool CombatMode { get; set; }

        public event Action<float, bool, bool, bool> OnAttackPressed;

        bool attackDown;
        bool heavyAttackDown;

        public float AttackHoldTime { get; private set; } = 0f;
        public float HeavyAttackHoldTime {get; private set;} = 0f;

        float chargeTime = 0f;
        bool useAttackInputForCounter = false;
        private void Start()
        {
            chargeTime = CombatSettings.i.HoldTimeForChargedAttacks;
            useAttackInputForCounter = CombatSettings.i.SameInputForAttackAndCounter;
        }

#if inputsystem
        CombatInputAction input;
        private void OnEnable()
        {
            input = new CombatInputAction();
            input.Enable();
        }
        private void OnDisable()
        {
            input.Disable();
        }
#endif

        private void Update()
        {

            //Attack
            HandleAttack();

            //HeavyAttack
            HandleHeavyAttack();

            //Counter
            HandleCounter();

            //Block
            HandleBlock();

            //Combat Mode
            HandleCombatMode();
        }

        void HandleAttack()
        {
            if (
#if !UNITY_ANDROID && !UNITY_IOS
                    Input.GetKeyDown(attackKey) || IsButtonDown(attackButton) || 
#endif
                    IsNewInputAttackDown())
            {
                attackDown = true;
            }

            if (attackDown)
            {
                if (AttackHoldTime >= chargeTime ||
#if !UNITY_ANDROID && !UNITY_IOS
                        Input.GetKeyUp(attackKey) || IsButtonUp(attackButton) || 
#endif
                        IsNewInputAttackUp())
                {
                    OnAttackPressed?.Invoke(AttackHoldTime, false, useAttackInputForCounter, AttackHoldTime >= chargeTime);
                    attackDown = false;
                    AttackHoldTime = 0f;
                }
                AttackHoldTime += Time.deltaTime;
            }
        }

        void HandleHeavyAttack()
        {
            if (
#if !UNITY_ANDROID && !UNITY_IOS
        Input.GetKeyDown(heavyAttackKey) || IsButtonDown(heavyAttackButton) || 
#endif
            IsNewInputHeavyAttackDown())
            {
                heavyAttackDown = true;
            }

            if (heavyAttackDown)
            {
                if (HeavyAttackHoldTime >= chargeTime ||
#if !UNITY_ANDROID && !UNITY_IOS
            Input.GetKeyUp(heavyAttackKey) || IsButtonUp(heavyAttackButton) || 
#endif
            IsNewInputHeavyAttackUp())
                {
                    OnAttackPressed?.Invoke(HeavyAttackHoldTime, true, false, HeavyAttackHoldTime >= chargeTime);
                    heavyAttackDown = false;
                    HeavyAttackHoldTime = 0f;
                }

                HeavyAttackHoldTime += Time.deltaTime;
            }

        }

        void HandleCounter()
        {
            if (!useAttackInputForCounter && (
#if !UNITY_ANDROID && !UNITY_IOS
                Input.GetKeyDown(counterKey) || IsButtonDown(counterButton) || 
#endif
                IsNewInputCounterDown()))
                OnAttackPressed?.Invoke(0f, false, true, false);
        }

        void HandleBlock()
        {
            Block = false;

#if !UNITY_ANDROID && !UNITY_IOS
            if (Input.GetKey(blockKey) || 
                (!string.IsNullOrEmpty(blockButton) && Input.GetButton(blockButton)))
            {
                Block = true;
            }
#endif

#if inputsystem
            Block = Block || input.Combat.Block.inProgress;
#endif
        }

        void HandleCombatMode()
        {
            CombatMode = false;

#if !UNITY_ANDROID && !UNITY_IOS
            if (Input.GetKeyDown(combatModeKey) || (!string.IsNullOrEmpty(combatModeButton) && Input.GetButtonDown(combatModeButton)))
            {
                CombatMode = true;
            }
#endif

#if inputsystem
            if (input.Combat.CombatMode.WasPressedThisFrame())
            {
                CombatMode = true;
            }
#endif
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

        public bool IsNewInputAttackDown()
        {
#if inputsystem
            return input.Combat.Attack.WasPressedThisFrame();
#else
            return false;
#endif
        }

        public bool IsNewInputAttackUp()
        {
#if inputsystem
            return input.Combat.Attack.WasReleasedThisFrame();
#else
            return false;
#endif
        }

        public bool IsNewInputHeavyAttackDown()
        {
#if inputsystem
            return input.Combat.HeavyAttack.WasPressedThisFrame();
#else
            return false;
#endif
        }

        public bool IsNewInputHeavyAttackUp()
        {
#if inputsystem
            return input.Combat.HeavyAttack.WasReleasedThisFrame();
#else
            return false;
#endif
        }

        public bool IsNewInputCounterDown()
        {
#if inputsystem
            return input.Combat.Counter.WasPressedThisFrame();
#else
            return false;
#endif
        }
    }
}