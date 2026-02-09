# Certified PMC

A mod for SP Tarkov, an offline single player emulator for the game Escape from Tarkov.

Randomizes your PMC's skills and masteries at character creation, because not every 'trained operative' starts life the same way.

**New profile is required!**

**Does not modify SCAV's skills.**

## Configuration

The mod can be configured by editing the `config.json` file. The following options are available:
- minSkillLevel: The minimum level for any skill (default: 0)
- maxSkillLevel: The maximum level for any skill (default: 1500)
	- 100 represents 1 skill level, so 1500 is actually level 15
- minMasteryLevel: The minimum level for any mastery (default: 0)
- maxMasteryLevel: The maximum level for any mastery (default: 1000)
- Skills list:
	- if `true` the skill will be randomized, if `false` it will be set to the 0 level
- Mastering:
	- SplitByFaction (default: true):
		- if `true` the mastery levels will be randomized separately for each faction (USEC, BEAR), according to the list below this option
		- if `false` both BEAR and USEC weapon masteries will be randomized together

## Installation
Copy contents of the archive to root SPTarkov directory.

Want to support my work? Buy me a Coffee [ko-fi/fryciarz7](https://ko-fi.com/fryciarz7/tip)