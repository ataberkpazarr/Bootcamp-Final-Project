
using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // syntax for generic class
{

	static T m_instance; //private static. specific object in our hierarchy

	public static T Instance //public way to set and get private instance
	{
		get //get accessor. There is no set because the pattern is read only  
		{
			if (m_instance == null) // if not currently set, then set it 
			{
				m_instance = GameObject.FindObjectOfType<T>();

				if (m_instance == null) // it is actually still null since not a new object assigned yet
				{
					GameObject singleton = new GameObject(typeof(T).Name); // for etc if the name is score manager then call score manager 
					m_instance = singleton.AddComponent<T>();
				}
			}
			return m_instance;
		}
	}

	public virtual void Awake()
	{
		if (m_instance == null) // if it is not set to anything
		{
			m_instance = this as T; //then we need to set 

		}
		else //instance != this // destroy on load 
		{
			Destroy(gameObject); //if already exists then destroy the newly created one
		}
	}
}