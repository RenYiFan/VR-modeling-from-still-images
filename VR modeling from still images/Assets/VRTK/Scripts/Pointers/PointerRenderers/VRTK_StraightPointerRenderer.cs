// Straight Pointer Renderer|PointerRenderers|10020
namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The Straight Pointer Renderer emits a coloured beam from the end of the object it is attached to and simulates a laser beam.
    /// </summary>
    /// <remarks>
    /// It can be useful for pointing to objects within a scene and it can also determine the object it is pointing at and the distance the object is from the controller the beam is being emitted from.
    /// </remarks>
    /// <example>
    /// `VRTK/Examples/003_Controller_SimplePointer` shows the simple pointer in action and code examples of how the events are utilised and listened to can be viewed in the script `VRTK/Examples/Resources/Scripts/VRTK_ControllerPointerEvents_ListenerExample.cs`
    /// </example>
    [AddComponentMenu("VRTK/Scripts/Pointers/Pointer Renderers/VRTK_StraightPointerRenderer")]
    public class VRTK_StraightPointerRenderer : VRTK_BasePointerRenderer
    {
        [Header("Straight Pointer Appearance Settings")]

        [Tooltip("The maximum length the pointer tracer can reach.")]
        public float maximumLength = 100f;
        [Tooltip("The scale factor to scale the pointer tracer object by.")]
        public float scaleFactor = 0.002f;
        [Tooltip("The scale multiplier to scale the pointer cursor object by in relation to the `Scale Factor`.")]
        public float cursorScaleMultiplier = 25f;
        [Tooltip("The cursor will be rotated to match the angle of the target surface if this is true, if it is false then the pointer cursor will always be horizontal.")]
        public bool cursorMatchTargetRotation = false;
        [Tooltip("Rescale the cursor proportionally to the distance from the tracer origin.")]
        public bool cursorDistanceRescale = false;
        [Tooltip("The maximum scale the cursor is allowed to reach. This is only used when rescaling the cursor proportionally to the distance from the tracer origin.")]
        public Vector3 maximumCursorScale = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        [Header("Straight Pointer Custom Appearance Settings")]

        [Tooltip("A custom game object to use as the appearance for the pointer tracer. If this is empty then a Box primitive will be created and used.")]
        public GameObject customTracer;
        [Tooltip("A custom game object to use as the appearance for the pointer cursor. If this is empty then a Sphere primitive will be created and used.")]
        public GameObject customCursor;

        protected GameObject actualContainer;
        protected GameObject actualTracer;
        protected GameObject actualCursor;

    /*    private LineRenderer areaLine;
        private int LengthOfLineRenderer = 0;
        private Vector3 position;
        private Vector3[] dis = new Vector3[1000];
        private double[] side = new double[1000];
        public Camera cameraN1;
        private double m = 1;
        public Text distenceN1;
        public Text areaN1;
        public Text volumeText;
        public Text surfaceText;
        private double areaNumber;
        private double distance = 0;
        public Button distenceButton;
        public Button areaButton;
        private bool distenceButtonClick = false;
        private bool areaButtonClick = false;
        private bool accurateButtonClick = false;
        private bool test1 = false;
        private bool test2 = false;
*/
        protected Vector3 cursorOriginalScale = Vector3.one;

        /// <summary>
        /// The UpdateRenderer method is used to run an Update routine on the pointer.
        /// </summary>
        public override void UpdateRenderer()
        {
            if ((controllingPointer && controllingPointer.IsPointerActive()) || IsVisible())
            {
                float tracerLength = CastRayForward();
                SetPointerAppearance(tracerLength);
                MakeRenderersVisible();
            }
            base.UpdateRenderer();
        }

        /// <summary>
        /// The GetPointerObjects returns an array of the auto generated GameObjects associated with the pointer.
        /// </summary>
        /// <returns>An array of pointer auto generated GameObjects.</returns>
        public override GameObject[] GetPointerObjects()
        {
            return new GameObject[] { actualContainer, actualCursor, actualTracer };
        }

        protected override void ToggleRenderer(bool pointerState, bool actualState)
        {
            ToggleElement(actualTracer, pointerState, actualState, tracerVisibility, ref tracerVisible);
            ToggleElement(actualCursor, pointerState, actualState, cursorVisibility, ref cursorVisible);
        }

        protected override void CreatePointerObjects()
        {
            actualContainer = new GameObject(VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Container"));
            actualContainer.transform.localPosition = Vector3.zero;
            VRTK_PlayerObject.SetPlayerObject(actualContainer, VRTK_PlayerObject.ObjectTypes.Pointer);

            CreateTracer();
            CreateCursor();
            Toggle(false, false);
            if (controllingPointer)
            {
                controllingPointer.ResetActivationTimer(true);
                controllingPointer.ResetSelectionTimer(true);
            }
        }

        protected override void DestroyPointerObjects()
        {
            if (actualContainer != null)
            {
                Destroy(actualContainer);
            }
        }

        protected override void ChangeMaterial(Color givenColor)
        {
            base.ChangeMaterial(givenColor);
            ChangeMaterialColor(actualTracer, givenColor);
            ChangeMaterialColor(actualCursor, givenColor);
        }

        protected override void UpdateObjectInteractor()
        {
            base.UpdateObjectInteractor();
            //if the object interactor is too far from the pointer tip then set it to the pointer tip position to prevent glitching.
            if (objectInteractor && actualCursor && Vector3.Distance(objectInteractor.transform.position, actualCursor.transform.position) > 0f)
            {
                objectInteractor.transform.position = actualCursor.transform.position;
            }
        }

        protected virtual void CreateTracer()
        {
            if (customTracer)
            {
                actualTracer = Instantiate(customTracer);
            }
            else
            {
                actualTracer = GameObject.CreatePrimitive(PrimitiveType.Cube);
                actualTracer.GetComponent<BoxCollider>().isTrigger = true;
                actualTracer.AddComponent<Rigidbody>().isKinematic = true;
                actualTracer.layer = LayerMask.NameToLayer("Ignore Raycast");

                SetupMaterialRenderer(actualTracer);
            }

            actualTracer.transform.name = VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Tracer");
            actualTracer.transform.SetParent(actualContainer.transform);

            VRTK_PlayerObject.SetPlayerObject(actualTracer, VRTK_PlayerObject.ObjectTypes.Pointer);
        }

        protected virtual void CreateCursor()
        {
            if (customCursor)
            {
                actualCursor = Instantiate(customCursor);
            }
            else
            {
                actualCursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                actualCursor.transform.localScale = Vector3.one * (scaleFactor * cursorScaleMultiplier);
                actualCursor.GetComponent<Collider>().isTrigger = true;
                actualCursor.AddComponent<Rigidbody>().isKinematic = true;
                actualCursor.layer = LayerMask.NameToLayer("Ignore Raycast");

                SetupMaterialRenderer(actualCursor);
            }

            cursorOriginalScale = actualCursor.transform.localScale;
            actualCursor.transform.name = VRTK_SharedMethods.GenerateVRTKObjectName(true, gameObject.name, "StraightPointerRenderer_Cursor");
            actualCursor.transform.SetParent(actualContainer.transform);
            VRTK_PlayerObject.SetPlayerObject(actualCursor, VRTK_PlayerObject.ObjectTypes.Pointer);
        }

        protected virtual void CheckRayMiss(bool rayHit, RaycastHit pointerCollidedWith)
        {
            if (!rayHit || (destinationHit.collider && destinationHit.collider != pointerCollidedWith.collider))
            {
                if (destinationHit.collider != null)
                {
                    PointerExit(destinationHit);
                }

                destinationHit = new RaycastHit();
                ChangeColor(invalidCollisionColor);
            }
        }

        protected virtual void CheckRayHit(bool rayHit, RaycastHit pointerCollidedWith)
        {
            if (rayHit)
            {
                PointerEnter(pointerCollidedWith);

                destinationHit = pointerCollidedWith;
                ChangeColor(validCollisionColor);
            }
        }

        protected virtual float CastRayForward()
        {
            Transform origin = GetOrigin();
            Ray pointerRaycast = new Ray(origin.position, origin.forward);
            RaycastHit pointerCollidedWith;
          //Debug.Log(origin.position);
         // Debug.Log(origin.forward);
#pragma warning disable 0618
            bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, pointerRaycast, out pointerCollidedWith, layersToIgnore, maximumLength);
#pragma warning restore 0618

            CheckRayMiss(rayHit, pointerCollidedWith);
            CheckRayHit(rayHit, pointerCollidedWith);

            float actualLength = maximumLength;
            if (rayHit && pointerCollidedWith.distance < maximumLength)
            {
                actualLength = pointerCollidedWith.distance;
            }
      /*      GameObject volumesum = GameObject.Find("model/default");
            MeshCalculator vol = volumesum.GetComponent<MeshCalculator>();
            float volume = vol.sum;
            float surface = vol.sum1;
            areaLine = GetComponent<LineRenderer>();
            areaLine.SetColors(Color.yellow, Color.yellow);
            areaLine.SetWidth(0.01f, 0.01f);
            Button disButton = distenceButton.GetComponent<Button>();
            disButton.onClick.AddListener(distenceButtonClicked);
            Button areaButtonFinish = areaButton.GetComponent<Button>();
            areaButtonFinish.onClick.AddListener(areaButtonClicked);
            if (Input.GetKeyDown(KeyCode.B))
            {
                 test1 = true;
                 test2 = false;
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                 test2 = true;
                 test1 = false;
            }
            if (distenceButtonClick == true||test1==true)
            {

                if (Input.GetMouseButtonDown(0))
                {

                    if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
                    {
                        position = pointerCollidedWith.point;
                        LengthOfLineRenderer++;
               //       if (LengthOfLineRenderer <= 6)
                //      {
                            areaLine.SetVertexCount(LengthOfLineRenderer);
                            areaLine.SetPosition(LengthOfLineRenderer - 1, position);
                            // areaLine.positionCount = dis.Length;
                            dis[LengthOfLineRenderer - 1] = position;
                         // dis[LengthOfLineRenderer - 1] = position;
                            if (LengthOfLineRenderer >= 2)
                                distance = distance + Vector3.Distance(dis[LengthOfLineRenderer - 2], dis[LengthOfLineRenderer - 1]);
                            else
                                distance = 0;
                //      }
                  }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        LengthOfLineRenderer = 0;
                        distance = 0;
                        areaNumber = 0;
                        volume = 0;
                        surface = 0;
                    }
                    distenceN1.text = "distence:" + distance * m;
                }

            }
            if (areaButtonClick == true||test2==true)
            {

                if (Input.GetMouseButtonDown(0))
                {

                    if (Physics.Raycast(pointerRaycast, out pointerCollidedWith))
                    {
                        position = pointerCollidedWith.point;
                        LengthOfLineRenderer++;
                        if (LengthOfLineRenderer <= 6)
                        {
                            areaLine.SetVertexCount(LengthOfLineRenderer);
                            areaLine.SetPosition(LengthOfLineRenderer - 1, position);

                            dis[LengthOfLineRenderer - 1] = position;
                            if (LengthOfLineRenderer == 3)
                            {
                                areaLine.loop = true;
                                //Debug.Log(dis[0]);
                                // side[LengthOfLineRenderer - 1] = System.Math.Sqrt(System.Math.Pow(dis[LengthOfLineRenderer].x - dis[LengthOfLineRenderer - 1].x, 2)
                                //     + System.Math.Pow(dis[LengthOfLineRenderer].y - dis[LengthOfLineRenderer - 1].y, 2)
                                //     + System.Math.Pow(dis[LengthOfLineRenderer].z - dis[LengthOfLineRenderer - 1].z, 2));
                                side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                     + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                     + System.Math.Pow(dis[1].z - dis[0].z, 2));
                                side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                      + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                        + System.Math.Pow(dis[2].z - dis[1].z, 2));
                                side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                      + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                      + System.Math.Pow(dis[0].z - dis[2].z, 2));

                                if (side[0] + side[1] <= side[2] || side[0] + side[2] <= side[1] || side[1] + side[2] <= side[0])
                                {
                                    areaNumber = 0;
                                }
                                else
                                {
                                    double p = (side[0] + side[1] + side[2]) / 2;
                                    //Debug.Log(p);
                                    areaNumber = System.Math.Sqrt(p * (p - side[0]) * (p - side[1]) * (p - side[2]));
                                }
                                //Debug.Log(dis[LengthOfLineRenderer].x);
                                //Debug.Log(side[0]);
                                // Debug.Log(areaNumber);
                            }
                            if (LengthOfLineRenderer == 4)
                            {
                                areaLine.loop = true;
                                side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                     + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                     + System.Math.Pow(dis[1].z - dis[0].z, 2));
                                side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                      + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                        + System.Math.Pow(dis[2].z - dis[1].z, 2));
                                side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                      + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                      + System.Math.Pow(dis[0].z - dis[2].z, 2));
                                side[3] = System.Math.Sqrt(System.Math.Pow(dis[3].x - dis[2].x, 2)
                                    + System.Math.Pow(dis[3].y - dis[2].y, 2)
                                    + System.Math.Pow(dis[3].z - dis[2].z, 2));
                                side[4] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[3].x, 2)
                                    + System.Math.Pow(dis[0].y - dis[3].y, 2)
                                    + System.Math.Pow(dis[0].z - dis[3].z, 2));
                                double p1 = (side[0] + side[1] + side[2]) / 2;
                                double p2 = (side[2] + side[3] + side[4]) / 2;
                                areaNumber = System.Math.Sqrt(p1 * (p1 - side[0]) * (p1 - side[1]) * (p1 - side[2]))
                                    + System.Math.Sqrt(p2 * (p2 - side[2]) * (p2 - side[3]) * (p2 - side[4]));
                            }
                            if (LengthOfLineRenderer == 5)
                            {
                                areaLine.loop = true;
                                side[0] = System.Math.Sqrt(System.Math.Pow(dis[1].x - dis[0].x, 2)
                                     + System.Math.Pow(dis[1].y - dis[0].y, 2)
                                     + System.Math.Pow(dis[1].z - dis[0].z, 2));
                                side[1] = System.Math.Sqrt(System.Math.Pow(dis[2].x - dis[1].x, 2)
                                      + System.Math.Pow(dis[2].y - dis[1].y, 2)
                                        + System.Math.Pow(dis[2].z - dis[1].z, 2));
                                side[2] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[2].x, 2)
                                      + System.Math.Pow(dis[0].y - dis[2].y, 2)
                                      + System.Math.Pow(dis[0].z - dis[2].z, 2));
                                side[3] = System.Math.Sqrt(System.Math.Pow(dis[3].x - dis[2].x, 2)
                                    + System.Math.Pow(dis[3].y - dis[2].y, 2)
                                    + System.Math.Pow(dis[3].z - dis[2].z, 2));
                                side[4] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[3].x, 2)
                                    + System.Math.Pow(dis[0].y - dis[3].y, 2)
                                    + System.Math.Pow(dis[0].z - dis[3].z, 2));
                                side[5] = System.Math.Sqrt(System.Math.Pow(dis[4].x - dis[3].x, 2)
                                    + System.Math.Pow(dis[4].y - dis[3].y, 2)
                                    + System.Math.Pow(dis[4].z - dis[3].z, 2));
                                side[6] = System.Math.Sqrt(System.Math.Pow(dis[0].x - dis[4].x, 2)
                                    + System.Math.Pow(dis[0].y - dis[4].y, 2)
                                    + System.Math.Pow(dis[0].z - dis[4].z, 2));

                                double p1 = (side[0] + side[1] + side[2]) / 2;
                                double p2 = (side[2] + side[3] + side[4]) / 2;
                                double p3 = (side[4] + side[5] + side[6]) / 2;
                                areaNumber = System.Math.Sqrt(p1 * (p1 - side[0]) * (p1 - side[1]) * (p1 - side[2]))
                                    + System.Math.Sqrt(p2 * (p2 - side[2]) * (p2 - side[3]) * (p2 - side[4]))
                                    + System.Math.Sqrt(p3 * (p3 - side[4]) * (p3 - side[5]) * (p3 - side[6]));
                            }

                        }
                    }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LengthOfLineRenderer = 0;
                    areaNumber = 0;
                        volume = 0;
                        surface = 0;
                    }

                areaN1.text = "area:" + areaNumber * m;
                }

            }

            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(pointerRaycast, out pointerCollidedWith) && pointerCollidedWith.transform.tag == "model")
                {
                    if (pointerCollidedWith.transform.tag == "model")
                    {
                        volumeText.text = "Volume= " + Mathf.Abs(volume);
                        surfaceText.text = "Surface area = " + surface;
                        //Debug.Log("Volume= " + Mathf.Abs(volume));
                        // Debug.Log("Area = " + surface);
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    LengthOfLineRenderer = 0;
                    areaNumber = 0;
                    volume = 0;
                    surface = 0;
                }

            }

            // GameObject.Find("model/default").SendMessage("MeshCalculator", "sum");
*/
            return OverrideBeamLength(actualLength);
        }


/*
        void distenceButtonClicked()
        {
            accurateButtonClick = false;
            areaButtonClick = false;
            distenceButtonClick = true;
            LengthOfLineRenderer = 0;
            distance = 0;
            // lineRenderer.enabled = true;
        }
        void areaButtonClicked()
        {
            accurateButtonClick = false;
            areaButtonClick = true;
            distenceButtonClick = false;
            LengthOfLineRenderer = 0;
            areaNumber = 0;
            // Destroy(lineRenderer.gameObject, 1);
        }

*/
        protected virtual void SetPointerAppearance(float tracerLength)
        {
            if (actualContainer)
            {
                //if the additional decimal isn't added then the beam position glitches
                float beamPosition = tracerLength / (2f + BEAM_ADJUST_OFFSET);

                actualTracer.transform.localScale = new Vector3(scaleFactor, scaleFactor, tracerLength);
                actualTracer.transform.localPosition = Vector3.forward * beamPosition;
                actualCursor.transform.localScale = Vector3.one * (scaleFactor * cursorScaleMultiplier);
                actualCursor.transform.localPosition = new Vector3(0f, 0f, tracerLength);

                Transform origin = GetOrigin();
                actualContainer.transform.position = origin.position;
                actualContainer.transform.rotation = origin.rotation;

                float objectInteractorScaleIncrease = 1.05f;
                ScaleObjectInteractor(actualCursor.transform.lossyScale * objectInteractorScaleIncrease);

                if (destinationHit.transform)
                {
                    if (cursorMatchTargetRotation)
                    {
                        actualCursor.transform.forward = -destinationHit.normal;
                    }
                    if (cursorDistanceRescale)
                    {
                        float collisionDistance = Vector3.Distance(destinationHit.point, origin.position);
                        actualCursor.transform.localScale = Vector3.Min(cursorOriginalScale * collisionDistance, maximumCursorScale);
                    }
                }
                else
                {
                    if (cursorMatchTargetRotation)
                    {
                        actualCursor.transform.forward = origin.forward;
                    }
                    if (cursorDistanceRescale)
                    {
                        actualCursor.transform.localScale = Vector3.Min(cursorOriginalScale * tracerLength, maximumCursorScale);
                    }
                }

                ToggleRenderer(controllingPointer.IsPointerActive(), false);
                UpdateDependencies(actualCursor.transform.position);
            }
        }
    }
}