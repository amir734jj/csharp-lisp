using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core
{
    public class Contour<T>
    {
        private readonly Dictionary<string, T> _table = new Dictionary<string, T>();

        private Contour<T> _parent;

        public bool Lookup(string key, out T result)
        {
            if (_table.ContainsKey(key))
            {
                result = _table[key];
                return true;
            }

            if (_parent != null)
            {
                return _parent.Lookup(key, out result);
            }

            result = default;
            
            return false;
        }
        
        public T Add(string key, T value)
        {
            return _table[key] = value;
        }

        public Contour<T> Pop()
        {
            return _parent;
        }
        
        public Contour<T> Push()
        {
            return new Contour<T>
            {
                _parent = this
            };
        }

        [IndexerName("Item")]
        public T this[string key]
        {
            get
            {
                Lookup(key, out var result);

                return result;
            }
        }
    }
}