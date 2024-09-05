using System.Collections;

namespace FPSoundLib.Utils.DLinkList
{
	internal class DLinkListEnum<T>(DLinkList<T> list) : IEnumerator<Node<T>>
	{
		private int _position = -1;

		/// <inheritdoc />
		public bool MoveNext()
		{
			if (Current.Next is not { } next) 
				return false;

			if (_position++ != -1) Current = next;
			return true;

		}

		/// <inheritdoc />
		public void Reset()
		{
			Current = list.First ?? throw new InvalidOperationException();
			_position = -1;
		}

		/// <inheritdoc />
		public Node<T> Current { get; private set;  } = list.First ?? throw new InvalidOperationException();

		/// <inheritdoc />
		object IEnumerator.Current => Current;

		/// <inheritdoc />
		public void Dispose() { }
	}
}
