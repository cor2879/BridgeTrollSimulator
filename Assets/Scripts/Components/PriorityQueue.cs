using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Text.Json;
using Unity.IO.LowLevel.Unsafe;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    [DebuggerDisplay("DebuggerDisplay()")]
    public class PriorityQueue<T> : IEnumerable<(T, double)>
    {
        private List<(T value, double priority)> elements = new List<(T, double)>();

        public int Count => elements.Count;

        public void Enqueue(T item, double priority)
        {
            elements.Add((item, priority));
            int index = elements.Count - 1;

            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;

                if (elements[parentIndex].priority <= priority)
                {
                    break;
                }

                (T parentValue, double parentPriority) = elements[parentIndex];
                elements[parentIndex] = (item, priority);
                elements[index] = (parentValue, parentPriority);
                index = parentIndex;
            }
        }

        public T Peek()
        {
            return elements[0].value;
        }

        public T Dequeue()
        {
            T result = this.Peek();
            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);

            int index = 0;

            while (true)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;

                if (leftChildIndex >= elements.Count)
                {
                    break;
                }

                int minIndex = index;

                if (elements[leftChildIndex].priority < elements[minIndex].priority)
                {
                    minIndex = leftChildIndex;
                }

                if (rightChildIndex < elements.Count && elements[rightChildIndex].priority < elements[minIndex].priority)
                {
                    minIndex = rightChildIndex;
                }

                if (minIndex == index)
                {
                    break;
                }

                (T minValue, double minPriority) = elements[minIndex];
                elements[minIndex] = elements[index];
                elements[index] = (minValue, minPriority);
                index = minIndex;
            }

            return result;
        }

        public bool Contains(T value)
        {
            return elements.Any(e => !(e.value is null) && e.value.Equals(value));
        }

        public IEnumerator<(T, double)> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public string DebuggerDisplay()
        {
            var stringBuilder = new StringBuilder();

            if (this.elements != null && this.elements.Any())
            {
                foreach (var element in elements)
                {
                    stringBuilder.AppendLine($"{{item: {element.value}, priority: {element.priority}}}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
