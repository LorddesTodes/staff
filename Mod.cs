using MelonLoader;
using Il2Cpp;
using PMAPI;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using PMAPI.CustomSubstances;
using PMAPI.OreGen;
using Il2CppSystem;
using System;
using MelonLoader.TinyJSON;
using System.Text.Json;
using static stuff.stuff;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Numerics;
using static Il2Cpp.CubeGenerator;



namespace stuff
{
    public class stuff : MelonMod
    {
        public static Transform LHandTransform;
        public static Transform RHandTransform;
        public static PlayerLife PlayerLifeRef;


        public static Substance Stabisator;
        public static Substance reactor;
        public static Substance uranOre;
        public static Substance refineduran;
        public static Substance tungsten;
        public static Substance piston_head;
        public static Substance piston_base;
        //public static SubstanceManagerr manager;

        //public static private GameObject player;

        public override void OnInitializeMelon()
        {
            //base.OnInitializeMelon();
            PMAPIModRegistry.InitPMAPI(this);
            instance();

            ClassInjector.RegisterTypeInIl2Cpp<stabi>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<urore>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<reactorWork>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<nbeh>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });

            //piston
            ClassInjector.RegisterTypeInIl2Cpp<pstn_base>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<SubstanceID>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            }); 
            //piston end

            Registerstabisator();
            RegisteruranOre();
            Registeruran();
            Registerreactor();
            Registertungsten();
            RegisterPiston_base();
            RegisterPiston_head();
            //manager = new SubstanceManagerr();

            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(Substance.AncientAlloy, Substance.Rubber), new Il2CppSystem.ValueTuple<float, Substance, float>(0.25f, reactor, 1f));


            CustomOreManager.RegisterCustomOre(uranOre, new CustomOreManager.CustomOreParams
            {
                chance = 0.1f,
                substanceOverride = Substance.Stone,
                maxSize = 0.1f,
                minSize = 0.1f,
                alpha = 1f
            });



        }

        void Registertungsten()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "tungston",
                color = Color.white,
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "tungsten_ir_leon";
            param.material = cmat.name;
            param.density = 60f;
            param.strength = 600;
            param.stiffness = 1000;
            param.hardness = 1000f;
            param.isFlammable = false;
            //nbeh

            param.softeningPoint = 1000f;

            tungsten = CustomSubstanceManager.RegisterSubstance("tungsten", param, new CustomSubstanceParams
            {
                enName = "tungsten",
                deName = "tungsten",
                
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<radioprot>();
                    beh.prot = 1f;
                    return beh;
                }
            });
        }

        void Registeruran()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "refined_uran",
                color = Color.green,
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "refineduran_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;
            param.isFlammable = false;
            //nbeh

            param.softeningPoint = 1000f;

            refineduran = CustomSubstanceManager.RegisterSubstance("refineduran", param, new CustomSubstanceParams
            {
                enName = "refined uran",
                deName = "uran",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<nbeh>();
                    return beh;
                }
            });
        }

        void RegisteruranOre()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Sulfur"))
            {
                name = "uranOre",
                color = Color.green,
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Sulfur).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "uranOre_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;
            param.isFlammable = false;
            //nbeh

            param.thermalConductivity = 1000f;

            uranOre = CustomSubstanceManager.RegisterSubstance("uranOre", param, new CustomSubstanceParams
            {
                enName = "uranium ore",
                deName = "uran erz",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<urore>();
                    return beh;
                }
            });
        }
        void Registerstabisator()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Rubber"))
            {
                name = "stabisator",
                color = Color.black
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Rubber).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "stabisator_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;
            param.isFlammable = false;
            
            //nbeh

            param.softeningPoint = 200f;

            Stabisator = CustomSubstanceManager.RegisterSubstance("stabisator", param, new CustomSubstanceParams
            {
                enName = "stabisator",
                deName = "stabisator",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<stabi>();
                    return beh;
                }
            });

        }

        //functional

        void Registerreactor()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "reactor",
                color = Color.gray
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "reactor_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;
            param.thermalConductivity = 10000f;
            param.isFlammable = false;
            param.softeningPoint = 1000f;
            param.isConductor = true;
            //nbeh

            reactor = CustomSubstanceManager.RegisterSubstance("reactor", param, new CustomSubstanceParams
            {
                enName = "reactor",
                deName = "reactor",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<reactorWork>();
                    return beh;
                }
            });

        }
        //piston stuff
        void RegisterPiston_base()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "piston_base",
                color = Color.gray
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "piston_base_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 30;
            param.stiffness = 0;
            param.hardness = 4.0f;
            param.thermalConductivity = 0f;
            param.isFlammable = false;
            param.softeningPoint = 100000000;
            //nbeh

            piston_base = CustomSubstanceManager.RegisterSubstance("piston_base", param, new CustomSubstanceParams
            {
                enName = "piston base",
                deName = "piston unten",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<pstn_base>();
                    
                    return beh;
                }
            });
        }
        void RegisterPiston_head()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "piston_head",
                color = Color.gray
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "piston_head_ir_leon";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 30;
            param.stiffness = 0;
            param.hardness = 4.0f;
            param.thermalConductivity = 0f;
            param.isFlammable = false;
            param.softeningPoint = 100000000;
            //nbeh

            piston_head = CustomSubstanceManager.RegisterSubstance("piston_head", param, new CustomSubstanceParams
            {
                enName = "piston head",
                deName = "piston kopf",

            });

        }

        //stuff
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            MelonLogger.Msg("Hello Primitier!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            updatest();
            // Check for key press

            if (Input.GetKeyDown(KeyCode.Y))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 10f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), reactor);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 10f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.15f), Substance.LED);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                //1
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 10f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), Stabisator);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                //2
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 10f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), tungsten);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 11f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), Substance.AncientAlloy);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 10f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), Substance.Rubber);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                //3
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 11f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), uranOre);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                GameObject generatedCube = CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 3f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), piston_head);
                string id = System.Guid.NewGuid().ToString();
                //manager.substances.Add(id, generatedCube);
                var idComponent = generatedCube.AddComponent<SubstanceID>();
                idComponent.id = id;

                GameObject generatedCubeb = CubeGenerator.GenerateCube(mv.cameraTransform.position + new UnityEngine.Vector3(0f, 2f, 1f), new UnityEngine.Vector3(0.1f, 0.1f, 0.1f), piston_base);
                var idComponentb = generatedCubeb.GetComponent<pstn_base>();
                idComponentb.maxlenght = 4;
                idComponentb.speed = 0.01f;
                idComponentb.head_id = id;
                
            }
            if (Input.GetKeyDown(KeyCode.R))
            {

            }
        }

        public class urore : MonoBehaviour
        {

            public urore(System.IntPtr ptr) : base(ptr)
            {

            }

            CubeBase cubeBase;
            bool updated = false;

            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();
                cubeBase.enabled = true;
                //MelonLogger.Msg("OreBeh has initialized");
            }

            void Update()
            {

                    if (!updated)
                    {
                        

                        // Check if the cube is hot enough to be transformed
                        if (cubeBase.heat.GetCelsiusTemperature() > 600.0)
                        {
                            cubeBase.ChangeSubstance(refineduran);
                            updated = true;
                        }
                    }

            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
        }

        public class stabi : MonoBehaviour
        {

            void Update()
            {
                if (!updated)
                {
                    // Ensure the stabi object's rotation is set to identity (i.e., no rotation)
                    //transform.rotation = Quaternion.identity;
                    MelonLogger.Msg(cubeBase.transform.childCount);
                    cubeBase.transform.rotation = UnityEngine.Quaternion.identity;
                    
                    foreach (GameObject parent in cubeBase.transform.parent)
                    {
                        parent.transform.rotation = UnityEngine.Quaternion.identity;
                    }
                }
            }

            public stabi(System.IntPtr ptr) : base(ptr)
            {

            }

            CubeBase cubeBase;
            bool updated = false;

            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();
                cubeBase.enabled = true;
                //MelonLogger.Msg("OreBeh has initialized");
            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
        }


        public class reactorWork : MonoBehaviour
        {
            float maintemp = 3000f;
            float temp;
            float ctemp;
            float inttemp = 696f;
            float explodetemp = 3000f;
            float genspeed = 0.01f;
            float maxpower = 100f;

            public reactorWork(System.IntPtr ptr) : base(ptr)
            {

            }

            CubeBase cubeBase;
            bool updated = false;

            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();
                cubeBase.enabled = true;
                //MelonLogger.Msg("OreBeh has initialized");
            }

            void Update()
            {
                if (!updated)
                {
                    temp = maintemp - (cubeBase.heat.GetCelsiusTemperature() + 1);
                    if (temp >= inttemp)
                    {
                        temp = inttemp;
                    }
                    ctemp = cubeBase.heat.Temperature;
                    MelonLogger.Msg(ctemp);
                    cubeBase.heat.AddHeat(temp);
                    float en = cubeBase.electricPart.energy;
                    MelonLogger.Msg(en.ToString());
                    MelonLogger.Msg(Il2CppSystem.Math.Max(1 + (temp * genspeed), maxpower));
                    cubeBase.electricPart.energy = Il2CppSystem.Math.Max(en + (temp * genspeed),maxpower);
                    //if (explodetemp <= ctemp)
                    //{
                    //    cubeBase.AncientExplode();
                        //Destroy(cubeBase);
                    //}
                }
            }


            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }


        }

        public class nbeh : MonoBehaviour
        {
            void Update()
            {

            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
            void OnInitialize()
            {

            }

        }
        //piston beh
        
        public class pstn_base : MonoBehaviour
        {

            public pstn_base(System.IntPtr ptr) : base(ptr)
            {

            }

            CubeBase cubeBase;
            public float maxlenght { get; set; }
            public float speed { get; set; }
            public string head_id { get; set; }
            bool updated = false;

            public void OnFragmentInitialized(CubeBase fragmentCubeBase)
            {
                Destroy(cubeBase);
            }
            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();
                cubeBase.enabled = true;
                //cubeBase.AncientExplode();
                //Destroy(cubeBase);
                //cubeBase.maxLife
                //MelonLogger.Msg("OreBeh has initialized");

            }
            void OnCollisionEnterReceived()
            {
                updated = true;
                MelonLogger.Msg("cut test");
            }

            void Update()
            {
                if (!updated)
                {
                    if (maxlenght < 1)
                    { maxlenght = 1; }
                    if (speed <= 0)
                    { speed = 0.1f; }
                    //if(!string.IsNullOrEmpty(head_id))
                    //{ head_id = "null"; }
                    if (maxlenght != 0 && speed != 0 && head_id != "")
                    {
                        string head_id_got = head_id;
                        UnityEngine.Vector3 mypos = cubeBase.gameObject.transform.position;
                        UnityEngine.Vector3 calcpos = mypos + new UnityEngine.Vector3(0f, 0.2f, 0f);
                        Transform myrot = cubeBase.transform;
                        //manager.UpdateSubstance_dir(head_id_got, myrot);
                        //MelonLogger.Msg("speed:" + speed);
                        //MelonLogger.Msg("maxlenght:" + maxlenght);
                        //MelonLogger.Msg("my_piston_head:" + head_id_got);
                    }
                    if(head_id == "")
                    {
                        updated = true;
                        cubeBase.AncientExplode();
                        MelonLogger.Msg("cut test");
                    }
                    
                }
            }


            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }

            
        }
        public class SubstanceID : MonoBehaviour
        {
            public string id;
        }


        /*public class SubstanceManagerr
        {
            public Dictionary<string, GameObject> substances = new Dictionary<string, GameObject>();
            // Existing methods...

            public void UpdateSubstance_dir(string id, Transform newRotation)
            {
                if (substances.TryGetValue(id, out GameObject substanc))
                {
                    //substanc.transform.localRotation = newRotation;
                    //substanc.gameObject.transform.localRotation = newRotation;
                    //MelonLogger.Msg(substanc.transform.eulerAngles.ToString());
                    
                    Transform gt = substanc.transform;
                    if (gt != null)
                    {
                        MelonLogger.Msg(gt.parent.ToString());
                        foreach (Transform t in gt.parent.parentInternal)
                        {
                            t.rotation = newRotation.rotation;
                            
                        }
                        gt.rotation = newRotation.rotation;
                    }
                    //substanc.gameObject.transform.eulerAngles = eu;
                    //.gameObject.cubeConnector.transform

                }
            }
            public void UpdateSubstance_pos(string id, Vector3 newPosition)
            {
                if (substances.TryGetValue(id, out GameObject substanc))
                {
                    //substanc.gameObject.transform.position = newPosition;
                    var TransformComponent = substanc.Cast<Transform>();

                    TransformComponent.rotation = Quaternion.identity;
                }
            }
            private void RotateObjectAndChildren(GameObject parent, Quaternion newRotation)
            {
                //parent.gameObject.GetComponentsInChildren<Transform>().
                
            }
            public void DeleteSubstanceById(string id)
            {
                if (substances.TryGetValue(id, out GameObject substanc))
                {
                    substances.Remove(id);
                    //substanc
                }
            }

            // Other existing methods...
        }*/

        //piston beh end


        void instance()
        {
            ClassInjector.RegisterTypeInIl2Cpp<radioprot>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
        }

        void updatest()
        {
            //PlayerLifeRef = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();
            //LHandTransform = PlayerLifeRef.grabberL.realHand;
            //RHandTransform = PlayerLifeRef.grabberR.realHand;

        }

        public class radioprot : MonoBehaviour
        {
            public float prot;
        }
        /*public static void CastRadiationRays(CubeBase obj,float damage,float maxDistance)
        {
            int numberOfRays = 30; // Number of rays to cast
            var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
            for (int i = 0; i < numberOfRays; i++)
            {
                UnityEngine.Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 0.5f; // Randomize raycast positions around player
                UnityEngine.Vector3 rayDirection = (mv.cameraTransform.position + randomOffset) - obj.transform.position;

                RaycastHit[] hits = Physics.RaycastAll(obj.transform.position, rayDirection, maxDistance);

                foreach (RaycastHit hit in hits)
                {
                    Material hitMaterial = hit.collider.GetComponent<Renderer>()?.material;
                    GameObject hitGO = hit.collider.gameObject;

                    if (hitMaterial != null)
                    {
                        float radiationResistance = hitGO.GetComponent<radioprot>().prot;
                        if (radiationResistance > 0)
                        {
                            float calc = damage - radiationResistance;

                            // If the material has radiation resistance >= 3f, stop the ray
                            if (0f >= calc)
                            {
                                Debug.Log("Hit material with radiation resistance " + damage + ". Stopping ray.");
                                break;
                            }
                            else
                            {

                            }
                        }
                    }

                    // Continue processing if we didn't break
                    Debug.Log("Raycast hit: " + hit.collider.name);
                }
            }
        }*/

    }
}