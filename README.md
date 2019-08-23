* PersistentThrust Restored and Revectored
A plugin for [[http://www.kerbalspaceprogram.com][Kerbal Space Program]] to allow low thrust propulsion
systems like ion engines and solar sails to operate during time warp.
** Requirements
- [[http://forum.kerbalspaceprogram.com/threads/55219-Module-Manager-1-5-6-%28Jan-6%29][ModuleManager]]
** What works
- Engines and solar sail continue thrusting during realtime and timewarp
- Engine propellant mass and other resources depleted during timewarp
- Engine throttle stays set from realtime to timewarp and back
- Engines with multiple propellants (e.g. liquid fuel & oxidizer) plus
  massless propellants (e.g. ElectricCharge)
** What doesn't work
- Continuous thrust engine throttle and sail attitude can't be changed during timewarp
- Operating engines during timewarp while on a suborbital trajectory
** Navigation
A companion project enables planning and previewing continous thrust
trajectories by scheduling attitude and throttle maneuvers. See
[[http://github.com/bld/SolarSailNavigator][SolarSailNavigator]] for the current version.
