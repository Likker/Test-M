namespace CustomPackages
{
	public class Unit_MapManager : MappedObject
	{
		private void Start()
		{
			RegisterMap(1.0f);
		}

		private void Update()
		{
			UpdateMap();
		}

		private void OnDestroy()
		{
			UnregisterMap();
		}
	}
}
