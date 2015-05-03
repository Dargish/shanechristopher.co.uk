using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float MovementSpeed = 0.25f;
	public float ZoomSpeed = 2.0f;
	public float ClosestZoom = 1.0f;
	public float FurthestZoom = 8.0f;
	public float Dampening = 0.7f;
	public float MaxSpeed = 0.5f;

	Vector3 m_velocity;

	void Start ()
	{
	
	}

	void Update ()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		float zoom = -Input.GetAxis("Mouse ScrollWheel");
		m_velocity.x += horizontal * MovementSpeed;
		m_velocity.z += vertical * MovementSpeed;
		m_velocity.y += zoom * ZoomSpeed;
		m_velocity.x = Mathf.Max (-MaxSpeed, Mathf.Min (m_velocity.x, MaxSpeed));
		m_velocity.z = Mathf.Max (-MaxSpeed, Mathf.Min (m_velocity.z, MaxSpeed));
		m_velocity.y = Mathf.Max (-MaxSpeed, Mathf.Min (m_velocity.y, MaxSpeed));
	}

	void FixedUpdate()
	{
		Vector3 position = gameObject.transform.position;
		position += m_velocity;
		if (position.y < ClosestZoom)
		{
			position.y = ClosestZoom;
			m_velocity.y = 0.0f;
		}
		if (position.y > FurthestZoom)
		{
			position.y = FurthestZoom;
			m_velocity.y = 0.0f;
		}
		gameObject.transform.position = position;
		m_velocity *= Dampening;
		if (m_velocity.magnitude < 0.01f)
		{
			m_velocity = Vector3.zero;
		}
	}
}
