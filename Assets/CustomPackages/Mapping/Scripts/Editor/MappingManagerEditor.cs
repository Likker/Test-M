using UnityEditor;
using UnityEngine;

namespace CustomPackages
{
	[CustomEditor(typeof(MappingManager))]
	public class MappingManagerEditor : Editor
	{
		private MappingManager m_Target;

		private void OnEnable()
		{
			if (target == null)
				return;
			m_Target = (MappingManager)target;
		}

		public override void OnInspectorGUI()
		{
			if (Application.isPlaying)
				EditorGUILayout.TextArea(m_Target.GetNames());
			else
				base.OnInspectorGUI();
		}
	}
}