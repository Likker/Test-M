using UnityEngine;

namespace CustomPackages
{
	public abstract class MappedObject : MonoBehaviour
	{
		public float m_Radius { get; private set; }
		
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

		protected void RegisterMap(float _Radius)
		{
			m_Radius     = _Radius;
			m_MapIndex   = m_MapManager.RegisterEntity(this, transform.position, out m_LastMapKey);
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
		
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, m_Radius);
		}
	}
}
