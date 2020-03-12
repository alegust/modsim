using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class School2 : MonoBehaviour
{
    [SerializeField]
    private int m_numFish = 50;

    [SerializeField]
    private Boid m_fishPrefab = null;

    [SerializeField]
    private float m_spawnRadius = 10;

    [SerializeField]
    private BoxCollider m_bounds = null;

    [SerializeField]
    private float m_boundsForceFactor = 5;

    [Header("Boid behaviour data. Experiment with changing these during runtime")]
    [SerializeField]
    private float m_cohesionForceFactor = 1;
    public float CohesionForceFactor
    {
        get { return m_cohesionForceFactor; }
        set { m_cohesionForceFactor = value; }
    }

    [SerializeField]
    private float m_cohesionRadius = 3;
    public float CohesionRadius
    {
        get { return m_cohesionRadius; }
        set { m_cohesionRadius = value; }
    }

    [SerializeField]
    private float m_separationForceFactor = 1;
    public float SeparationForceFactor
    {
        get { return m_separationForceFactor; }
        set { m_separationForceFactor = value; }
    }

    [SerializeField]
    private float m_separationRadius = 2;
    public float SeparationRadius
    {
        get { return m_separationRadius; }
        set { m_separationRadius = value; }
    }

    [SerializeField]
    private float m_alignmentForceFactor = 1;
    public float AlignmentForceFactor
    {
        get { return m_alignmentForceFactor; }
        set { m_alignmentForceFactor = value; }
    }

    [SerializeField]
    private float m_alignmentRadius = 3;
    public float AlignmentRadius
    {
        get { return m_alignmentRadius; }
        set { m_alignmentRadius = value; }
    }

    [SerializeField]
    private float m_maxSpeed = 8;
    public float MaxSpeed
    {
        get { return m_maxSpeed; }
        set { m_maxSpeed = value; }
    }

    [SerializeField]
    private float m_minSpeed;
    public float MinSpeed
    {
        get { return m_minSpeed; }
        set { m_minSpeed = value; }
    }

    [SerializeField]
    private float m_drag = 0.1f;
    public float Drag
    {
        get { return m_drag; }
        set { m_drag = value; }
    }



    public float NeighborRadius
    {
        get { return Mathf.Max(m_alignmentRadius, Mathf.Max(m_separationRadius, m_cohesionRadius)); }
    }

    public BoidManager BoidManager { get; set; }

    public IEnumerable<Hunter> SpawnFish()
    {
        for (int i = 0; i < m_numFish; ++i)
        {
            Vector3 spawnPoint = transform.position + m_spawnRadius * Random.insideUnitSphere;

            for (int j = 0; j < 3; ++j)
                spawnPoint[j] = Mathf.Clamp(spawnPoint[j], m_bounds.bounds.min[j], m_bounds.bounds.max[j]);

            Hunter hunter = Instantiate(m_fishPrefab, spawnPoint, m_fishPrefab.transform.rotation) as Hunter;
            hunter.PositionH = spawnPoint;
            hunter.VelocityH = Random.insideUnitSphere;
            hunter.School2 = this;
            hunter.transform.parent = this.transform;
            yield return hunter;
        }
    }

    public Vector3 GetForceFromBounds(Hunter hunter)
    {
        Vector3 force = new Vector3();
        Vector3 centerToPos = (Vector3)hunter.PositionH - transform.position;
        Vector3 minDiff = centerToPos + m_bounds.size * 0.5f;
        Vector3 maxDiff = centerToPos - m_bounds.size * 0.5f;
        float friction = 0.0f;

        for (int i = 0; i < 3; ++i)
        {
            if (minDiff[i] < 0)
                force[i] = minDiff[i];
            else if (maxDiff[i] > 0)
                force[i] = maxDiff[i];
            else
                force[i] = 0;

            friction += Mathf.Abs(force[i]);
        }

        force += 0.1f * friction * (Vector3)hunter.VelocityH;
        return -m_boundsForceFactor * force;
    }
}
