* DONE Save current throttle and Isp to other variables
* DONE During timewarp, perturb orbit and mass based on saved throttle and Isp [3/3]
** DONE Perturb orbit
** DONE Perturb mass
** DONE Perturb other resources (e.g. energy)
* DONE Look up energy consumption
* DONE Look up mass consumption
* DONE Format thrust in more readable form [3/3]
** DONE mN when < 1 N
** DONE N when < 1000 N
** DONE kN when < 1000000 N
* DONE Drop out of timewarp on depletion of any resource
* DONE Scale energy to more realistic amount
* DONE Show resource consumption
* DONE Test if this works for multiple engines and sails [2/2]
** DONE Multiple engines
** DONE Multiple sails
* DONE Use one or more deltaV propellants [2/2]				:BUG:
** DONE Iterate over list of deltaV propellants and calculate demand and deltaV
** DONE Automatically detect which propellants contribute to deltaV
*** Do they have density units? Yes.
* DONE Modularize PersistentEngine with methods used by OnFixedUpdate [5/5] :FEATURE:
** DONE UpdatePersistentParameters
** DONE CalculateDemands
** DONE ApplyDemands
** DONE CalculateDeltaVV
** DONE ApplyDeltaVV
* DONE Make PersistentEngine a separate addon that uses existing ModuleEngines* module [6/6] :FEATURE:
** DONE Add "engine" field to PersistentEngine class
** DONE Make class inherit PartModule
** DONE Use "engine" field instead of direct fields
** DONE Make module search vessel for ModuleEngine* module
** DONE Update ion engine config to add PersistentEngine instead of replace ModuleEnginesFX
** DONE Disable persistent features when ModuleEngine not found
* TODO Realistic sail attitude control				   :WISHLIST:
** Vanes to cause solar torque about CM
** Gimbaled boom to shift CM
** Realistic moments of inertia (i.e. BIG)
** IKAROS style thin film reflective control devices
* TODO Make new solar sail models [0/5]			   :WISHLIST:FEATURE:
** TODO Square sail (LightSail, Lunar Flashlight, NEA Scout)
*** 4 triangular panels with billowed shape
*** Wrinkled looking surface
** TODO Sunjammer
*** 4 triangular panels with stripe net and billow in between
** TODO Steering vanes
*** Vanes at boom tips that rotate to face sun with ~20 deg cant for stability
** TODO Halley Rendezvous square sail
*** Big, billowed sail with masts and stays
** TODO IKAROS
*** Animated rotation
**** Scale rotation speed to sensible amount during time warp
*** Animate change in reflective control device color as it performs attitude maneuvers
* TODO Make TweakScale work with parts [0/2]			    :FEATURE:
** TODO Solar sail
** TODO Ion engine
* TODO With solar electric ion engine spacecraft, batteries deplete in high timewarp, even though the panels generate enough ElectricCharge [1/2] :BUG:
** DONE Temporarily bypass by turning off resource request and depletion
** TODO Fix
** Sample craft: 1 ion engine, 2 gigantor XL solar arrays, Z-200 battery
** 5% throttle
** At 100x and 1000x, batteries a little low
** At 1000x, batteries ~50%
** At max warp, batteries deplete, but vessel doesn't shutdown
* TODO In timewarp, persistent engines lower a suborbital engine when they should be raising it [1/2] :BUG:
** DONE Temporary fix: throw error message when suborbital & in timewarp, don't perturb orbit
** TODO Make it work
* TODO Add GUI window that shows propellant data [0/3] 		    :FEATURE:
** TODO Button in main GUI to toggle
** TODO List of propellant names
** TODO List of propellant rates
* TODO Test if resource wasn't fully requested (demandOut < demand) [0/6] :FEATURE:BUG:
** TODO Test to make sure demandOut always == demand during normal operation
** TODO Scale mass change dm to ratio of demandOut/demand
** TODO Ensure a similar ratio of other propellants was applied
** TODO Apply a reduced deltaV
** TODO Flag depletion
* TODO Do we want to calculate PersistentThrust as a function of PersistentThrottle, minThrust, and maxThrust? :FEATURE:
