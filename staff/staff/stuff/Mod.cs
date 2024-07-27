using Il2CppSystem;
using PrimitierModdingFramework;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

namespace stuff
{
    public class Mod : PrimitierMod
    {
        private Substance stabisator;
        private GameObject player;

        public virtual void OnInitializeMelon()
        {
            PMAPIModRegistry.InitPMAPI((MelonMod)this);
            //ClassInjector.RegisterTypeInIl2Cpp<TestBeh>(new RegisterTypeOptions()
            {
                Interfaces = Il2CppInterfaceCollection.op_Implicit(new Type[2]
                {
                    typeof(ICubeBehavior),
                    typeof(ISavable)
                }
                );
            });
            this.Registerstabisator();
        }

        private void Registerstabisator()
        {
            Material mat = new Material(SubstanceManager.GetMaterial("Rubber"));
            ((Object)mat).name = "stabisator";
            mat.color = Color.black;
            CustomMaterialManager.RegisterMaterial(mat);
            SubstanceParameters.Param substanceParams = ((Il2CppObjectBase)((Object)SubstanceManager.GetParameter((Substance)34)).MemberwiseClone()).Cast<SubstanceParameters.Param>();
            substanceParams.strength = 2000f;
            this.wheelium = CustomSubstanceManager.RegisterSubstance("stabisator", substanceParams, new CustomSubstanceParams()
            {
                enName = "stabisator",
                deName = "stabisator"
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
        public virtual void OnUpdate()
        {

            // Check for key press
            if (Input.GetKeyDown(KeyCode.T))
            {
                CubeGenerator.GenerateCube(Vector3.op_Addition(GameObject.Find("XR Origin").GetComponent<PlayerMovement>().cameraTransform.position, new Vector3(0.0f, 3f, 1f)), Vector3.one, this.stabisator, (CubeAppearance.SectionState)0, (CubeAppearance.UVOffset)null, "");
            }
        }

        private void OnObjectCreated(GameObject obj)
        {
            // Check if the created object is a stabisator
            if (obj.name == "stabisator")
            {
                MelonLogger.Msg("Stabisator created, making it non-rotatable and adding stability check.");
                obj.AddComponent<NonRotatable>();
                obj.AddComponent<StabilityChecker>();
            }
        }

        private void SpawnStabisator()
        {
            if (player == null)
            {
                player = GameObject.Find("XR Origin").GetComponent<PlayerMovement>().cameraTransform.gameObject;
                if (player == null)
                {
                    MelonLogger.Msg("Player object not found.");
                    return;
                }
            }

            Vector3 spawnPosition = player.transform.position + new Vector3(0.0f, 10f, 1f);
            GameObject stabisator = CubeGenerator.GenerateCube(spawnPosition, new Vector3(0.1f, 1f, 0.1f), (Substance)16, (CubeAppearance.SectionState)0, (CubeAppearance.UVOffset)null, "");
            stabisator.name = "stabisator";
            MelonLogger.Msg("Spawned stabisator at " + spawnPosition);
        }


        void ApplyForceToNearbyObjects(float forceRadius, float acceleration)
        {
            var colliders = Physics.OverlapSphere(cubeBase.transform.position, forceRadius);
            foreach (var hit in colliders)
            {

                Component[] components = hit.transform.parent.gameObject.GetComponents<Component>();
                foreach (Component component in components)
                {
                    MelonLogger.Msg(component.ToString());
                }
            }
        }

        // Component to prevent rotation
        public class NonRotatable : MonoBehaviour
        {
            void Update()
            {
                // Set rotation to identity to prevent rotation
                transform.rotation = Quaternion.identity;
            }
        }

        // Component to check stability based on energy
        public class StabilityChecker : MonoBehaviour
        {
            private bool hasEnergy;

            void Update()
            {
                // Placeholder for energy check logic
                // Replace this with actual energy check logic
                hasEnergy = CheckForEnergy();

                if (!hasEnergy)
                {
                    MelonLogger.Msg("Stabisator has no energy, becoming unstable.");
                    // Logic to make the stabisator unstable
                    // For example, you can destroy the object or disable its functionality
                    Destroy(gameObject);
                }
            }

            private bool CheckForEnergy()
            {
                // Implement your logic to check for energy
                // This is a placeholder, you need to replace it with actual game logic
                return true; // Default to no energy
            }
        }
    } 
}
