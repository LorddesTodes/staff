using MelonLoader;
using Il2Cpp;
using PMAPI;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using PMAPI.CustomSubstances;
using PMAPI.OreGen;
using UnityEngine.SceneManagement;
using System.Text.Json;
using Il2CppSystem;
using System;


namespace stuff
{
    public class stuff : MelonMod
    {
        public static Substance Stabisator;
        public static Substance reactor;
        public static Substance uranOre;
        public static Substance refineduran;
        public static Substance beltob;
        public static Substance tungsten;
        //public static private GameObject player;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            PMAPIModRegistry.InitPMAPI(this);

            ClassInjector.RegisterTypeInIl2Cpp<stabi>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<urore>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<reactorWork>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            }); 
            ClassInjector.RegisterTypeInIl2Cpp<belt>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            }); 
            ClassInjector.RegisterTypeInIl2Cpp<nbeh>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            });

            Registerstabisator();
            RegisteruranOre();
            Registeruran();
            Registerreactor();
            Registerbelt();
            Registertungsten();

            CustomOreManager.RegisterCustomOre(uranOre, new CustomOreManager.CustomOreParams
            {
                chance = 0.1f,
                substanceOverride = Substance.Stone,
                maxSize = 0.3f,
                minSize = 0.3f,
                alpha = 1f
            });

            
            var key = new Il2CppSystem.ValueTuple<Substance, Substance>(Substance.Rubber, Substance.AncientAlloy);
            var value = new Il2CppSystem.ValueTuple<float, Substance, float>(1f, beltob, 2f);

            
            // Check if the key already exists in the dictionary
            if (!CubeMerge.compoundablePairs.ContainsKey(key))
            {
                // Add the compoundable pair only if the key does not exist

                CubeMerge.compoundablePairs.Add(key, value);
            }
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
                    var beh = cb.gameObject.AddComponent<nbeh>();
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

        void Registerbelt()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Rubber"))
            {
                name = "belt",
                color = Color.gray
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Rubber).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "belt_ir_leon";
            param.material = cmat.name;
            param.density = 2f;
            param.strength = 300;
            param.stiffness = 6;
            param.hardness = 20.0f;
            param.isFlammable = false;
            //nbeh

            param.softeningPoint = 200f;


            beltob = CustomSubstanceManager.RegisterSubstance("belt", param, new CustomSubstanceParams
            {
                enName = "belt",
                deName = "flißband",
                behInit = (cb) =>
                {
                    // Adding test behavior
                    var beh = cb.gameObject.AddComponent<belt>();
                    return beh;
                }
            });

        }
        
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
            param.thermalConductivity = 1000f;
            param.isFlammable = false;
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
        
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            MelonLogger.Msg("Hello Primitier!");
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            // Check for key press
            if (Input.GetKeyDown(KeyCode.S))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), beltob);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), reactor);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), Stabisator);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), tungsten);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 11f, 1f), new Vector3(0.1f, 0.1f, 0.1f), Substance.AncientAlloy);
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), Substance.Rubber);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 11f, 1f), new Vector3(0.1f, 0.1f, 0.1f), uranOre);
            }
        }
        
        public class urore : MonoBehaviour
        {
            CubeBase cubeBase;
            bool updated = true;

            void OnInitialize()
            {
                try
                {
                    MelonLogger.Msg("uranore initialized");
                    // Get the cube base
                    cubeBase = GetComponent<CubeBase>();
                    if (cubeBase == null)
                    {
                        return;
                    }
                    cubeBase.enabled = true;
                    updated = false;
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"Exception in urore OnInitialize: {ex.Message}");
                }
            }

            void Update()
            {
                try
                {
                    if (!updated)
                    {
                        if (cubeBase == null)
                        {
                            MelonLogger.Error("cubeBase is null in Update");
                            return;
                        }

                        // Check if the cube is hot enough to be transformed
                        if (cubeBase.heat.GetCelsiusTemperature() > 600.0)
                        {
                            cubeBase.ChangeSubstance(refineduran);
                            updated = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"Exception in urore Update: {ex.Message}");
                }
            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
        }
        /*
        public class urore : MonoBehaviour
        {
            CubeBase cubeBase;
            bool updated = true;
            void OnInitialize()
            {
                MelonLogger.Msg("uranore");
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();
                cubeBase.enabled = true;
                updated = false;
                //MelonLogger.Msg("OreBeh has initialized");
            }
            void Update()
            {
                if (!updated)
                {
                    cubeBase = GetComponent<CubeBase>();
                    // check if the cube is hot enough to be transformed
                    if (cubeBase.heat.GetCelsiusTemperature() > 600.0)
                    {
                        //MelonLogger.Msg("Hit temp target to turn into refined uran");
                        cubeBase.ChangeSubstance(refineduran);
                        updated = true;
                    }
                }
            }
        }*/
        public class stabi : MonoBehaviour
        {
            CubeBase cubeBase;

            void Update()
            {
                // Ensure the stabi object's rotation is set to identity (i.e., no rotation)
                    transform.rotation = Quaternion.identity;

            }

            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();

                // Make it hooooooot
                //cubeBase.heat.AddHeat(1000000f);
            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
        }

        public class belt : MonoBehaviour
        {
            CubeBase cubeBase;
            Quaternion rot;

            void Update()
            {
                // Ensure the stabi object's rotation is set to identity (i.e., no rotation)
                

                // Check if the player is holding this Stabisator
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                var heldObject = mv.GetComponent<CubeBase>();
                MelonLogger.Msg(mv);
                MelonLogger.Msg(heldObject);
                MelonLogger.Msg(heldObject.GetComponent<CubeBase>().substance);
                if (heldObject != null && heldObject.GetComponent<CubeBase>().substance == beltob)
                {
                    // Get the rotation of the held object
                    rot = heldObject.transform.rotation;
                }else
                {
                    transform.rotation = rot;
                }
            }

            void OnInitialize()
            {
                // Get the cube base
                rot = Quaternion.identity;
                cubeBase = GetComponent<CubeBase>();

                // Make it hooooooot
                //cubeBase.heat.AddHeat(1000000f);
            }
            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }
        }

        public class reactorWork : MonoBehaviour
        {
            CubeBase cubeBase;
            float maintemp = 3000f;
            float temp = 0f;
            float inttemp = 696f;


            void Update()
            {
                cubeBase = GetComponent<CubeBase>();
                if (true)
                {
                    temp = maintemp - (cubeBase.heat.GetCelsiusTemperature()+1);
                    if (temp >= inttemp)
                    {
                        temp = inttemp;
                    }
                    cubeBase.heat.AddHeat(temp);
                }
            }


            void Start()
            {
                //MelonLogger.Msg("OreBeh has started");
            }


            void OnInitialize()
            {
                // Get the cube base
                

                // Make it hooooooot
                //cubeBase.heat.AddHeat(1000000f);
                //cubeBase.
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
    }
}
