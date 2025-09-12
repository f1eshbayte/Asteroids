using UnityEngine;

public class AsteroidModel : Body2D
{
    public float Speed;

    public AsteroidModel(Vector2 position, float radius = 0.5f, float mass = 1f, float speed = 10f)
        : base(position, radius, mass)
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