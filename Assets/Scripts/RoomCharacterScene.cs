using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class RoomCharacterScene : MonoBehaviour
{
    private CharacterVisualObjectBehaviour GetVisualObjectBehaviourByName(string name)
    {
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            CharacterVisualObjectBehaviour characterVisualObjectBehaviour = this.visualObjectBehaviours[i];
            if (characterVisualObjectBehaviour.name == name)
            {
                return characterVisualObjectBehaviour;
            }
        }
        return null;
    }

    private void DestroyAll()
    {
        Transform transform = this.offset;
        if (transform == null)
        {
            return;
        }
        this.transformsToRemove.Clear();
        foreach (object obj in transform)
        {
            Transform item = (Transform)obj;
            this.transformsToRemove.Add(item);
        }
        for (int i = 0; i < this.transformsToRemove.Count; i++)
        {
            Transform transform2 = this.transformsToRemove[i];
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(transform2.gameObject);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(transform2.gameObject);
            }
        }
        this.visualObjectBehaviours.Clear();
    }

    private CharacterVisualObjectBehaviour CreateVisualObjectBehaviour(VisualObjectBehaviour vo)
    {
        CharacterVisualObjectBehaviour characterVisualObjectBehaviour = new GameObject
        {
            transform =
            {
                parent = this.offset,
                localPosition = Vector3.zero
            },
            name = vo.name,

            layer = this.offset.gameObject.layer
        }.AddComponent<CharacterVisualObjectBehaviour>();
        characterVisualObjectBehaviour.Init(vo, this);
        return characterVisualObjectBehaviour;
    }

    public void Init(DecoratingScene3DSetup scene3dSetup, DecoratingScene decoratingScene, RenderTexture renderTexture)
    {
        this.InitGameObjects();
        this.scene = decoratingScene;
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(scene3dSetup.FindMainCamera().gameObject, this.spaceRoot);
        this.worldCamera = gameObject.GetComponent<Camera>();
        this.worldCamera.targetTexture = renderTexture;
        this.worldCamera.cullingMask = this.layer.value;
        this.Init();
        List<DecoratingScene3DSetup.VisualObject> visualObjectList = scene3dSetup.visualObjectList;
        int num = 1;
        for (int i = 0; i < this.visualObjectBehaviours.Count; i++)
        {
            CharacterVisualObjectBehaviour characterVisualObjectBehaviour = this.visualObjectBehaviours[i];
            DecoratingScene3DSetup.VisualObject forName = scene3dSetup.GetForName(characterVisualObjectBehaviour.visualObjectBeh.visualObject.name);
            if (forName != null)
            {
                Transform transform = forName.rootTransform;
                if (!(transform == null) && !(forName.collisionRoot == null))
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject, this.spaceRoot);
                    Transform transform2 = gameObject2.transform;
                    GGUtil.CopyWorldTransform(transform, transform2);
                    CharacterVisualObjectSceneItem characterVisualObjectSceneItem = gameObject2.AddComponent<CharacterVisualObjectSceneItem>();
                    characterVisualObjectSceneItem.Init(forName, this.geoMaterial, this.geoSortingLayer);
                    characterVisualObjectSceneItem.stencilIndex = num;
                    num++;
                    characterVisualObjectBehaviour.sceneItem = characterVisualObjectSceneItem;
                    characterVisualObjectBehaviour.Hide();
                }
            }
        }
        GGUtil.SetLayerRecursively(base.gameObject, this.layer);
    }

    private void InitGameObjects()
    {
        if (this.rootTransform == null)
        {
            GameObject gameObject = new GameObject("Root");
            this.rootTransform = gameObject.transform;
            this.rootTransform.parent = base.transform;
            this.rootTransform.localPosition = Vector3.zero;
            this.rootTransform.localScale = Vector3.one;
            this.rootTransform.localRotation = Quaternion.identity;
        }
        if (this.offset == null)
        {
            GameObject gameObject2 = new GameObject("Offset");
            this.offset = gameObject2.transform;
            this.offset.parent = this.rootTransform;
            this.offset.localPosition = Vector3.zero;
            this.offset.localScale = Vector3.one;
            this.offset.localRotation = Quaternion.identity;
        }
        if (this.spaceRoot == null)
        {
            GameObject gameObject3 = new GameObject("SpaceRoot");
            this.spaceRoot = gameObject3.transform;
            this.spaceRoot.parent = base.transform;
            this.spaceRoot.localPosition = Vector3.zero;
            this.spaceRoot.localScale = Vector3.one;
            this.spaceRoot.localRotation = Quaternion.identity;
        }
    }

    public Vector3 WorldToScreenPoint(Vector3 worldPoint)
    {
        return this.worldCamera.WorldToScreenPoint(worldPoint);
    }

    public void Init()
    {
        this.InitGameObjects();
        this.rootTransform.position = Vector3.zero;
        this.rootTransform.rotation = Quaternion.identity;
        this.rootTransform.localScale = Vector3.one;
        this.offset.localPosition = Vector3.left * (float)this.scene.config.width * 0.5f + Vector3.up * (float)this.scene.config.height * 0.5f;
        this.DestroyAll();
        List<VisualObjectBehaviour> list = this.scene.visualObjectBehaviours;
        for (int i = 0; i < list.Count; i++)
        {
            VisualObjectBehaviour visualObjectBehaviour = list[i];
            CharacterVisualObjectBehaviour characterVisualObjectBehaviour = this.CreateVisualObjectBehaviour(visualObjectBehaviour);
            visualObjectBehaviour.characterBehaviour = characterVisualObjectBehaviour;
            this.visualObjectBehaviours.Add(characterVisualObjectBehaviour);
        }
        Transform transform = this.worldCamera.transform;
        this.rootTransform.rotation = transform.rotation;
        Vector3[] array = new Vector3[4];
        this.worldCamera.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), this.distanceFromCamera, Camera.MonoOrStereoscopicEye.Mono, array);
        Vector3 a = this.worldCamera.transform.TransformVector(array[0]);
        Vector3 b = this.worldCamera.transform.TransformVector(array[1]);
        Vector3 b2 = this.worldCamera.transform.TransformVector(array[2]);
        Vector3 b3 = this.worldCamera.transform.TransformVector(array[3]);
        Vector3 position = (a + b + b2 + b3) * 0.25f;
        float num = this.worldCamera.sensorSize.x / this.worldCamera.sensorSize.y;
        int height = this.worldCamera.targetTexture.height;
        float num2 = (float)height * num;
        int width = this.scene.config.width;
        int height2 = this.scene.config.height;
        float aspect = this.worldCamera.aspect;
        float num3 = 2f * this.distanceFromCamera * Mathf.Tan(this.worldCamera.fieldOfView * 0.5f * 0.0174532924f);
        float num4 = num3 * num;
        Vector3 _vector = transform.InverseTransformPoint(position);
        _vector = new Vector3(_vector.x, _vector.y, this.distanceFromCamera);

        float num5 = num3;
        float num6 = num4;
        float num7 = Mathf.Max(num5 / (float)this.scene.config.height, num6 / (float)this.scene.config.width);
        this.rootTransform.position = transform.position + transform.forward * this.distanceFromCamera + transform.right * num6 * this.worldCamera.lensShift.x + transform.up * num5 * this.worldCamera.lensShift.y;
        UnityEngine.Debug.Log(string.Concat(new object[]
        {
            "IMAGE WIDTH ",
            num2,
            " resWidth ",
            width,
            " imageHeight ",
            height,
            " resolutionHeight ",
            height2
        }));
        float num8 = num7 * (float)width / num2;
        float y = num8;
        this.rootTransform.localScale = new Vector3(num8, y, 1f);
    }

    [SerializeField]
    private DecoratingScene scene;

    [SerializeField]
    private Transform rootTransform;

    [SerializeField]
    private Transform offset;

    [SerializeField]
    private Transform spaceRoot;

    [SerializeField]
    public Camera worldCamera;

    [SerializeField]
    private float distanceFromCamera = 20f;

    [SerializeField]
    public Material geoMaterial;

    [SerializeField]
    private SpriteSortingSettings geoSortingLayer = new SpriteSortingSettings();

    [SerializeField]
    public Material spriteMaterial;

    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private List<CharacterVisualObjectBehaviour> visualObjectBehaviours = new List<CharacterVisualObjectBehaviour>();

    private List<Transform> transformsToRemove = new List<Transform>();
}
