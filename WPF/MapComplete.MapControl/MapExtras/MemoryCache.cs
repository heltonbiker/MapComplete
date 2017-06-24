using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Maps.MapExtras
{
	internal sealed class MemoryCache
	{
		private object lockObj = new object();

		private LinkedList<KeyValuePair<object, MemoryCacheValue>> age = new LinkedList<KeyValuePair<object, MemoryCacheValue>>();

		private Dictionary<object, LinkedListNode<KeyValuePair<object, MemoryCacheValue>>> items = new Dictionary<object, LinkedListNode<KeyValuePair<object, MemoryCacheValue>>>();

		private int size;

		private int maxSize = 104857600;

		private static MemoryCache instance = new MemoryCache();

		public static MemoryCache Instance
		{
			get
			{
				return MemoryCache.instance;
			}
		}

		internal int MaxSize
		{
			get
			{
				return this.maxSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxSize = value;
				this.Trim();
			}
		}

		internal IEnumerable<MemoryCacheValue> ObjectsByAge
		{
			get
			{
				foreach (KeyValuePair<object, MemoryCacheValue> current in this.age)
				{
					KeyValuePair<object, MemoryCacheValue> keyValuePair = current;
					yield return keyValuePair.Value;
				}
				yield break;
			}
		}

		internal MemoryCache()
		{
		}

		public void Add(object key, MemoryCacheValue item)
		{
			lock (this.lockObj)
			{
				this.InternalAdd(key, item);
				this.Trim();
			}
		}

		public T GetValue<T>(object key) where T : MemoryCacheValue
		{
			lock (this.lockObj)
			{
				LinkedListNode<KeyValuePair<object, MemoryCacheValue>> linkedListNode;
				if (this.items.TryGetValue(key, out linkedListNode))
				{
					this.age.Remove(linkedListNode);
					this.age.AddFirst(linkedListNode);
					return linkedListNode.Value.Value as T;
				}
			}
			return default(T);
		}

		public void Remove(object key)
		{
			lock (this.lockObj)
			{
				this.InternalRemove(key);
			}
		}

		public void Replace(object key, MemoryCacheValue item)
		{
			lock (this.lockObj)
			{
				this.InternalRemove(key);
				this.InternalAdd(key, item);
				this.Trim();
			}
		}

		private void Trim()
		{
			lock (this.lockObj)
			{
				while (this.size > this.maxSize)
				{
					LinkedListNode<KeyValuePair<object, MemoryCacheValue>> last = this.age.Last;
					this.age.RemoveLast();
					this.items.Remove(last.Value.Key);
					last.Value.Value.SizeChanged -= new EventHandler<MemoryCacheObjectSizeChangedEventArgs>(this.CacheEntrySizeChanged);
					Interlocked.Add(ref this.size, -last.Value.Value.Size);
				}
			}
		}

		private void InternalAdd(object key, MemoryCacheValue item)
		{
			LinkedListNode<KeyValuePair<object, MemoryCacheValue>> linkedListNode = this.age.AddFirst(new KeyValuePair<object, MemoryCacheValue>(key, item));
			this.items.Add(key, linkedListNode);
			linkedListNode.Value.Value.SizeChanged += new EventHandler<MemoryCacheObjectSizeChangedEventArgs>(this.CacheEntrySizeChanged);
			Interlocked.Add(ref this.size, item.Size);
		}

		private void InternalRemove(object key)
		{
			LinkedListNode<KeyValuePair<object, MemoryCacheValue>> linkedListNode;
			if (this.items.TryGetValue(key, out linkedListNode))
			{
				this.items.Remove(key);
				this.age.Remove(linkedListNode);
				linkedListNode.Value.Value.SizeChanged -= new EventHandler<MemoryCacheObjectSizeChangedEventArgs>(this.CacheEntrySizeChanged);
				Interlocked.Add(ref this.size, -linkedListNode.Value.Value.Size);
			}
		}

		private void CacheEntrySizeChanged(object sender, MemoryCacheObjectSizeChangedEventArgs e)
		{
			if (Interlocked.Add(ref this.size, ((MemoryCacheValue)sender).Size - e.PreviousSize) > this.maxSize)
			{
				this.Trim();
			}
		}
	}
}
