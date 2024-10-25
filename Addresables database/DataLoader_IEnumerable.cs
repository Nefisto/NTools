using System.Collections;
using System.Collections.Generic;

public partial class DataLoader<T>
{
    public IEnumerator<T> GetEnumerator() => Data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}