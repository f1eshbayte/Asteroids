using UnityEngine;

public class Body2D
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Mass;
   
    public Body2D(Vector2 position, float mass)
    {
        Position = position;
        Mass = mass;
        Velocity = Vector2.zero;
    }

    public void Integrate(float deltaTime)
    {
        Position += Velocity * deltaTime;
    }
}
