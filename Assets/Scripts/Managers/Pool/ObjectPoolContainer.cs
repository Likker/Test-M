public class ObjectPoolContainer<T>
{
	public T Item { get; }

	public bool Used { get; private set; }

	public ObjectPoolContainer(T item)
	{
		Item = item;
		Used = false;
	}

	public void Consume()
	{
		Used = true;
	}

	public void Release()
	{
		Used = false;
	}
}