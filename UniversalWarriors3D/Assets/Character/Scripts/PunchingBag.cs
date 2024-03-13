using UnityEngine;

public class PunchingBag : MonoBehaviour
{
    public Rigidbody _rigidBody;
    public float forceMultiplier = 0.5f;
    public SpringJoint joint;
    public Vector3 hitPoint;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Sensor" ) return;

        //HitBoxData hitBoxData = other.GetComponent<HitBoxData>();
        other.TryGetComponent<HitBoxData>(out HitBoxData hitBoxData);

        var point = other.ClosestPoint(hitPoint);
        //Debug.Log($"hit point {point}");
        var relativePoint = transform.position;
        relativePoint.y = hitPoint.y;
        var forceForward = relativePoint + hitPoint;
        //Debug.Log("hitbox data " + hitBoxData.attack.AttackForce.ToString());
        float _attackForce = hitBoxData.attack.AttackForce;


        if (_rigidBody != null)
        {
            _rigidBody.AddForce(forceForward * ( _attackForce * forceMultiplier), ForceMode.Force);

        }
    }
}
