using UnityEngine;

public class Asteroid : Body2D
{
    public float Speed;

    public Asteroid(Vector2 position, float mass = 1f, float speed = 5f)
        : base(position, mass)
    {
        Speed = speed;
        SetRandomVelocity();
    }

    public void Reset(Vector2 position, float speed)
    {
        Position = position;
        Speed = speed;
        SetRandomVelocity();
    }
    

    private void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Speed;
    }

}