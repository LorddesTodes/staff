using MelonLoader;
using Il2Cpp;
using PMAPI;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using PMAPI.CustomSubstances;
using PMAPI.OreGen;
using UnityEngine.SceneManagement;
using System.Text.Json;

namespace stuff
{
    public class stuff : MelonMod
    {
        public static Substance Stabisator;
        //public static Substance reactor;
        public static Substance uranOre;
        public static Substance refineduran;
        //public static private GameObject player;

        public virtual void OnInitializeMelon()
        {
            MelonLogger.Msg("init start");
            PMAPIModRegistry.InitPMAPI(this);
            MelonLogger.Msg("init done");

            ClassInjector.RegisterTypeInIl2Cpp<stabi>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior) }
            });
            ClassInjector.RegisterTypeInIl2Cpp<urore>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            });
            Registerstabisator();
            RegisteruranOre();
            Registeruran();
            //this.Register();
            
            CustomOreManager.RegisterCustomOre(uranOre, new CustomOreManager.CustomOreParams
            {
                chance = 10f,
                substanceOverride = Substance.Stone,
                maxSize = 0.3f,
                minSize = 0.3f,
                alpha = 1f
            });

            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(Substance.AncientAlloy, Substance.Iron), new Il2CppSystem.ValueTuple<float, Substance, float>(0.25f, Stabisator, 1f));
        
        }
        public void Registeruran()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Iron"))
            {
                name = "refined_uran",
                color = Color.green,
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Iron).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "refineduran";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;

            refineduran = CustomSubstanceManager.RegisterSubstance("refineduran", param, new CustomSubstanceParams
            {
                enName = "refined uran",
                deName = "uran",
                //behInit = (cb) =>
                //{
                    // Adding test behavior
                //    var beh = cb.gameObject.AddComponent<urore>();
                //    return beh;
                //}
            });
        }

        public void RegisteruranOre()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Sulfur"))
            {
                name = "uranOre",
                color = Color.green,
            };

            CustomMaterialManager.RegisterMaterial(cmat);

            var param = SubstanceManager.GetParameter(Substance.Sulfur).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "uranOre";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;

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
        public void Registerstabisator()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Rubber"))
            {
                name = "stabisator",
                color = Color.black
            };

            CustomMaterialManager.RegisterMaterial(cmat);
            MelonLogger.Msg(cmat);
            var param = SubstanceManager.GetParameter(Substance.Rubber).MemberwiseClone().Cast<SubstanceParameters.Param>();
            param.displayNameKey = "stabisator";
            param.material = cmat.name;
            param.density = 2.5f;
            param.strength = 10;
            param.stiffness = 30;
            param.hardness = 4.0f;

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
            MelonLogger.Msg(Stabisator);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            MelonLogger.Msg("Hello Primitier!");
        }
        /*
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            //PMFSystem.EnableSystem<PMFHelper>();

            // Hook into the object creation event
            //PMFHelper.OnObjectCreated += OnObjectCreated;

            // Add the custom crafting recipe
            //AddCraftingRecipe();
        }
        */
        public override void OnUpdate()
        {

            // Check for key press
            if (Input.GetKeyDown(KeyCode.T))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(0.1f, 0.1f, 0.1f), Stabisator);
                MelonLogger.Msg(Stabisator);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(10f, 10f, 10f), Stabisator);
            }
        }

        public class urore : MonoBehaviour
        {
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
                    // check if the cube is hot enough to be transformed
                    if (cubeBase.heat.GetCelsiusTemperature() > 600.0)
                    {
                        //MelonLogger.Msg("Hit temp target to turn into refined uran");
                        cubeBase.ChangeSubstance(stuff.refineduran);
                        updated = true;
                    }
                }
            }
        }

        public class stabi : MonoBehaviour
        {
            void Update()
            {
                transform.rotation = Quaternion.identity;
            }



            CubeBase cubeBase;

            void OnInitialize()
            {
                // Get the cube base
                cubeBase = GetComponent<CubeBase>();

                // Make it hooooooot
                cubeBase.heat.AddHeat(1000000f);
            }
        }
    }
}
