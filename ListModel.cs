using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

interface ICommand
{
	void Execute();
	void Undo();
}

class AddItem<TItem> : ICommand
{
	private TItem item;
	private int index;
	private List<TItem> list;

	public AddItem(TItem item, int index, List<TItem> list)
	{
		this.item = item;
		this.index = index;
		this.list = list;
	}
	
	public void Execute()
	{
		list.Add(item);
	}

	public void Undo()
	{
		list.RemoveAt(index);
	}
}

class RemoveItem<TItem> : ICommand
{
	private TItem item;
	private int index;
	private List<TItem> list;

	public RemoveItem(TItem item, int index, List<TItem> list)
	{
		this.item = item;
		this.index = index;
		this.list = list;
	}
	public void Execute()
	{
		list.RemoveAt(index);
	}

	public void Undo()
	{
		list.Insert(index, item);
	}
}

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;
	private LimitedSizeStack<ICommand> stack; 
        
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		stack = new LimitedSizeStack<ICommand>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		AddItem<TItem> cmd = new AddItem<TItem>(item, Items.Count, Items);
		cmd.Execute();
		stack.Push(cmd);
	}

	public void RemoveItem(int index)
	{
		RemoveItem<TItem> cmd = new RemoveItem<TItem>(Items[index], index, Items);
		cmd.Execute();
		stack.Push(cmd);
	}

	public bool CanUndo()
	{
		return stack.Count!=0;
	}

	public void Undo()
	{
		ICommand cmd = stack.Pop();
		cmd.Undo();
	}
}