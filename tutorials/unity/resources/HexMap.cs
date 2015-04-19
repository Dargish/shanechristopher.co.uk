using UnityEngine;
using System;
using System.Collections;

public class HexMap : MonoBehaviour
{
	public int Width = 16;
	public int Height = 32;
	public float Scale = 4.0f;
	public int Seed = 1;
	public float MountainHeight = 0.8f;
	public float HillHeight = 0.64f;
	public float GrassHeight = 0.4f;

	int m_width;
	int m_height;
	float m_scale;
	float m_seed;
	float m_mountainHeight;
	float m_hillHeight;
	float m_grassHeight;
	GameObject[,] m_hexes;

	void Start ()
	{
		ResetHexMap();
	}

	void Update ()
	{
		if(RequiresReset())
		{
			DestroyHexMap();
			ResetHexMap();
		}
	}

	bool RequiresReset()
	{
		return (
			Width != m_width ||
			Height != m_height || 
			Scale != m_scale ||
			Seed != m_seed ||
			MountainHeight != m_mountainHeight || 
			HillHeight != m_hillHeight ||
			GrassHeight != m_grassHeight
			);
	}

	void DestroyHexMap()
	{
		for(int z = 0; z < m_height; ++z)
		{
			for(int x = 0; x < m_width; ++x)
			{
				try
				{
					if (m_hexes[x, z] != null)
					{
						if (Application.isPlaying)
						{
							GameObject.Destroy(m_hexes[x, z]);
						}
						else
						{
							GameObject.DestroyImmediate(m_hexes[x, z]);
						}
					}
				}
				catch(NullReferenceException e)
				{
					Debug.Log (e.Message.ToString());
				}
			}
		}
	}

	void ResetHexMap()
	{
		m_width = Width;
		m_height = Height;
		m_scale = Scale;
		m_seed = Seed;
		m_mountainHeight = Mathf.Max (HillHeight, Mathf.Min (1.0f, MountainHeight));
		MountainHeight = m_mountainHeight;
		m_hillHeight = Mathf.Max (GrassHeight, Mathf.Min (MountainHeight, HillHeight));
		HillHeight = m_hillHeight;
		m_grassHeight = Mathf.Max (0.0f, Mathf.Min (HillHeight, GrassHeight));
		GrassHeight = m_grassHeight;
		m_hexes = new GameObject[m_width, m_height];
		for(int z = 0; z < m_height; ++z)
		{
			float rowXOffset = (z % 2 == 0) ? 0.0f : 0.75f;
			for(int x = 0; x < m_width; ++x)
			{
				string resource = "Models/river";
				float r = Mathf.PerlinNoise((x / m_scale) + m_seed, (z / m_scale) + m_seed);
				if (r > m_mountainHeight)
				{
					resource = "Models/mountain";
				}
				else if (r > m_hillHeight)
				{
					resource = "Models/hill";
				}
				else if (r > m_grassHeight)
				{
					resource = "Models/grass";
				}
				m_hexes[x, z] = (GameObject)Instantiate(Resources.Load(resource));
				m_hexes[x, z].transform.position = new Vector3(x * 1.5f + rowXOffset, 0.0f, z * 0.433f);
				m_hexes[x, z].transform.parent = gameObject.transform;
			}
		}
	}
}
