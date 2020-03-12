using UnityEngine;
using System.Collections;


  public class Hunter : Boid
{
    public School2 School2 { get; set; }

    public Vector2 PositionH;
    public Vector2 VelocityH;
    public Vector2 AccelerationH;



    void Start()
    {
        VelocityH = Random.insideUnitSphere * 2;
    }

    public void UpdateSimulationH(float deltaTime)
    {
        //colisionDetection();
        //Clear acceleration from last frame
        AccelerationH = Vector2.zero;

        //Apply forces
        AccelerationH += (Vector2)School2.GetForceFromBounds(this);
        AccelerationH += GetConstraintSpeedForce();
        AccelerationH += GetSteeringForce();

        //Step simulation
        VelocityH += deltaTime * AccelerationH;
        PositionH +=  0.5f * deltaTime * deltaTime * AccelerationH + deltaTime * VelocityH;

    }



    private void colisionDetection() {
      // Andra parametern i getNeighbors är radien på "grannar" cirkeln. Prova med olika värden
      foreach (Boid neighbor in School2.BoidManager.GetNeighbors(this, 2)) { // Fixa så den tar från boid school
        if(Mathf.Abs((PositionH - neighbor.Position).magnitude) < 2) {
          Destroy(neighbor);
        }
      }
    }

    Vector2 GetSteeringForce()
    {
        Vector2 cohesionForce = Vector2.zero;
        Vector2 alignmentForce = Vector2.zero;
        Vector2 separationForce = Vector2.zero;

        //Boid forces
        foreach (Hunter neighbor in School2.BoidManager.GetNeighborsH(this, School2.NeighborRadius))
        {
            float distance = (neighbor.PositionH - PositionH).magnitude;
        /*      // 2 är placeholder atm
            if( distance < 2 & neighbor.Hunter == false) {

            } */

            //Separation force
            if (distance < School2.SeparationRadius)
            {
              //  separationForce += School2.SeparationForceFactor * ((School2.SeparationRadius - distance) / distance) * (PositionH - neighbor.PositionH);
            }

            //Calculate average position/velocity here
        }

        //Set cohesio n/alignment forces here
        Debug.Log(alignmentForce);
        Debug.Log(cohesionForce);
        Debug.Log(separationForce);
        return alignmentForce + cohesionForce + separationForce;
    }

    Vector2 GetConstraintSpeedForce()
    {
        Vector2 force = Vector2.zero;

        //Apply drag
        force -= School2.Drag * VelocityH;

        float vel = VelocityH.magnitude;
        if (vel > School2.MaxSpeed)
        {
            //If speed is above the maximum allowed speed, apply extra friction force
            force -= (20.0f * (vel - School2.MaxSpeed) / vel) * VelocityH;
        }
        else if (vel < School2.MinSpeed)
        {
            //Increase the speed slightly in the same direction if it is below the minimum
            force += (5.0f * (School2.MinSpeed - vel) / vel) * VelocityH;
        }

        return force;
    }
}
