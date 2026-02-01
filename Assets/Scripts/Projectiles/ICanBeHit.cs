namespace Projectiles
{
    public interface ICanBeHit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns><c>true</c> if bullet should <b>NOT</b> be destroyed.<br/><c>false</c>if the bullet should be destroyed</returns>
        bool Hit(Projectile projectile);
    }
}