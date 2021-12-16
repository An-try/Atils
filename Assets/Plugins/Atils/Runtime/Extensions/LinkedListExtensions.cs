using System;
using System.Collections.Generic;

namespace Atils.Runtime.Extensions
{
	public static class LinkedListExtensions
	{
		/// <summary>
		/// Find the very first node that matches the predicate.
		/// </summary>
		/// <typeparam name="T">Specifies the element type of the linked list</typeparam>
		/// <param name="linkedList">The linked list in which the search occurs</param>
		/// <param name="predicate">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>Linked list node</returns>
		public static LinkedListNode<T> FindFromFirst<T>(this LinkedList<T> linkedList, Predicate<T> predicate)
		{
			LinkedListNode<T> node = linkedList.First;

			while (node != null)
			{
				if (predicate(node.Value))
				{
					return node;
				}
				node = node.Next;
			}

			return null;
		}

		/// <summary>
		/// Find the very last node that matches the predicate.
		/// </summary>
		/// <typeparam name="T">Specifies the element type of the linked list</typeparam>
		/// <param name="linkedList">The linked list in which the search occurs</param>
		/// <param name="predicate">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>Linked list node</returns>
		public static LinkedListNode<T> FindFromLast<T>(this LinkedList<T> linkedList, Predicate<T> predicate)
		{
			LinkedListNode<T> node = linkedList.Last;

			while (node != null)
			{
				if (predicate(node.Value))
				{
					return node;
				}
				node = node.Previous;
			}

			return null;
		}

		/// <summary>
		/// Swap the current element with its next element.
		/// </summary>
		/// <typeparam name="T">Specifies the element type of the linked list</typeparam>
		/// <param name="linkedList">The linked list in which the swap occurs</param>
		/// <param name="item">Item to swap</param>
		public static void SwapWithNext<T>(this LinkedList<T> linkedList, T item)
		{
			LinkedListNode<T> node = linkedList.Find(item);
			if (node == null)
			{
				//LogSystemFactory.CreateContext().Log("There is no such item in the linked list", LogLevel.Error);
				return;
			}

			LinkedListNode<T> nextNode = node.Next;
			if (nextNode == null)
			{
				//LogSystemFactory.CreateContext().Log("There is no next node in the linked list", LogLevel.Error);
				return;
			}

			linkedList.Remove(node);
			linkedList.AddAfter(nextNode, node);
		}

		/// <summary>
		/// Swap the current element with its previous element.
		/// </summary>
		/// <typeparam name="T">Specifies the element type of the linked list</typeparam>
		/// <param name="linkedList">The linked list in which the swap occurs</param>
		/// <param name="item">Item to swap</param>
		public static void SwapWithPrevious<T>(this LinkedList<T> linkedList, T item)
		{
			LinkedListNode<T> node = linkedList.Find(item);
			if (node == null)
			{
				//LogSystemFactory.CreateContext().Log("There is no such item in the linked list", LogLevel.Error);
				return;
			}

			LinkedListNode<T> previousNode = node.Previous;
			if (previousNode == null)
			{
				//LogSystemFactory.CreateContext().Log("There is no previous node in the linked list", LogLevel.Error);
				return;
			}

			linkedList.Remove(node);
			linkedList.AddBefore(previousNode, node);
		}
	}
}
