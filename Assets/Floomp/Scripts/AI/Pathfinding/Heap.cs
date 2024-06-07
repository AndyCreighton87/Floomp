using System;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;
    public int Count => currentItemCount;

    public Heap(int _capacity) {
        items = new T[_capacity];
    }

    public void Add(T _item) {
        _item.HeapIndex = currentItemCount;
        items[currentItemCount] = _item;
        SortUp(_item);
        currentItemCount++;
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public bool Contains(T _item) {
        return Equals(items[_item.HeapIndex], _item);
    }

    public void UpdateItem(T _item) {
        SortUp(_item);
    }

    private void SortDown(T _item) {
        while (true) {
            int childIndexLeft = _item.HeapIndex * 2 + 1;
            int childIndexRight = _item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            
            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                if (_item.CompareTo(items[swapIndex]) < 0) {
                    Swap(_item, items[swapIndex]);
                }
                else {
                    return;
                }
            }
            else {
                return;
            }
        }
    }

    private void SortUp(T _item) {
        int parentIndex  = (_item.HeapIndex - 1) / 2;
        
        while (true) {
            T parentItem = items[parentIndex];
            if (_item.CompareTo(parentItem) > 0) {
                Swap(_item, parentItem);
            }
            else {
                break;
            }

            parentIndex = (_item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T _itemA, T _itemB) {
        items[_itemA.HeapIndex] = _itemB;
        items[_itemB.HeapIndex] = _itemA;

        int itemAIndex = _itemA.HeapIndex;
        _itemA.HeapIndex = _itemB.HeapIndex;
        _itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    public int HeapIndex { get; set; }

}
