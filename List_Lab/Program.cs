using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace List_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePaths = Directory.GetFiles("Data");
            foreach (var path in filePaths)
            {
                Console.WriteLine(path);
                TestFromFile(path);
                Console.WriteLine("---------------------------------------------");
            }
        }

        static void TestStatic(int count)
        {
            var data = new int[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = i;
            }
        
            Test(data);
        }

        static void Test(int[] array)
        {
            var list = new MyLinkedList<int>();
            var watch = new Stopwatch();
            watch.Start();
            foreach (var d in array)
            {
                list.Add(d);
            }

            Console.WriteLine($"Time for add : {watch.ElapsedMilliseconds}");
            foreach (var d in array)
            {
                list.Remove(d);
            }

            Console.WriteLine($"Time for remove : {watch.ElapsedMilliseconds}");
        }

        static void TestFromFile(string path)
        {
            var array = File.ReadAllText(path).Split().Select(int.Parse).ToArray();
            Test(array);
        }
    }

    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    public class MyLinkedList<T> : IEnumerable<T> // односвязный список
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        public void Add(T data)
        {
            var node = new Node<T>(data);
            if (count == 0)
            {
                appendFirst(data);
                return;
            }

            if (head == null)
            {
                head = node;
            }
            else
            {
                tail.Next = node;
            }

            tail = node;
            count++;
        }

        private void appendFirst(T data)
        {
            Node<T> node = new Node<T>(data);
            node.Next = head;
            head = node;
            if (count == 0)
                tail = head;
            count++;
        }

        public bool Remove(T data)
        {
            // жадібне видалення
            var current = head;
            Node<T> previous = null;
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    // видаляємо з списку
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current.Next == null)
                            tail = previous;
                    }
                    else
                    {
                        // якщо немає попереднього, то данний елемент є першим у списку
                        head = head.Next;
                        if (head == null)
                            tail = null;
                    }

                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        public int Count
        {
            get => count;
        }

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
    }
}