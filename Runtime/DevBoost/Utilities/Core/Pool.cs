/********************************************************************
 created:	2014/03/14
 filename:	Pool.cs
 author:		Benjamin
 purpose:	[]
*********************************************************************/

using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DevBoost.Utilities
{
	public class Pool<T>
	{
		private readonly List<T> items = new List<T>();
		private readonly Queue<T> freeItems = new Queue<T>();

		private readonly Func<T> createItemAction;

		public Pool(Func<T> createItemAction)
		{
			Assert.IsNotNull(createItemAction, "pool object is null");
			this.createItemAction = createItemAction;
		}

		public Pool(Func<T> createItemAction, int initSize)
		{
			Assert.IsNotNull(createItemAction, "pool object is null");
			this.createItemAction = createItemAction;

			freeItems = new Queue<T>(initSize);
		}

		public void FlagFreeItem(T item)
		{
			freeItems.Enqueue(item);
			items.Remove(item);
		}

		public T GetFreeItem()
		{
			if (freeItems.Count == 0) {
				T newItem = createItemAction();
				items.Add(newItem);
				return newItem;
			}
			else
            {
				T newItem = freeItems.Dequeue();
				items.Add(newItem);
				return newItem;
			}
		}

		public List<T> Items
		{
			get { return items; }
		}

		public void Clear() 
		{
			items.Clear();
			freeItems.Clear();
		}
	}
}