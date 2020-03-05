using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
    public School School { get; set; }

    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;



    

    void Start()
    {
        Velocity = Random.insideUnitSphere * 2;
    }

    public void UpdateSimulation(float deltaTime)
    {
        //Clear acceleration from last frame
        Acceleration = Vector3.zero;

        //Apply forces
        Acceleration += (Vector2)School.GetForceFromBounds(this);
        Acceleration += GetConstraintSpeedForce();
        Acceleration += GetSteeringForce();

        //Step simulation
        Velocity += deltaTime * Acceleration;
        Position +=  0.5f * deltaTime * deltaTime * Acceleration + deltaTime * Velocity;
    }

    Vector2 GetSteeringForce()
    {
        Vector2 cohesionForce = Vector2.zero;
        Vector2 alignmentForce = Vector2.zero;
        Vector2 separationForce = Vector2.zero;

        //Boid forces
        foreach (Boid neighbor in School.BoidManager.GetNeighbors(this, School.NeighborRadius))
        {
            float distance = (neighbor.Position - Position).magnitude;

            //Separation force
            if (distance < School.SeparationRadius)
            {
                separationForce += School.SeparationForceFactor * ((School.SeparationRadius - distance) / distance) * (Position - neighbor.Position);
            }

            //Calculate average position/velocity here
        }

        //Set cohesio n/alignment forces here

        return alignmentForce + cohesionForce + separationForce;
    }

    Vector2 GetConstraintSpeedForce()
    {
        Vector2 force = Vector3.zero;

        //Apply drag
        force -= School.Drag * Velocity;

        float vel = Velocity.magnitude;
        if (vel > School.MaxSpeed)
        {
            //If speed is above the maximum allowed speed, apply extra friction force
            force -= (20.0f * (vel - School.MaxSpeed) / vel) * Velocity;
        }
        else if (vel < School.MinSpeed)
        {
            //Increase the speed slightly in the same direction if it is below the minimum
            force += (5.0f * (School.MinSpeed - vel) / vel) * Velocity;
        }

        return force;
    }
}
