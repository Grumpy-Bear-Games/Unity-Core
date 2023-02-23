namespace Games.GrumpyBear.Core
{
    public partial class ObservablesTest
    {
        public class CallbackObserver<T>
        {
            public T Value;
            public int CallCount = 0;

            public void Reset()
            {
                Value = default;
                CallCount = 0;
            }

            public void Callback(T value)
            {
                Value = value;
                CallCount++;
            }
        }
        
        public class CallbackObserver
        {
            public int CallCount = 0;

            public void Reset()
            {
                CallCount = 0;
            }

            public void Callback()
            {
                CallCount++;
            }
        }
        
    }
}
