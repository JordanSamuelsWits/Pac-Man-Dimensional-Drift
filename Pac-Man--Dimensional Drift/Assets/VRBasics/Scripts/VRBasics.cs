
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the manager for VRBasics. It is a Singleton class that will be in every scene without reconfiguration.
/// In your Layer manager, you must add a layer called Ignore Collisions.
/// Assign the index of this layer to the ignoreCollisionsLayer property of this class.
/// Under Edit -> Project Settings -> Physics use the Layer Collision Matrix to disable
/// all collisions between the Ignore Collisions layer and all other layers
/// this gives your application a layer where all collisions are ignored
/// and allows some of the concepts of VRBasics to function properly. (See Read Me file)
/// </summary>
public class VRBasics : MonoBehaviour
{
	private static VRBasics _instance = null;                           // will hold a single instance of this class
    public static VRBasics Instance { get { return _instance; } }       // returns the instance of this class

    [Tooltip("True if the application determines which VR device is active.")]
    public bool autoVRType = false;

	public enum VRTypes{SteamVR, OVR};                                  // a list of possible VR types supported by VRBasics
    [Tooltip("A list of possible VR types supported by VRBasics.")]
    public VRTypes vrType;

    [Tooltip("Index of the ignore collisions layer. Set up in the Physics Manager.")]
    public int ignoreCollisionsLayer = 8;
    [Tooltip("Controls the actual size of eye textures as a multiplier of the device's default resolution.")]
	public float renderScale = 1.0f;

    /// <summary>
    /// Create and maintain the singleton pattern for VRBasics.
    /// Sets up collisions between touchers and pushers of controllers.
    /// Sets size of eye textures.
    /// </summary>
    void Awake()
    {
        // insure this object class is a singleton
        // if there is already an instance of this class
        // and it is not this object
        if (_instance != null && _instance != this)
        {
            // get rid of any other instance
            Destroy(this.gameObject);
        }
        else
        {
            // make this the single instance
            _instance = this;
            // keep in all scenes
            DontDestroyOnLoad(this);
        }

        // ignore collisions between all Touchers
        List<GameObject> touchers = GetAllTouchers();
        int numTouchers = touchers.Count;
        for (int t = 0; t < numTouchers; t++)
        {
            IgnoreAllTouchers(touchers[t]);
        }

        // ignore collisions between all Pushers
        List<GameObject> pushers = GetAllPushers();
        int numPushers = pushers.Count;
        for (int p = 0; p < numPushers; p++)
        {
            IgnoreAllPushers(pushers[p]);
        }

        // ignore collisions between all Touchers and Pushers
        for (int t = 0; t < numTouchers; t++)
        {
            IgnoreAllPushers(touchers[t]);
        }

        // set size of eye textures
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = renderScale;
    }

    /// <summary>
    /// When the scene is loaded adds OnSceneLoaded as a delegate to run.
    /// </summary>
    void OnEnable()
    {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

    /// <summary>
    /// After the scene has loaded, if the application determines which VR device is in use,
    /// VRBasics activates the cooresponding camera rig. And deactivates the unneeded one.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		// if using automatic detection of connected device
		if (autoVRType)
        {
			string model = UnityEngine.XR.XRDevice.model != null ? UnityEngine.XR.XRDevice.model : "";
			Debug.Log (model);

            if (model.ToLower().Contains("oculus"))
            {
                VRBasics.Instance.vrType = VRBasics.VRTypes.OVR;
                GameObject.Find("SteamVRCameraRig [VRBasics]").SetActive(false);
                GameObject.Find("OVRCameraRig [VRBasics]").SetActive(true);
            }
            else
            {
                VRBasics.Instance.vrType = VRBasics.VRTypes.SteamVR;
                GameObject.Find("SteamVRCameraRig [VRBasics]").SetActive(true);
                GameObject.Find("OVRCameraRig [VRBasics]").SetActive(false);
            }

        // if not using automatic detection of connected device
		}
        else
        {
            if(vrType == VRTypes.SteamVR)
            {
                GameObject.Find("SteamVRCameraRig [VRBasics]").SetActive(true);
                GameObject.Find("OVRCameraRig [VRBasics]").SetActive(false);
            }
            else if (vrType == VRTypes.OVR)
            {
                GameObject.Find("SteamVRCameraRig [VRBasics]").SetActive(false);
                GameObject.Find("OVRCameraRig [VRBasics]").SetActive(true);
            }
        }
	}

    /// <summary>
    /// Removes OnSceneLoaded delegate when scene is changed.
    /// </summary>
	void OnDisable()
    {
		SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Get a list of all VRBasics_Controllers.
    /// </summary>
    /// <returns>A list of game objects of the VRBasics_Controller class.</returns>
	public List<GameObject> GetAllControllers()
    {
		List<GameObject> controllers = new List<GameObject> ();
		foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
			if (go.GetComponent<VRBasics_Controller> ())
            {
				controllers.Add (go);
			}
		}

		return controllers;
	}

    /// <summary>
    /// Get a list of all game objects named "Toucher" that are children of VRBasics_Controllers.
    /// Touchers allow a controller to touch objects with out moving them.
    /// </summary>
    /// <returns>A list of game objects.</returns>
	public List<GameObject> GetAllTouchers()
    {
		List<GameObject> controllers = GetAllControllers ();
		List<GameObject> touchers = new List<GameObject> ();

		int numControllers = controllers.Count;
		for (int c = 0; c < numControllers; c++)
        {
			int numChildren = controllers [c].transform.childCount;
			for (int ch = 0; ch < numChildren; ch++)
            {
				if (controllers [c].transform.GetChild (ch).name == "Toucher")
                {
					touchers.Add (controllers [c].transform.GetChild (ch).gameObject);
				}
			}
		}

		return touchers;
	}

    /// <summary>
    /// Get a list of all game objects named "Pusher" that are children of "Toucher"" objects.
    /// Pushers allow controllers to move objects with a touch.
    /// </summary>
    /// <returns>A list of game objects.</returns>
	public List<GameObject> GetAllPushers()
    {
		List<GameObject> touchers = GetAllTouchers ();
		List<GameObject> pushers = new List<GameObject> ();

		int numTouchers = touchers.Count;
		for (int t = 0; t < numTouchers;t++)
        {
			int numChildren = touchers [t].transform.childCount;
			for (int ch = 0; ch < numChildren; ch++)
            {
				if (touchers [t].transform.GetChild (ch).name == "Pusher")
                {
					pushers.Add (touchers [t].transform.GetChild (ch).gameObject);
				}
			}
		}

		return pushers;
	}

    /// <summary>
    /// Makes a game object get ignored by collisions with Touchers.
    /// </summary>
    /// <param name="other">The game object to ignore collisions with Touchers.</param>
	public void IgnoreAllTouchers(GameObject other)
    {
		List<GameObject> touchers = GetAllTouchers ();
		int numTouchers = touchers.Count;
		for (int t = 0; t < numTouchers; t++)
        {
			Physics.IgnoreCollision (touchers [t].GetComponent<Collider> (), other.GetComponent<Collider> ());
		}
	}

    /// <summary>
    /// Makes a game object get ignored by collisions with Pushers.
    /// </summary>
    /// <param name="other">The game object to ignore collisions with Pushers.</param>
	public void IgnoreAllPushers(GameObject other)
    {
		List<GameObject> pushers = GetAllPushers ();
		int numPushers = pushers.Count;
		for (int p = 0; p < numPushers; p++)
        {
			Physics.IgnoreCollision (pushers [p].GetComponent<Collider> (), other.GetComponent<Collider> ());
		}
	}

    /// <summary>
    /// Makes a game object collide with Touchers.
    /// Usually used on objects that have had that collision previously ignored for some reason.
    /// </summary>
    /// <param name="other">The game object to collide with Touchers.</param>
    public void CollideWithAllTouchers(GameObject other)
    {
        List<GameObject> touchers = GetAllTouchers();
        int numTouchers = touchers.Count;
        for (int t = 0; t < numTouchers; t++)
        {
            Physics.IgnoreCollision(touchers[t].GetComponent<Collider>(), other.GetComponent<Collider>(), false);
        }
    }

    /// <summary>
    /// Makes a game object collide with Pushers.
    /// Usually used on objects that have had that collision previously ignored for some reason.
    /// </summary>
    /// <param name="other">The game object to collide with Pushers.</param>
    public void CollideWithAllPushers(GameObject other)
    {
        List<GameObject> pushers = GetAllPushers();
        int numPushers = pushers.Count;
        for (int p = 0; p < numPushers; p++)
        {
            Physics.IgnoreCollision(pushers[p].GetComponent<Collider>(), other.GetComponent<Collider>(), false);
        }
    }
}


