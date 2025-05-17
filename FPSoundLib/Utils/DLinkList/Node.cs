namespace FPSoundLib.Utils.DLinkList
{
	public class Node<T>(T data): IComparable<Node<T>> 
	{
		public Node<T>? Next;
		public Node<T>? Prev;
		public T Value = data;

		public int Index
		{
			get
			{
				int i = 0;
				Node<T>? node = this;
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
			return obj is Node<T> { Value: IEquatable<T> data } && data.Equals(Value);
		}

		public static bool operator ==(Node<T>? left, Node<T>? right) => left?.Equals(right) ?? right is null;

		public static bool operator !=(Node<T>? left, Node<T>? right) => !(left == right);

		public static Node<T>? operator ++(Node<T> node) => node.Next;

		public static Node<T>? operator --(Node<T> node) => node.Prev;

		/// <inheritdoc />
		public override int GetHashCode() => default;
	}
}
