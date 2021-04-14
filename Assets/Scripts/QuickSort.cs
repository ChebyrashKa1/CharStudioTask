public static class QuickSort<T> where T : System.IComparable
{
    public static void Sort(UserData[] ar, int lBound, int uBound, bool scoreCheck)
    {
        if (lBound < uBound)
        {
            var loc = Partition(ar, lBound, uBound, scoreCheck);
            Sort(ar, lBound, loc - 1, scoreCheck);
            Sort(ar, loc + 1, uBound, scoreCheck);
        }
    }

    private static int Partition(UserData[] ar, int lBound, int uBound, bool scoreCheck)
    {
        var start = lBound;
        var end = uBound;

        var pivot = ar[uBound];

        while (start < end)
        {

            while (ar[start].CompareTo(pivot, scoreCheck) < 0)
            {
                start++;
            }

            while (ar[end].CompareTo(pivot, scoreCheck) > 0)
            {
                end--;
            }

            if (start < end)
            {
                if (ar[start].CompareTo(ar[end], scoreCheck) == 0)
                {
                    start++;
                }
                else
                {
                    swap(ar, start, end);
                }
            }
        }

        return end;
    }

    private static void swap(UserData[] ar, int i, int j)
    {
        var temp = ar[i];
        ar[i] = ar[j];
        ar[j] = temp;
    }
}