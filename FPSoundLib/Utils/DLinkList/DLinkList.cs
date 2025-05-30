﻿using System.Collections;

namespace FPSoundLib.Utils.DLinkList
{
	public class DLinkList<T>: IList<Node<T>> 
	{
		public Node<T>? First { get; private set; }
		public Node<T>? Last { get; private set; }

		/// <inheritdoc />
		public IEnumerator<Node<T>> GetEnumerator()
		{
			return new DLinkListEnum<T>(this);
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(Node<T> node)
        {
            if (First == null)
				First = node;
			else
				node.AddAfterNode(Last!);
            
            Last = node;
        }

		/// <inheritdoc cref="Add(Node{T})"/>
		/// <param name="item">The item to add to the list.</param>
		public void Add(T item) => Add(new Node<T>(item));
		

		/// <inheritdoc />
		public void Clear()
		{
			foreach (Node<T> node in this)
			{
				node.Remove();
			}

			First = null;
			Last = null;
		}

		/// <inheritdoc />
		public bool Contains(Node<T> item) => this.Any(node => node.Equals(item));
		

		/// <inheritdoc />
		public void CopyTo(Node<T>[] array, int arrayIndex)
		{
			Node<T>? node = First;
			while (node != null)
			{
				array[arrayIndex++] = node;
				node = node.Next;
			}
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Node<T>? node = First;
			while (node != null)
			{
				array[arrayIndex++] = node.Value;
				node = node.Next;
			}
		}

		/// <inheritdoc />
		public bool Remove(Node<T> item)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public int Count { get => Last?.Index + 1 ?? 0; }

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <inheritdoc />
		public int IndexOf(Node<T> item)
		{
			foreach (Node<T> node in this)
			{
				if (node.Equals(item))
					return node.Index;
			}

			return -1;
		}

		/// <inheritdoc />
		public void Insert(int index, Node<T> item)
		{
			if (index == 0)
			{
				item.AddBeforeNode(First!);
				First = item;
			}
			else if (index == Count)
			{
				item.AddAfterNode(Last!);
				Last = item;
			}
			else if (index < 0 || index > Count)
			{
				throw new IndexOutOfRangeException();
			}
			else
			{
				item.AddBeforeNode(this[index]);
			}
		}

		/// <inheritdoc />
		public void RemoveAt(int index) => this[index].Remove();

		/// <inheritdoc />
		public Node<T> this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
					throw new IndexOutOfRangeException();

				foreach (Node<T> node in this)
					if (node.Index == index) 
						return node;

				throw new IndexOutOfRangeException();
			}
			set
			{
				Node<T> node = value.AddBeforeNode(this[index]);
				node.Next!.Remove();

				if (index == 0)
					First = node;
				else if (index == Count - 1)
					Last = node;
			}
		}

		/// <summary>
		/// Sets the data value of a node at a specific index.
		/// </summary>
		///	<param name="index">The index of the node to set the data value of.</param>
		/// <param name="value">The new data value to set.</param>
		/// <returns>The old value of the node.</returns>
		public T SetNode(int index, T value)
		{
			T oldValue = this[index].Value;
			this[index].Value = value;

			return oldValue;
		}
	}
}
