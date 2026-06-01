// Cualquier cosa que pueda recibir impactos de bala del jugador (enemigos, el nucleo final...). Permite que
// BulletController reste vida sin necesidad de conocer el tipo concreto de objeto al que ha golpeado
public interface IDamageable
{
    void TakeDamage(int damage);
}
