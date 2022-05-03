namespace CustomPackages
{
	public class Unit_MapManager : MappedObject
	{
		private void Start()
		{
			RegisterMap();
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
