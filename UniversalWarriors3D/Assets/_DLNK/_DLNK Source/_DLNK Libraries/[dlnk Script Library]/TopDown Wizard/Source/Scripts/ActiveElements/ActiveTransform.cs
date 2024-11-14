using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTransform : MonoBehaviour
{
    [System.Serializable]
    public class Target
    {
        public GameObject TargetGO;
        public Vector3 TranslateGO = new Vector3(0, 0, 0);
        public Vector3 RotateGO = new Vector3(0, 0, 0);  // Changed to Vector3 for easy editing
        public bool Scalable = false;
        public Vector3 ScaleGO = new Vector3(1, 1, 1);

        [HideInInspector]
        public Vector3 originalTf;
        [HideInInspector]
        public Quaternion originalRot;
        [HideInInspector]
        public Vector3 differenceSc;
    }

    public List<Target> Targets;
    public float Duration;
    public bool done;
    public bool _localAuto = false;
    private TDActiveElement activeParent;
    private bool _ismoving = false;
    private float _timer = 0f;
    private float _percentage = 0f;
    private bool _waitforit = false;
    private bool _isIn;
    private bool _isOut;

    void Start()
    {
        activeParent = GetComponent<TDActiveElement>();

        foreach (Target tar in Targets)
        {
            tar.originalTf = tar.TargetGO.transform.localPosition;
            tar.originalRot = tar.TargetGO.transform.localRotation;

            if (tar.Scalable)
            {
                tar.differenceSc = tar.ScaleGO - tar.TargetGO.transform.localScale;
                tar.ScaleGO = tar.TargetGO.transform.localScale;
            }

            if (done)
            {
                tar.TargetGO.transform.localPosition = tar.originalTf + tar.TranslateGO;
                tar.TargetGO.transform.localRotation = tar.originalRot * Quaternion.Euler(tar.RotateGO); // Apply rotation
                if (tar.Scalable)
                    tar.TargetGO.transform.localScale = tar.ScaleGO + tar.differenceSc;
            }
        }

        if (activeParent.Automatic)
            _localAuto = true;
    }

    void Update()
    {
        if (activeParent.ActiveCollider.actived)
        {
            if (_localAuto)
            {
                if (!_isIn)
                {
                    _waitforit = false;
                    _ismoving = true;
                }
                _isIn = true;
            }
            else if (Input.GetKeyDown(activeParent.ActiveElementKey))
            {
                _ismoving = true;
                _waitforit = false;
                if (activeParent.AutoOnExit && !done)
                    _isIn = true;
            }
        }
        else if (activeParent.ActiveCollider.hasexit)
        {
            if (activeParent.AutoOnExit && _isIn)
                _isOut = true;
            _isIn = false;
        }

        if (activeParent.AutoOnExit && _isOut && !_ismoving)
        {
            _waitforit = false;
            _ismoving = true;
            _isOut = false;
        }

        if (_ismoving && !_waitforit)
        {
            _timer += Time.deltaTime;
            _percentage = done ? (1 - (_timer / Duration)) : (_timer / Duration);

            if (_percentage >= 1f)
            {
                _ismoving = false;
                _percentage = 1f;
                _timer = 0f;
                done = true;
                _waitforit = true;
                Debug.Log("Action Completed");
            }
            else if (_percentage <= 0f)
            {
                _ismoving = false;
                _percentage = 0f;
                _timer = 0f;
                done = false;
                _waitforit = true;
                Debug.Log("Action Reversed");
            }

            foreach (Target tar in Targets)
            {
                tar.TargetGO.transform.localPosition = tar.originalTf + (_percentage * tar.TranslateGO);

                Quaternion targetRotation = tar.originalRot * Quaternion.Euler(tar.RotateGO);
                tar.TargetGO.transform.localRotation = Quaternion.Slerp(tar.originalRot, targetRotation, _percentage);

                if (tar.Scalable)
                    tar.TargetGO.transform.localScale = tar.ScaleGO + (_percentage * tar.differenceSc);
            }
        }
    }
}
