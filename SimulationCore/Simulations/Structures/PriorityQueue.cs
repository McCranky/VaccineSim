using System.Collections.Generic;

namespace SimulationLib.Simulations.Structures
{
    public interface IPrioritizable
    {
        double Priority { get; }
    }

    public class PriorityQueue<TItem> where TItem : IPrioritizable
    {
        public LinkedList<TItem> Items { get; set; } = new LinkedList<TItem>();
        public int Count => Items.Count;

        public void Enqueue(TItem item)
        {
            var node = new LinkedListNode<TItem>(item);
            if (Items.First == null)
            {
                Items.AddFirst(node);
            }
            else
            {
                var itemNode = Items.First;
                while (itemNode.Next != null && itemNode.Value.Priority < item.Priority)
                {
                    itemNode = itemNode.Next;
                }

                if (itemNode.Value.Priority <= item.Priority)
                {
                    Items.AddAfter(itemNode, node);
                }
                else
                {
                    Items.AddBefore(itemNode, node);
                }
            }
        }

        public TItem Dequeue()
        {
            if (Count > 0)
            {
                var itemValue = Items.First.Value;
                Items.RemoveFirst();
                return itemValue;
            }

            return default;
        }
    }
}
