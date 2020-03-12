using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Hunter))]
public class Fisk : MonoBehaviour
{
    private Hunter m_hunter = null;

	void Start ()
    {
        m_hunter = GetComponent<Hunter>();
	}

	void Update ()
    {
        Vector3 velocity = m_hunter.VelocityH;
        if (velocity.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 6);
        }

        transform.position = m_hunter.PositionH;
        Debug.Log(m_hunter.PositionH);
        transform.position += new Vector3(0, 0, transform.parent.position.z); //inherit z-position
	}
}
