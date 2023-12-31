using UnityEngine;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    [FormerlySerializedAs("arrowBodyRigidbody2D")]
    public Rigidbody2D rigidbody2D;

    public bool isShooting;

    private float _angle;
    public ParticleSystem sparkParticle;

    void FixedUpdate()
    {
        if (isShooting)
        {
            RotateArrow();
        }
    }

    private void RotateArrow()
    {
        _angle = Mathf.Atan2(-rigidbody2D.velocity.y, -rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Target")
        {
            transform.SetParent(null);
            rigidbody2D.simulated = false;
            isShooting = false;
            sparkParticle.gameObject.SetActive(false);

        }
    }
}