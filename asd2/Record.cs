namespace asd2
{
    public class Record<T>
    {
        public T Data;
        public int Key { get; private set; }
        
        public Record(T data, int key)
        {
            Data = data;
            Key = key;
        }
    }
}