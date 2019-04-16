using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawToShader : MonoBehaviour
{
    public Camera _camera;
    public Shader _drawShader;
   
    private RenderTexture _splatMap;
    private Material _planeMaterial, _drawMaterial;

    private RaycastHit _hit;
    // Start is called before the first frame update
    void Start()
    {

        _drawMaterial = new Material(_drawShader);
        _drawMaterial.SetVector("_Color", Color.red);       

        _planeMaterial = GetComponent<MeshRenderer>().material;

        _splatMap = new RenderTexture(1024,1024,0, RenderTextureFormat.ARGBFloat);
        _planeMaterial.SetTexture("_Splat", _splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _drawMaterial.SetVector("_PlayerCoords", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                RenderTexture Temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, Temp);
                Graphics.Blit(Temp, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(Temp);
            }
            
        }
    }
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatMap, ScaleMode.ScaleToFit, false, 1);
    }
}
