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
        Substance Stabisator;
        //private GameObject player;

        public virtual void OnInitializeMelon()
        {
            PMAPIModRegistry.InitPMAPI(this);
            ClassInjector.RegisterTypeInIl2Cpp<stabi>(new RegisterTypeOptions
            {
                Interfaces = new[] { typeof(ICubeBehavior), typeof(ISavable) }
            });
            this.Registerstabisator();
            CubeMerge.compoundablePairs.Add(new Il2CppSystem.ValueTuple<Substance, Substance>(Substance.AncientAlloy, Substance.Iron), new Il2CppSystem.ValueTuple<float, Substance, float>(0.25f, Stabisator, 1f));
        
        }

        public void Registerstabisator()
        {
            Material cmat = new(SubstanceManager.GetMaterial("Rubber"))
            {
                name = "stabisator",
                color = Color.black
            };

            CustomMaterialManager.RegisterMaterial(cmat);

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
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var mv = GameObject.Find("XR Origin").GetComponent<PlayerMovement>();
                CubeGenerator.GenerateCube(mv.cameraTransform.position + new Vector3(0f, 10f, 1f), new Vector3(10f, 10f, 10f), Stabisator);
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
