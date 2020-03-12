using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidManager : MonoBehaviour
{
    private List<Boid> m_boids;
    private List<Hunter> m_hunters;

    void Start()
    {
        m_boids = new List<Boid>();
        m_hunters = new List<Hunter>();

        var schools = GameObject.FindObjectsOfType<School>();
        foreach (var school in schools)
        {
            school.BoidManager = this;
            m_boids.AddRange(school.SpawnFish());
        }

        var schools2 = GameObject.FindObjectsOfType<School2>();
        foreach (var school in schools2)
        {
            school.BoidManager = this;
            m_hunters.AddRange(school.SpawnFish());
        }
    }

    void FixedUpdate()
    {
      foreach (Boid boid in m_boids)
      {
          boid.UpdateSimulation(Time.fixedDeltaTime);
      }

        foreach (Hunter hunter in m_hunters)
        {
            hunter.UpdateSimulationH(Time.fixedDeltaTime);
        }

    }

    public IEnumerable<Boid> GetNeighbors(Boid boid, float radius)
    {
        float radiusSq = radius * radius;
        foreach (var other in m_boids)
        {
            if (other != boid && (other.Position - boid.Position).sqrMagnitude < radiusSq)
                yield return other;
        }
    }


    public IEnumerable<Hunter> GetNeighborsH(Hunter hunter, float radius)
    {
        float radiusSq = radius * radius;
        foreach (var other in m_hunters)
        {
            if (other != hunter && (other.PositionH - hunter.PositionH).sqrMagnitude < radiusSq)
                yield return other;
        }
    }

}
