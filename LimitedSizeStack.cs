using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class StackItem<T>
{
	public T Val { get; set; }
	public StackItem<T> Next { get; set; }
	public StackItem<T> Prev { get; set; }
}

public class LimitedSizeStack<T>
{
	private StackItem<T> head;
	private StackItem<T> tail;
	private int limit;
	private int count;
	
	public LimitedSizeStack(int undoLimit)
	{
		limit = undoLimit;
	}

	public void Push(T item)
	{
		if (head == null && limit != 0)
		{
			tail = head = new StackItem<T>() { Val = item, Next = null, Prev = null };
			count++;
		}
		else if (limit != 0)
		{
			var temp = new StackItem<T>() { Val = item, Next = null, Prev = tail };
			tail.Next = temp;
			tail = temp;
			count++;
			if (count > limit)
			{
				(head.Next).Prev = null;
				head = head.Next;
				count--;
			}
		}
	}

	public T Pop()
	{
		T temp = tail.Val;
		if (tail.Prev == null)
		{
			head = tail = null;
			count--;
			return temp;
		}
		(tail.Prev).Next = null;
		tail = tail.Prev;
		count--;
		return temp;	
	}

	public int Count { get { return count; } }
}