using UnityEngine;
using System.Collections;

public interface cellPart
{
    int checkType(string type);

}

public interface IDamageable<T>
{
    void Damage(T damageTaken);
}