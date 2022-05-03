using System.Collections.Generic;
using UnityEngine;

namespace CustomPackages
{
	public class Player_MapManager : MonoBehaviour
	{
		private const float m_Radius = 2.0f;

		private List<MappedObject> m_SearchBuffer;

		private void Awake()
		{
			m_SearchBuffer = new List<MappedObject>();
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Camera cam = Camera.main;
				if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, cam.farClipPlane))
				{
					MappingManager.Instance.FindEntities(hit.point, m_Radius * m_Radius, ref m_SearchBuffer);
					m_SearchBuffer.ForEach(x => SelectEntity(x.gameObject));
				}
			}
		}

		private void SelectEntity(GameObject entity)
		{
			Renderer rend = entity.GetComponent<Renderer>();
			rend.material.color = Color.red;
		}

		private void OnGUI()
		{
			GUI.matrix = Matrix4x4.Scale(Vector3.one * 5f);
			GUILayout.BeginVertical(GUILayout.Width(Screen.width / 5f));
			GUILayout.Label("Left click: Select all entities within " + m_Radius * m_Radius + "m");
			GUILayout.EndVertical();
			GUI.matrix = Matrix4x4.identity;
		}
	}
}
