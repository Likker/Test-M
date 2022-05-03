using UnityEngine;

namespace CustomPackages
{
	public abstract class MappedObject : MonoBehaviour
	{
        // Cache
        protected MappingManager m_MapManager;

		// Buffers
		protected int m_MapIndex;
		protected int m_LastMapKey;

		private bool m_Registered;
		
		protected virtual void Awake()
		{
			m_Registered = false;
			
			// Cache
			m_MapManager = MappingManager.Instance;
		}

		protected void RegisterMap()
		{
			m_MapIndex   = m_MapManager.RegisterEntity(gameObject, transform.position, out m_LastMapKey);
			m_Registered = true;
		}

		protected void UpdateMap()
		{
			m_LastMapKey = m_MapManager.UpdateEntity(m_LastMapKey, transform.position, m_MapIndex);
		}

		protected void UnregisterMap()
		{
			if (m_Registered)
				m_MapManager.UnregisterEntity(m_LastMapKey, m_MapIndex);
			m_Registered = false;
		}
	}
}
