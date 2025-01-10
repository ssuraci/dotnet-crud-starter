namespace NetCrudStarter.Model
{
    public abstract class BaseEntity<T>
    {
        public T? Id { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (BaseEntity<T>)obj;

            // If both entities are not yet persisted, they can't be equal
            if (EqualityComparer<T>.Default.Equals(Id, default(T)) || EqualityComparer<T>.Default.Equals(other.Id, default(T)))
                return false;

            return EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            // Use a prime number to reduce collision likelihood
            return Id != null ? Id.GetHashCode() : 0;
        }

    }


    // verificare se fare override del metodo equals e hash
}
