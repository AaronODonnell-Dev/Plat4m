using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFootprints : MonoBehaviour
{
    public Camera _camera;
    public Shader _drawShader;
    public GameObject _plane;
    private Material _planeMaterial, _drawMaterial;
    private RenderTexture _splatMap;
    public Transform[] feet;
    private RaycastHit _hit;
    int _layerMask;
    [Range(1, 5)]
    public float brushSize;
    [Range(0, 1)]
    public float brushStrength;

    public float raydist;
    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        _drawMaterial = new Material(_drawShader);
        _planeMaterial = _plane.GetComponent<MeshRenderer>().material;
        _planeMaterial.SetTexture("_Splat", _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
       
    }

    // Update is called once per frame
    void Update()
    {
        raydist = _hit.distance;
       
        for (int i = 0; i < feet.Length; i++)
        {
            if (Physics.Raycast(feet[i].position, Vector3.down, out _hit, 1f, _layerMask))
            {
                _drawMaterial.SetVector("_Coords", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_BrushSize", brushSize);
                _drawMaterial.SetFloat("_BrushStrength", brushStrength);
                RenderTexture Temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, Temp);
                Graphics.Blit(Temp, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(Temp);
            }
        }
    }
}
