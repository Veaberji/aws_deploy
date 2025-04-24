using System.Collections.Generic;
using System.Linq;

namespace MusiciansAPP.Domain;

public abstract class Entity
{
    public static bool IsFullData(IEnumerable<Entity> entities, int pageSize)
    {
        return entities.Count() == pageSize && entities.All(e => e.IsFull());
    }

    protected abstract bool IsFull();
}