using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomPackages
{
	public class MappingManager : SingletonMB<MappingManager>
	{
		private const int c_MapComputationFactor = 10000;

		public float m_TileSize        = 5f;

		private List<MappedObject> m_Entities;
		private Hashtable          m_Datas;
		private bool               m_IsInitialized = false;
		private float              m_SqrTileFactor;

		private List<MappedObject> m_SearchBuffer;
		private List<MappedObject> m_PrevBuffer;
		private List<MappedObject> m_TMPBuffer;
		
		#region Exposed

		private void Awake()
		{
			m_SearchBuffer = new List<MappedObject>();
			m_PrevBuffer   = new List<MappedObject>();
			m_TMPBuffer    = new List<MappedObject>();
		}
		
		public int RegisterEntity(MappedObject _Entity, Vector3 _Position, out int _FirstKey)
		{
			if (!m_IsInitialized)
				Init();

			_FirstKey = InitEntity(_Position, _Entity);

			return m_Entities.Count - 1;
		}

		public void UnregisterEntity(int _LastKey, int _EntityIndex)
		{
			RemoveOldKey(_LastKey, _EntityIndex);
			m_Entities[_EntityIndex] = null;
		}

		private int InitEntity(Vector3 _Position, MappedObject _Entity)
		{
			m_Entities.Add(_Entity);
			int entityIndex = m_Entities.Count - 1;

			int newKey = Mathf.RoundToInt(_Position.x / m_TileSize) * c_MapComputationFactor + Mathf.RoundToInt(_Position.z / m_TileSize);
			AddNewKey(newKey, entityIndex);

			return newKey;
		}

		public int UpdateEntity(int _LastKey, Vector3 _Position, int _EntityIndex)
		{
			int newKey = Mathf.RoundToInt(_Position.x / m_TileSize) * c_MapComputationFactor + Mathf.RoundToInt(_Position.z / m_TileSize);
			RemoveOldKey(_LastKey, _EntityIndex);
			AddNewKey(newKey, _EntityIndex);
			return newKey;
		}
		
		public bool IsEntityPresent(Vector3 _Position, float _SqrRadius, int _Layer = -1)
		{
			if (m_Datas == null)
				return false;

			int passCount = GetPassCount(_SqrRadius);

			int XBase = Mathf.RoundToInt(_Position.x / m_TileSize);
			int ZBase = Mathf.RoundToInt(_Position.z / m_TileSize);
			
			for (int XIndex = -passCount; XIndex <= passCount; ++XIndex)
			{
				for (int ZIndex = -passCount; ZIndex <= passCount; ++ZIndex)
				{
					int XCurrent = XBase + XIndex;
					int ZCurrent = ZBase + ZIndex;

					float x = _Position.x - (m_TileSize * XCurrent);
					float z = _Position.z - (m_TileSize * ZCurrent);

					float sqrMagnitude = x * x + z * z;
					if (sqrMagnitude > _SqrRadius + m_SqrTileFactor)
						continue;

					int key = XCurrent * c_MapComputationFactor + ZCurrent;
					
					if (m_Datas[key] == null)
						continue;


					List<int> entityIndexes = (List<int>)m_Datas[key];

					for (int index = 0; index < entityIndexes.Count; index++)
					{
						int          entityIndex = entityIndexes[index];
						MappedObject entity      = m_Entities[entityIndex];

						if (entity == null || (_Layer != -1 && entity.gameObject.layer != _Layer))
							continue;

						float entityDiff = Vector3.Distance(entity.transform.position, _Position);

						if (entityDiff < (_SqrRadius + entity.m_Radius))
							return true;
					}
				}
			}

			return false;
		}

		public void FindEntities(Vector3 _Position, float _SqrRadius, ref List<MappedObject> _Results, int _Layer = -1)
		{
			if (_Results == null)
				_Results = new List<MappedObject>();
			else
				_Results.Clear();

			if (m_Datas == null)
				return;

			int passCount = GetPassCount(_SqrRadius);

			int XBase = Mathf.RoundToInt(_Position.x / m_TileSize);
			int ZBase = Mathf.RoundToInt(_Position.z / m_TileSize);

			for (int XIndex = -passCount; XIndex <= passCount; ++XIndex)
			{
				for (int ZIndex = -passCount; ZIndex <= passCount; ++ZIndex)
				{
					int XCurrent = XBase + XIndex;
					int ZCurrent = ZBase + ZIndex;

					float x = _Position.x - (m_TileSize * XCurrent);
					float z = _Position.z - (m_TileSize * ZCurrent);

					float sqrMagnitude = x * x + z * z;

					if (sqrMagnitude > _SqrRadius + m_SqrTileFactor)
						continue;

					int key = XCurrent * c_MapComputationFactor + ZCurrent;

					if (m_Datas[key] == null)
						continue;

					List<int> entityIndexes = (List<int>)m_Datas[key];

					for (int index = 0; index < entityIndexes.Count; index++)
					{
						int          entityIndex = entityIndexes[index];
						MappedObject entity      = m_Entities[entityIndex];
						if (entity == null || (_Layer != -1 && entity.gameObject.layer != _Layer))
							continue;
						
						float entityDiff = Vector3.Distance(entity.transform.position, _Position);

						if (entityDiff < (_SqrRadius + entity.m_Radius))
							_Results.Add(entity);
					}
				}
			}
		}

		public void Clear()
		{
			if (m_Entities != null)
				m_Entities.Clear();

			if (m_Datas != null)
				m_Datas.Clear();
		}

		#endregion

		#region Internal

		private void Init()
		{
			m_IsInitialized = true;
			m_Datas = new Hashtable();
			m_Entities = new List<MappedObject>();

			m_SqrTileFactor = m_TileSize / Mathf.Sqrt(Mathf.PI);
			m_SqrTileFactor *= m_SqrTileFactor;
		}

		private void RemoveOldKey(int _OldKey, int _EntityIndex)
		{
			List<int> indexes = (List<int>)m_Datas[_OldKey];

			if (indexes == null)
				return;

			if (indexes.Count == 1)
			{
				indexes.Clear();
				m_Datas[_OldKey] = null;
			}
			else
			{
				indexes.Remove(_EntityIndex);
				m_Datas[_OldKey] = indexes;
			}
		}

		private void AddNewKey(int _NewKey, int _EntityIndex)
		{
			if (m_Datas[_NewKey] != null)
			{
				List<int> indexes = (List<int>)m_Datas[_NewKey];
				indexes.Add(_EntityIndex);
			}
			else
			{
				List<int> indexes = new List<int>();
				indexes.Add(_EntityIndex);
				m_Datas[_NewKey] = indexes;
			}
		}

		private int GetPassCount(float _SqrRadius)
		{
			int passCount = Mathf.RoundToInt(_SqrRadius / (m_TileSize * m_TileSize));
			if (passCount == 0) passCount = 1;
			return passCount;
		}

		#endregion

#if UNITY_EDITOR

		public string GetNames()
		{
			if (m_Entities == null)
				return string.Empty;

			string names = "";
			for (int i = 0; i < m_Entities.Count; ++i)
			{
				if (m_Entities[i] == null)
					names += "<NULL>\n";
				else
					names += m_Entities[i].gameObject.name + "\n";
			}
			return names;
		}

#endif
	}
}
