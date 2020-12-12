namespace Core.Abstracts
{
    public class StaticConstructor<T> where T : new()
    {
        private static T _instance;
        
        public static T New()
        {
            return _instance ??= new T();
        }
    }
}