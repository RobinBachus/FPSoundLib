namespace FPSoundLib.Utils.DLinkList
{
	internal class Node<T>(T data): IComparable<Node<T>> where T : IEquatable<T> 
	{
		public Node<T>? Next;
		public Node<T>? Prev;
		public T Data = data;

		public int Index
		{
			get
			{
				int i = 0;
				var node = this;
				while (node.Prev != null)
				{
					node = node.Prev;
					i++;
				}

				return i;
			}
		}

		public Node<T> AddAfterNode(Node<T> previousNode)
		{
			Prev = previousNode;
			Next = previousNode.Next;

			previousNode.Next = this;
			if (Next != null) Next.Prev = this;

			return this;
		}

		public Node<T> AddBeforeNode(Node<T> nextNode)
		{
			Next = nextNode;
			Prev = nextNode.Prev;

			nextNode.Prev = this;
			if (Prev != null) Prev.Next = this;

			return this;
		}

		public void Remove()
		{
			if (Prev != null) Prev.Next = Next;
			if (Next != null) Next.Prev = Prev;
		}

		/// <inheritdoc />
		public int CompareTo(Node<T>? other) => Index.CompareTo(other?.Index);

		/// <inheritdoc />
		public override bool Equals(object? obj)
		{
			if (obj is Node<T> node)
				return node.Data.Equals(Data);

			return false;
		}

		public static bool operator ==(Node<T>? left, Node<T>? right) => left?.Equals(right) ?? right is null;

		public static bool operator !=(Node<T>? left, Node<T>? right) => !(left == right);

		/// <inheritdoc />
		public override int GetHashCode() => default;
	}
}
