using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialOptions
    {
        public List<Material> Materials;
    }

    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private List<MaterialOptions> _materials;

    void Awake()
    {
        // Not validating size of collections but hey, JAM TIME!
        var materials = new List<Material>(_materials.Count);
        var index = Random.Range(0, _materials[0].Materials.Count);
        for (int i = 0; i < _materials.Count; i++)
        {
            var list = _materials[i].Materials;
            materials.Add(list[index]);
        }

        _renderer.SetSharedMaterials(materials);
    }
}
