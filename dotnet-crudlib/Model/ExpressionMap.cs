using System.Linq.Expressions;

namespace NetCrudLib.Model
{

    public class ExpressionMap<T, TK> where T : BaseEntity<TK>
    {
        public Dictionary<string, Expression<Func<T, object>>[]> Items { get; set; } = []; 
        public ExpressionMap<T, TK> Add(string key, params Expression<Func<T, object>>[] value)
        {
            Items.Add(key, value);
            return this;
        }
    }
}