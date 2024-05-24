using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    public enum Type { small = 1, heavy = 2, large = 3}
    public Type type;

    private Target target;

    public void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public void Destroyed()
    {
        OnDestroyed?.Invoke(this);
    }

    public void delete()
    {
        target = GetComponent<Target>();
       Destroy(target);
        
        
        
    }



}
