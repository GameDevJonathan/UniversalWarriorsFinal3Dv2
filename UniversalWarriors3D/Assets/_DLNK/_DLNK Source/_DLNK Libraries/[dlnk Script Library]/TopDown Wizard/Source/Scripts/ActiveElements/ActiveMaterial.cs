using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMaterial : MonoBehaviour
{
    [System.Serializable]
    public class Target
    {
        public GameObject TargetGO;
        public Material MaterialTarget;
        public int MaterialIndex; // New variable for material index
        [HideInInspector]
        public Material tmpMaterial;
    }

    public List<Target> Targets;
    public bool OverrideMatTarget;
    public bool Switched = false;
    public Material OverrideMaterial;
    public bool _localAuto = false;

    private TDActiveElement activeParent;
    private bool _waiting = false;

    void Start()
    {
        // find reference for vars
        activeParent = this.GetComponent<TDActiveElement>();
        foreach (Target tar in Targets)
        {
            // Store the original material at the specified index
            Renderer renderer = tar.TargetGO.GetComponent<Renderer>();
            if (renderer != null && tar.MaterialIndex < renderer.materials.Length)
            {
                tar.tmpMaterial = renderer.materials[tar.MaterialIndex];
            }
        }

        // check if global auto enabled
        if (activeParent.Automatic)
            _localAuto = true;
    }

    void Update()
    {
        // check if character hit collider
        if (activeParent.ActiveCollider.actived)
        {
            // check if character has activated action
            if ((_localAuto && !_waiting) || Input.GetKeyDown(activeParent.ActiveElementKey))
            {
                if (!Switched)
                {
                    foreach (Target tar in Targets)
                    {
                        Renderer renderer = tar.TargetGO.GetComponent<Renderer>();
                        if (renderer != null && tar.MaterialIndex < renderer.materials.Length)
                        {
                            Material[] materials = renderer.materials;
                            if (OverrideMatTarget)
                                materials[tar.MaterialIndex] = OverrideMaterial;
                            else
                                materials[tar.MaterialIndex] = tar.MaterialTarget;
                            renderer.materials = materials;
                        }
                    }
                    _waiting = true;
                    Switched = true;
                }
                else
                {
                    foreach (Target tar in Targets)
                    {
                        Renderer renderer = tar.TargetGO.GetComponent<Renderer>();
                        if (renderer != null && tar.MaterialIndex < renderer.materials.Length)
                        {
                            Material[] materials = renderer.materials;
                            materials[tar.MaterialIndex] = tar.tmpMaterial;
                            renderer.materials = materials;
                        }
                    }
                    _waiting = true;
                    Switched = false;
                }
            }
        }
        // check if character is leaving scene with automode
        else if (activeParent.ActiveCollider.hasexit && _waiting)
        {
            if (activeParent.AutoOnExit)
            {
                if (!Switched)
                {
                    foreach (Target tar in Targets)
                    {
                        Renderer renderer = tar.TargetGO.GetComponent<Renderer>();
                        if (renderer != null && tar.MaterialIndex < renderer.materials.Length)
                        {
                            Material[] materials = renderer.materials;
                            if (OverrideMatTarget)
                                materials[tar.MaterialIndex] = OverrideMaterial;
                            else
                                materials[tar.MaterialIndex] = tar.MaterialTarget;
                            renderer.materials = materials;
                        }
                    }
                    _waiting = false;
                    Switched = true;
                }
                else
                {
                    foreach (Target tar in Targets)
                    {
                        Renderer renderer = tar.TargetGO.GetComponent<Renderer>();
                        if (renderer != null && tar.MaterialIndex < renderer.materials.Length)
                        {
                            Material[] materials = renderer.materials;
                            materials[tar.MaterialIndex] = tar.tmpMaterial;
                            renderer.materials = materials;
                        }
                    }
                    _waiting = false;
                    Switched = false;
                }
            }
            else _waiting = false;
        }
    }
}
