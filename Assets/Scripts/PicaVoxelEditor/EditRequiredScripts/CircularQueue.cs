using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class Cqueue<T>
{
    int Head, Tail, size;
    T[] data;
    public Cqueue(int size)
    {
        this.size = size;
        Head = -1;
        Tail = -1;
        data = new T[size];
    }

    public bool isEmpty()
    {
        if(Head==-1 && Tail==-1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isFull()
    {
        if((Head+1)%size==Tail)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void push(T data)
    {
        if (isEmpty())
        {
            Head = 0;
            Tail = 0;
            this.data[Head]= data;
            Head++;
        }
        else if(isFull())
        {
            //incase something goes here
        }
        else
        {
            this.data[Head]=data;
            Head=(Head+1)%size;
        }
    }

    public T popRear()
    {
        if(isEmpty())
        {
            return data[2000];
        }
        else
        {
            T temp = data[Tail];
            Tail=(Tail+1)% size;
            if(Tail==Head)
            {
                Tail = -1;
                Head = -1;
            }
            return temp;
        }
    }

    public T popFront()
    {
        if(isEmpty())
        {
            return data[2000];
        }
        else
        {
            Head = Head - 1;
            if (Head == -1)
            {
                Head = size - 1;
            }
            T temp = data[Head];
            if (Head == Tail)
            {
                Head = -1;
                Tail = -1;
            }
            return temp;
        }
    }
}