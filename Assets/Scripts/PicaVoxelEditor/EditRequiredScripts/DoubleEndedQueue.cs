
using UnityEngine.UIElements;

public class Node<T>
{
    public T Value;
    public Node<T> next, last;


    public Node(T value)
    {
        Value = value;
        next = null;
        last = null;
    }

    public Node()
    {
        next = null;
        last = null;
    }



}

public class Deque<T>
{
    Node<T> Head = new Node<T>();
    Node<T> Tail = new Node<T>();

    public void AddRear(T value)
    {
        if (Head == Tail)
        {
            Node<T> temp = new Node<T>(value);
            temp.next = Tail;
            Tail.last = temp;
            Tail = temp;
            Head.last = temp;
            Tail.next = Head;
        }
        else
        {
            Node<T> temp = new Node<T>(value);
            temp.last = null;
            temp.next = Tail;
            Tail = temp;
        }
    }

    public Node<T> RemoveRear(T value)
    {
        if (checkEmpty())
        {
            Node<T> temp = Tail;
            Tail.next.last = null;
            Tail = Tail.next;
            return temp;
        }
        return Tail;
    }

    public bool checkEmpty() { return Tail.next == null ? false : true; }

    public void AddFront(T value)
    {
        if (Head == Tail)
        {
            Node<T> temp = new Node<T>(value);
            temp.next = null;
            temp.last = Tail;
            Tail.next = temp;
            Head = temp;
        }
        else
        {
            Node<T> temp = new Node<T>(value);
            temp.next = null;
            temp.last = Head;
            Head = temp;
        }
    }

    public Node<T> RemoveFront()
    {
        if (checkEmpty())
        {
            Node<T> temp = Head;
            Head.last.next = null;
            Head = Head.last;
            return temp;
        }
        return Head;
    }

    public void find()
    {

    }
}