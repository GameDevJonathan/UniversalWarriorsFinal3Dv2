using System.Collections;
using Unity.Collections;
using UnityEngine;

namespace FS_ThirdPerson
{
    public class CameraController : MonoBehaviour
    {
        public Transform followTarget;

        public bool advancedCameraRotation = true;
        Vector3 followTargetPosition;

        [Range(0, 1)]
        [SerializeField] float sensitivity = .6f;
        [SerializeField] float distance = 2.5f;
        [SerializeField] float dragEffectDistance = .75f;

        float newDistance, currDistance;

        [SerializeField] float minVerticalAngle = -45;
        [SerializeField] float maxVerticalAngle = 70;

        [SerializeField] Vector3 framingOffset = new Vector3(0, 1, 0);

        [SerializeField] bool invertX;
        [SerializeField] bool invertY = true;


        float rotationX;
        float rotationY;
        float yRot;

        float invertXVal;
        float invertYVal;

        LocomotionInputManager input;
        PlayerController playerController;
        public LayerMask layerMask = 1;

        [SerializeField, Tooltip("This value must be set before starting play mode. It cannot be changed while the game is running.")] bool lockCursor = true;
        
        public float Distance => distance;
        private void Awake()
        {
            input = FindObjectOfType<LocomotionInputManager>();
            playerController = followTarget.GetComponentInParent<PlayerController>();
            playerController.OnStartCameraShake -= StartCameraShake;
            playerController.OnStartCameraShake += StartCameraShake;
            playerController.OnLand -= playerController.OnStartCameraShake;
            playerController.OnLand += playerController.OnStartCameraShake;
        }

        private void Start()
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (lockCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
#endif
            followTargetPosition = followTarget.position - Vector3.forward * 4;
        }

        private void Update()
        {
            invertXVal = (invertX) ? -1 : 1;
            invertYVal = (invertY) ? -1 : 1;

            rotationX += input.CameraInput.y * invertYVal * sensitivity;
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

            if (input.CameraInput != Vector2.zero)
                yRot = rotationY += input.CameraInput.x * invertXVal * sensitivity;

            else if (advancedCameraRotation && playerController.CurrentSystemState == SystemState.Locomotion && input.CameraInput.x == 0 && input.DirectionInput.y > -.4f)
            {
                StartCoroutine(CameraRotDelay());
                rotationY = Mathf.Lerp(rotationY, yRot, Time.deltaTime * 25);
            }

            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

            followTargetPosition = Vector3.Lerp(followTargetPosition, followTarget.position, 10 * (dragEffectDistance / (Mathf.Abs((followTargetPosition - followTarget.position).magnitude - distance) + 0.01f)) * Time.deltaTime);
            if (CameraShakeDuration > 0)
            {
                followTargetPosition += Random.insideUnitSphere * CurrentCameraShakeAmount * cameraShakeAmount * Mathf.Clamp01(CameraShakeDuration);
                CameraShakeDuration -= Time.deltaTime;
            }

            var forward = transform.forward;
            forward.y = 0;
            var focusPostion = followTargetPosition + new Vector3(framingOffset.x, framingOffset.y) + forward * framingOffset.z;

            RaycastHit hit;
            

            if (Physics.Raycast(focusPostion, (transform.position - focusPostion), out hit, distance, layerMask))
                newDistance = Mathf.Clamp(hit.distance, 0.6f, distance);
            else
                newDistance = distance;

            currDistance = Mathf.Lerp(currDistance, newDistance, 10f * Time.deltaTime);

            transform.position = focusPostion - targetRotation * new Vector3(0, 0, currDistance);
            transform.rotation = targetRotation;

            previousPos = followTarget.transform.position;

        }
        public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

        bool moving;
        Vector3 previousPos;
        bool inDelay;
        float cameraRotSmooth;


        public float CameraShakeDuration { get; set; } = 0f;
        public float CurrentCameraShakeAmount { get; set; } = 0f;

        public float cameraShakeAmount = 0.6f;
        public void StartCameraShake(float currentCameraShakeAmount, float shakeDuration)
        {
            CurrentCameraShakeAmount = currentCameraShakeAmount;
            CameraShakeDuration = shakeDuration;
        }
        IEnumerator CameraRotDelay()
        {
            var movDist = Vector3.Distance(previousPos, followTarget.transform.position);
            if (movDist > 0.001f)
            {
                if (!moving)
                {
                    moving = true;
                    inDelay = true;
                    yield return new WaitForSeconds(1.5f);
                    inDelay = false;
                }
            }
            else
            {
                moving = false;
                cameraRotSmooth = 0;
            }

            cameraRotSmooth = Mathf.Lerp(cameraRotSmooth, !inDelay ? 25 : 5, Time.deltaTime);
            yRot = Mathf.Lerp(yRot, yRot + input.DirectionInput.x * invertXVal * 2, Time.deltaTime * cameraRotSmooth);
        }
    }
}