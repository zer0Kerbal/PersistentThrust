using System;
using System.Linq;
using UnityEngine;

namespace PersistentThrust {

    public class SolarSailPart : PartModule {

	// Persistent Variables
	// Sail enabled
	[KSPField(isPersistant = true)]
	public bool IsEnabled = false;
	
	// Persistent False
	[KSPField]
	public float reflectedPhotonRatio = 1f;
	[KSPField]
	public float surfaceArea; // Sail surface area
	[KSPField]
	public string animName;
	
	// GUI
	// Force and Acceleration
	[KSPField(guiActive = true, guiName = "Force")]
	protected string forceAcquired = "";
	[KSPField(guiActive = true, guiName = "Acceleration")]
	protected string solarAcc = "";
	
	protected Transform surfaceTransform = null;
	protected Animation solarSailAnim = null;

	// Reference distance for calculating thrust: sun to Kerbin (m)
	const double kerbin_distance = 13599840256;
	// Solar pressure N/m^2 at reference distance
	const double thrust_coeff = 9.08e-6;

	// Display numbers for force and acceleration
	protected double solar_force_d = 0;
	protected double solar_acc_d = 0;

	// GUI to deploy sail
	[KSPEvent(guiActive = true, guiName = "Deploy Sail", active = true)]
	public void DeploySail() {
	    if (animName != null && solarSailAnim != null) {
		solarSailAnim[animName].speed = 1f;
		solarSailAnim[animName].normalizedTime = 0f;
		solarSailAnim.Blend(animName, 2f);
	    }
	    IsEnabled = true;
	}
	
	// GUI to retract sail
	[KSPEvent(guiActive = true, guiName = "Retract Sail", active = false)]
	public void RetractSail() {
	    if (animName != null && solarSailAnim != null) {
		solarSailAnim[animName].speed = -1f;
		solarSailAnim[animName].normalizedTime = 1f;
		solarSailAnim.Blend(animName, 2f);
	    }
	    IsEnabled = false;
	}
	
	// Initialization
	public override void OnStart(StartState state) {

	    if (state != StartState.None && state != StartState.Editor) {

		if (animName != null) {
		    solarSailAnim = part.FindModelAnimators(animName).FirstOrDefault();
		}

		
		if (IsEnabled) {
		    solarSailAnim[animName].speed = 1f;
		    solarSailAnim[animName].normalizedTime = 0f;
		    solarSailAnim.Blend(animName, 0.1f);
		}

		this.part.force_activate();
	    }
	}

	public override void OnUpdate() {
	    // Sail deployment GUI
	    Events["DeploySail"].active = !IsEnabled;
	    Events["RetractSail"].active = IsEnabled;
	    // Text fields (acc & force)
	    Fields["solarAcc"].guiActive = IsEnabled;
	    Fields["forceAcquired"].guiActive = IsEnabled;
	    forceAcquired = solar_force_d.ToString("E") + " N";
	    solarAcc = solar_acc_d.ToString("E") + " m/s";
	}
	
	public override void OnFixedUpdate() {
	    if (FlightGlobals.fetch != null) {
		
		double UT = Planetarium.GetUniversalTime();
		double dT = TimeWarp.fixedDeltaTime;
		
		solar_force_d = 0;
		if (!IsEnabled) { return; }
		
		double sunlightFactor = 1.0;

		// Not in sunlight
		if (!inSun(vessel.orbit, UT)) {
		    sunlightFactor = 0.0;
		    solar_force_d = 0.0;
		    solar_acc_d = 0.0;
		}
		// If in sunlight
		else {
		    // Force from sunlight
		    Vector3d solarForce = CalculateSolarForce(this, vessel.orbit, this.part.transform.up, UT) * sunlightFactor;
		    // Acceleration from sunlight
		    Vector3d solarAccel = solarForce / vessel.GetTotalMass() / 1000.0;

		    // Apply acceleration
		    // Realtime
		    if (!this.vessel.packed) {
			vessel.ChangeWorldVelocity(solarAccel * dT);
		    }
		    // Timewarp
		    else {
			vessel.orbit.Perturb(solarAccel * dT, UT);
		    }

		    // Update displayed force & acceleration
		    solar_force_d = solarForce.magnitude;
		    solar_acc_d = solarAccel.magnitude;
		}
	    }
	}

	// Test if an orbit at UT is in sunlight
	public static bool inSun(Orbit orbit, double UT) {
	    Vector3d a = orbit.getPositionAtUT(UT);
	    Vector3d b = FlightGlobals.Bodies[0].getPositionAtUT(UT);
	    foreach (CelestialBody referenceBody in FlightGlobals.Bodies) {
		if (referenceBody.flightGlobalsIndex == 0) { // the sun should not block line of sight to the sun
		    continue;
		}
		Vector3d refminusa = referenceBody.getPositionAtUT(UT) - a;
		Vector3d bminusa = b - a;
		if (Vector3d.Dot(refminusa, bminusa) > 0) {
		    if (Vector3d.Dot(refminusa, bminusa.normalized) < bminusa.magnitude) {
			Vector3d tang = refminusa - Vector3d.Dot(refminusa, bminusa.normalized) * bminusa.normalized;
			if (tang.magnitude < referenceBody.Radius) {
			    return false;
			}
		    }
		}
	    }
	    return true;
	}

	private static double solarForceAtDistance(Vector3d sunPosition, Vector3d ownPosition) {
	    double distance_from_sun = Vector3.Distance(sunPosition, ownPosition);
	    double force_to_return = thrust_coeff * kerbin_distance * kerbin_distance / distance_from_sun / distance_from_sun;
	    return force_to_return;
	}
	
	// Calculate solar force as function of
	// sail, orbit, transform, and UT
	public static Vector3d CalculateSolarForce(SolarSailPart sail, Orbit orbit, Vector3d normal, double UT) {
	    if (sail.part != null) {
		Vector3d sunPosition = FlightGlobals.Bodies[0].getPositionAtUT(UT);
		Vector3d ownPosition = orbit.getPositionAtUT(UT);
		Vector3d ownsunPosition = ownPosition - sunPosition;
		// If normal points away from sun, negate so our force is always away from the sun
		// so that turning the backside towards the sun thrusts correctly
		if (Vector3d.Dot (normal, ownsunPosition) < 0) {
		    normal = -normal;
		}
		// Magnitude of force proportional to cosine-squared of angle between sun-line and normal
		double cosConeAngle = Vector3.Dot (ownsunPosition.normalized, normal);
		
		Vector3d force = normal * cosConeAngle * cosConeAngle * sail.surfaceArea * sail.reflectedPhotonRatio * solarForceAtDistance(sunPosition, ownPosition);
		return force;
	    } else {
		return Vector3d.zero;
	    }
	}
    }
}
