using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform m_LookAt;
	public float m_YawRotationalSpeed;
	public float m_PitchRotationalSpeed;
	public float m_MinPitch=-45.0f;
	public float m_MaxPitch=75.0f;
	public float minCamDist = 5.0f;
	public float maxCamDist = 10.0f;
	public KeyCode m_DebugLockAngleKeyCode=KeyCode.I;
	public KeyCode m_DebugLockKeyCode=KeyCode.O;
	bool m_AngleLocked=false;
	bool m_CursorLocked=true;
	
	void Start()
	{
		Cursor.lockState=CursorLockMode.Locked;
		m_CursorLocked=true;
	}
	void OnApplicationFocus()
	{
		if(m_CursorLocked)
			Cursor.lockState=CursorLockMode.Locked;
	}
	void LateUpdate()
	{
	
#if UNITY_EDITOR
		if(Input.GetKeyDown(m_DebugLockAngleKeyCode))
			m_AngleLocked=!m_AngleLocked;
		if(Input.GetKeyDown(m_DebugLockKeyCode))
		{
			if(Cursor.lockState==CursorLockMode.Locked)
				Cursor.lockState=CursorLockMode.None;
			else
				Cursor.lockState=CursorLockMode.Locked;
			m_CursorLocked=Cursor.lockState==CursorLockMode.Locked;
		}
#endif

        float l_MouseAxisX = Input.GetAxis("Mouse X");
        float l_MouseAxisY = Input.GetAxis("Mouse Y");

        Vector3 l_Direction = m_LookAt.position - transform.position;
        float l_Distance = l_Direction.magnitude;

        Vector3 l_DesiredPosition = transform.position;

		if(!m_AngleLocked && (l_MouseAxisX>0.01f || l_MouseAxisX<-0.01f || l_MouseAxisY>0.01f || l_MouseAxisY<-0.01f))
		{
			Vector3 l_EulerAngles=transform.eulerAngles;
			float l_Yaw=(l_EulerAngles.y+180.0f);
			float l_Pitch=l_EulerAngles.x;

			//TODO: Update Yaw and Pitch
			l_Yaw += l_MouseAxisX * m_YawRotationalSpeed;
			l_Pitch -= l_MouseAxisY * m_PitchRotationalSpeed;
			l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch, m_MaxPitch);

			//TODO: Update DesiredPosition
			l_DesiredPosition.x = m_LookAt.position.x 
				+ Mathf.Sin(l_Yaw * Mathf.Deg2Rad) 
				* Mathf.Cos(l_Pitch * Mathf.Deg2Rad)
				* l_Distance;
			l_DesiredPosition.y = m_LookAt.position.y +
				Mathf.Sin(l_Pitch * Mathf.Deg2Rad)
				*l_Distance;
			l_DesiredPosition.z = m_LookAt.position.z 
				+ Mathf.Cos(l_Yaw * Mathf.Deg2Rad) 
				*Mathf.Cos(l_Pitch*Mathf.Deg2Rad)
				* l_Distance;

			// x = lookAt.x + ( sin(yaw)*cos(pitch)*distance )
			// y = lookAt.y + ( sin(pitch)*distance )
			// z = lookAt.z + ( cos(yaw)*cos(pitch)*distance )

			//TODO: Update new direction
			l_Direction = m_LookAt.position - l_DesiredPosition;
		}
		l_Direction /=l_Distance;

		//TODO: Clamp between minDistance and maxDistance. Update desiredPosition.
		l_Distance = Mathf.Clamp(l_Distance, minCamDist, maxCamDist);
		l_DesiredPosition = m_LookAt.position - l_Direction * l_Distance;

		//TODO: Bring camera closer if colliding with any object.
            
		transform.forward=l_Direction;
		transform.position=l_DesiredPosition;
	} 
}
