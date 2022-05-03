using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomPackages
{
	 public class MappingManager : SingletonMB<MappingManager>
    {
        private const int c_MapComputationFactor = 10000;

        [SerializeField] private float m_TileSize = 5f;
        [SerializeField] private float m_RadiusDetection = 1.0f;

        public Action<GameObject> OnEnter;
        public Action<GameObject> OnExit;

        private List<MappedObject> m_Entities;
        private Dictionary<int, List<int>> m_Datas;
        private bool m_IsInitialized = false;
        private float m_SqrTileFactor;

        private List<MappedObject> m_SearchBuffer;
        private List<MappedObject> m_PrevBuffer;
        private List<MappedObject> m_TMPBuffer;

        #region Exposed

        private void Awake()
        {
            m_SearchBuffer = new List<MappedObject>();
            m_PrevBuffer = new List<MappedObject>();
            m_TMPBuffer = new List<MappedObject>();
        }

        public int RegisterEntity(MappedObject entity, Vector3 position, out int firstKey)
        {
            if (!m_IsInitialized)
                Init();

            firstKey = InitEntity(position, entity);

            return m_Entities.Count - 1;
        }

        public void UnregisterEntity(int lastKey, int entityIndex)
        {
            RemoveOldKey(lastKey, entityIndex);
            m_PrevBuffer.Remove(m_Entities[entityIndex]);
            m_Entities[entityIndex] = null;
        }

        private int InitEntity(Vector3 position, MappedObject entity)
        {
            m_Entities.Add(entity);
            int entityIndex = m_Entities.Count - 1;

            int newKey = ComputeKey(position);
            AddNewKey(newKey, entityIndex);

            return newKey;
        }

        public int UpdateEntity(int lastKey, Vector3 position, int entityIndex)
        {
            int newKey = ComputeKey(position);

            RemoveOldKey(lastKey, entityIndex);
            AddNewKey(newKey, entityIndex);

            return newKey;
        }

        public void Check(Vector3 position)
        {
            FindEntities(position, m_RadiusDetection, ref m_SearchBuffer);

            foreach (var entity in m_SearchBuffer)
            {
                if (!m_PrevBuffer.Contains(entity))
                {
                    m_PrevBuffer.Add(entity);
                    OnEnter?.Invoke(entity.gameObject);
                }
            }

            foreach (var entity in m_PrevBuffer)
            {
                if (!m_SearchBuffer.Contains(entity))
                    m_TMPBuffer.Add(entity);
            }

            foreach (var entity in m_TMPBuffer)
            {
                m_PrevBuffer.Remove(entity);
                OnExit?.Invoke(entity.gameObject);
            }

            m_TMPBuffer.Clear();
        }

        public bool IsEntityPresent(Vector3 position, float sqrRadius, int layer = -1)
        {
            if (m_Datas == null)
                return false;


            int passCount = GetPassCount(sqrRadius);

            int XBase = Mathf.RoundToInt(position.x / m_TileSize);
            int ZBase = Mathf.RoundToInt(position.z / m_TileSize);

            for (int XIndex = -passCount; XIndex <= passCount; ++XIndex)
            {
                for (int ZIndex = -passCount; ZIndex <= passCount; ++ZIndex)
                {
                    int XCurrent = XBase + XIndex;
                    int ZCurrent = ZBase + ZIndex;

                    float x = position.x - (m_TileSize * XCurrent);
                    float z = position.z - (m_TileSize * ZCurrent);

                    float sqrMagnitude = x * x + z * z;
                    if (sqrMagnitude > sqrRadius + m_SqrTileFactor)
                        continue;

                    int key = XCurrent * c_MapComputationFactor + ZCurrent;

                    if (!m_Datas.TryGetValue(key, out var entityIndices))
                        continue;

                    foreach (int entityIndex in entityIndices)
                    {
                        MappedObject entity = m_Entities[entityIndex];
                        if (entity == null || (layer != -1 && entity.gameObject.layer != layer))
                            continue;

                        float entityDiff = Vector3.Distance(entity.transform.position, position);

                        if (entityDiff < sqrRadius + entity.m_Radius * entity.m_Radius)
                            return true;
                    }
                }
            }

            return false;
        }

        public void FindEntities(Vector3 position, float sqrRadius, ref List<MappedObject> results, int layer = -1)
        {
            if (results == null)
                results = new List<MappedObject>();
            else
                results.Clear();

            if (m_Datas == null)
                return;

            int passCount = GetPassCount(sqrRadius);

            int XBase = Mathf.RoundToInt(position.x / m_TileSize);
            int ZBase = Mathf.RoundToInt(position.z / m_TileSize);

            for (int XIndex = -passCount; XIndex <= passCount; ++XIndex)
            {
                for (int ZIndex = -passCount; ZIndex <= passCount; ++ZIndex)
                {
                    int XCurrent = XBase + XIndex;
                    int ZCurrent = ZBase + ZIndex;

                    float x = position.x - (m_TileSize * XCurrent);
                    float z = position.z - (m_TileSize * ZCurrent);

                    float sqrMagnitude = x * x + z * z;

                    if (sqrMagnitude > sqrRadius + m_SqrTileFactor)
                        continue;

                    int key = XCurrent * c_MapComputationFactor + ZCurrent;

                    if (!m_Datas.TryGetValue(key, out var entityIndices))
                        continue;

                    foreach (int entityIndex in entityIndices)
                    {
                        MappedObject entity = m_Entities[entityIndex];
                        if (entity == null || (layer != -1 && entity.gameObject.layer != layer))
                            continue;
                        
                        float entityDiff = Vector3.Distance(entity.transform.position, position);

                        if (entityDiff < entity.m_Radius * entity.m_Radius + sqrRadius)
                            results.Add(entity);
                    }
                }
            }
        }

        public void Clear()
        {
            m_SearchBuffer?.Clear();
            m_PrevBuffer?.Clear();
            m_TMPBuffer?.Clear();

            m_Entities?.Clear();
            m_Datas?.Clear();
        }

        #endregion

        #region Internal

        private void Init()
        {
            m_IsInitialized = true;
            m_Datas = new Dictionary<int, List<int>>();
            m_Entities = new List<MappedObject>();

            m_SqrTileFactor = m_TileSize / Mathf.Sqrt(Mathf.PI);
            m_SqrTileFactor *= m_SqrTileFactor;
        }

        private void RemoveOldKey(int oldKey, int entityIndex)
        {
            if (!m_Datas.TryGetValue(oldKey, out var indices))
                return;

            if (indices.Count == 1)
            {
                indices.Clear();
                m_Datas.Remove(oldKey);
            }
            else
            {
                indices.Remove(entityIndex);
            }
        }

        private void AddNewKey(int newKey, int entityIndex)
        {
            if (m_Datas.TryGetValue(newKey, out var indexes))
            {
                indexes.Add(entityIndex);
            }
            else
            {
                m_Datas.Add(newKey, new List<int> {entityIndex});
            }
        }

        private int GetPassCount(float sqrRadius)
        {
            int passCount = Mathf.RoundToInt(sqrRadius / (m_TileSize * m_TileSize));
            if (passCount == 0) passCount = 1;
            return passCount;
        }

        #endregion

        private int ComputeKey(Vector3 position)
        {
            return ComputeKey(Mathf.RoundToInt(position.x / m_TileSize), Mathf.RoundToInt(position.z / m_TileSize));
        }

        private static int ComputeKey(int x, int y)
        {
            return x * c_MapComputationFactor + y;
        }

#if UNITY_EDITOR
        private readonly System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();

        public string GetNames()
        {
            if (m_Entities == null)
                return string.Empty;

            _stringBuilder.Clear();
            foreach (var entity in m_Entities)
            {
                _stringBuilder.Append(entity != null ? entity.gameObject.name : "<null>");
                _stringBuilder.Append('\n');
            }

            return _stringBuilder.ToString();
        }
#endif
    }
}
