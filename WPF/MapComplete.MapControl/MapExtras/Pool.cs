using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Microsoft.Maps.MapExtras
{
	internal class Pool<TKey, TValue>
	{
		private struct PooledItem
		{
			public TValue Item;

			public DateTime TimeWhenAdded;
		}

		private const int SecondsBeforeRemoval = 10;

		private const int MaxItemCount = 100;

		private Dictionary<TKey, LinkedList<Pool<TKey, TValue>.PooledItem>> pooledItems = new Dictionary<TKey, LinkedList<Pool<TKey, TValue>.PooledItem>>();

		private DispatcherTimer garbageCollectionTimer;

		public Pool()
		{
			this.garbageCollectionTimer = new DispatcherTimer();
			this.garbageCollectionTimer.Interval = new TimeSpan(0, 0, 1);
			this.garbageCollectionTimer.Tick += new EventHandler(this.GarbageCollectionTimer_Tick);
			this.garbageCollectionTimer.Start();
		}

		public void Add(TKey key, TValue value)
		{
			LinkedList<Pool<TKey, TValue>.PooledItem> linkedList;
			if (!this.pooledItems.TryGetValue(key, out linkedList))
			{
				linkedList = new LinkedList<Pool<TKey, TValue>.PooledItem>();
				this.pooledItems[key] = linkedList;
			}
			linkedList.AddLast(new Pool<TKey, TValue>.PooledItem
			{
				TimeWhenAdded = DateTime.UtcNow,
				Item = value
			});
			if (linkedList.Count > 100)
			{
				linkedList.RemoveFirst();
			}
		}

		public TValue Get(TKey key)
		{
			Pool<TKey, TValue>.PooledItem pooledItem = default(Pool<TKey, TValue>.PooledItem);
			LinkedList<Pool<TKey, TValue>.PooledItem> linkedList;
			if (this.pooledItems.TryGetValue(key, out linkedList) && linkedList.Count > 0)
			{
				pooledItem = linkedList.First.Value;
				linkedList.RemoveFirst();
			}
			return pooledItem.Item;
		}

		private void GarbageCollectionTimer_Tick(object sender, EventArgs e)
		{
			DateTime utcNow = DateTime.UtcNow;
			foreach (LinkedList<Pool<TKey, TValue>.PooledItem> current in this.pooledItems.Values)
			{
				LinkedListNode<Pool<TKey, TValue>.PooledItem> next;
				for (LinkedListNode<Pool<TKey, TValue>.PooledItem> linkedListNode = current.First; linkedListNode != null; linkedListNode = next)
				{
					next = linkedListNode.Next;
					if ((utcNow - linkedListNode.Value.TimeWhenAdded).TotalSeconds > 10.0)
					{
						current.Remove(linkedListNode);
					}
				}
			}
		}
	}
}
