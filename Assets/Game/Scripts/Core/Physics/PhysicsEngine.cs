using UnityEngine;

public static class PhysicsEngine
{
    public static void ResolveCollision(Body2D a, Body2D b)
    {
        Vector2 normal = (b.Position - a.Position).normalized;
        ResolveCollision(a, b, normal);
    }


    private static void ResolveCollision(Body2D a, Body2D b, Vector2 normal)
    {
        // простая упругая реакция (массой можно управлять)
        Vector2 relativeVel = b.Velocity - a.Velocity;
        float velAlongNormal = Vector2.Dot(relativeVel, normal);

        if (velAlongNormal > 0) 
            return; // уже расходятся

        float restitution = 1f; // коэффициент упругости
        float impulseMag = -(1 + restitution) * velAlongNormal / (1 / a.Mass + 1 / b.Mass);

        Vector2 impulse = impulseMag * normal;
        a.Velocity -= impulse / a.Mass;
        b.Velocity += impulse / b.Mass;
    }
}