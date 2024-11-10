using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(MeshFilter))]
public class ProceduralLightning : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] private Transform _boltOriginTransform = null;
    [SerializeField] private Transform _boltImpactTransform = null;
    [SerializeField] private LightningShapeGenerator _lightningShapeGenerator = null;

    [Header("Lightning Settings: ")]
    [Tooltip("How many vertices are used to construct a segment around one point of the lightning bolt. In case the bolt is " +
        "seen from up close, use a higher segment resolution. Otherwise, use a lower one.")]
    [SerializeField, Range(3, 32)] private int _segmentResolution = 6;
    [Tooltip("Radius of each segment, used when creating vertices around the lightning point. " +
        "Basically represents the width of the lightning mesh.")]
    [SerializeField, Range(0.0001f, 0.1f)] private float _segmentRadius = 0.0003f;

    private MeshFilter _meshFilter = null;
    private LightningMesh _lightningMesh = null;

    private MeshFilter MeshFilter
    {
        get
        {
            if (_meshFilter == null)
            {
                // Lazy component fetching upon first request of the mesh filter.
                _meshFilter = GetComponent<MeshFilter>();
            }
            return _meshFilter;
        }
    }
    private LightningMesh LightningMesh
    {
        get
        {
            if (_lightningMesh == null)
            {
                // Lazy initialization of a new LightningMesh instance.
                _lightningMesh = new LightningMesh();
            }
            return _lightningMesh;
        }
        set
        {
            _lightningMesh = value;
        }
    }

    private void Update()
    {
        if (!AreAllReferencesConnected())
        {
            return;
        }

        // Construct the shape of the lightning that spans between the provided positions.
        List<LightningBranch> lightningBranches = _lightningShapeGenerator.CreateLightningShape(_boltOriginTransform.position, _boltImpactTransform.position);

        // Update the mesh to reflect the changes in the shape of the lightning. This will possibly require mesh re-construction
        // if the shape changed drastically.
        Mesh lightningMesh = LightningMesh.CreateLightningMesh(lightningBranches, _segmentResolution, _segmentRadius);

        // Updating the mesh reference to the MeshFilter in case a new mesh has been constructed.
        if (MeshFilter.sharedMesh != lightningMesh)
        {
            MeshFilter.sharedMesh = lightningMesh;
        }
    }

    private void OnDestroy()
    {
        LightningMesh.CleanUpMesh();
        LightningMesh = null;
    }

    private bool AreAllReferencesConnected()
    {
        return _boltOriginTransform != null && _boltImpactTransform != null && _lightningShapeGenerator != null;
    }
}
