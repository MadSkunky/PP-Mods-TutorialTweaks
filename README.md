# TutorialTweaks
Updated Version, completely replaces my previous JS mod "TutorialJacobFix".
(The older mod will be deactivated when installing this "TutorialTweaks" mod per Modnix and can be removed).

## Some Tutorial tweaks, and, most important, Jacob as Sniper as it should be ;-)

Fixes the tutorial so that Jacob appears as a Sniper as he is shown on the start screen and the tutorial cutscenes and adds some configurations to tweak the tutorial members as well as a "new" modified and highly configurable Ares Assault Rifle.

It always bothered me and many others that Jacob appears in the tutorial as an assault although he is shown as a sniper on the start screen and in the cutscenes of the tutorial. That’s over now!

He is equipped with an Ares Assault Rifle, or the special modified Ares if configured, to prevent a soft lock for the first shooting sequence (2 AP necessary). In addition he has the Firebird Sniper Rifle as secondary weapon that also can be used after the first shooting sequences.

The story of the whole tutorial is unchanged, but all personal abilities of the tutorial members can be adjusted by confuguration file. And because Jacobs is changed from Assault to a Sniper the normal game will start with 2 Assaults, 1 Heavy and 2 Snipers.

Have fun!

## How-To configuration
All settings should, more or less, be self-explanatory.
The first settings are to give Jacob assault rifle proficiency and for the new Ares assault rifle, which can be adjusted to anything anyone wants, including to manufacturing it (after researching the "Phoenix Archieves").

"GiveJacobAssaultRifleProficiency": - should do what it says if set to 'true'. Unfortunately it doesn't work 100%, inventory will still show the warning triangle, but it should at least get rid of the accuracy penalty.

"GiveJacobModifiedAres": - if 'true' Jacob get the modified Ares, if 'false' he will get a regular Ares.
### New Rifle Settings
"NoPenaltyWithoutProficiency": - if 'true' the weapon is treated for all classes as proficient. It can be equipped without warning and there is no penalty when assault rifle proficiency is not given. BUT, any skill that needs proficiency like Quick Aim and Rage Burst will not work, for these regular AR proficiency is still necessary.

"Manufacturable": - if 'true' the new Ares can be manufactured after researching the "Phoenix Archives". Its costs can be adjusted below this setting.

"TimePoints": - the time to manufacture is calculated by number of fabs * 4 / these TimePoints.

All the damage values should be self-explanatory, be careful with "Fire", it looks very bugged when applied with direct fiering weapons. "Syphon" is the vampiric damage tritons can do with their melee attack, it is only in this mod because one buddy wants to have it (greetings at Dorante ;-) ).

"ProjectilesPerShot" - can be changed to make it behave more like a shotgun, combining with "BurstCount" will work, so something like 2 bursts á 3 projectiles are possible even when it probably doesn't really fit for an assault rifle. Choices for the user, anything goes ;-)

### Character Settings

"Sophia" is the main actor of the tutorial and comes as an Assault.<br>
"Jacob" is her buddy right from the start and with this mod now a Sniper.<br>
"Omar" is the Heavy they meet in the 2nd part of tactical tutorial.<br>
"Irina" and "Takeshi" are the Sniper and Assault that join the squad in the 3rd part.

The numbers in the first column stands for the level where the ability will be applied. <br>
List of all 14 possible personal abilities:
- "Biochemist"
- "Farsighted"
- "Cautious"
- "Close Quarter Specialist"
- "Bombardier"
- "Sniperist"
- "Trooper"
- "Healer"
- "Quarterback"
- "Reckless"
- "Resourceful"
- "Self Defense Specialist"
- "Strongman"
- "Thief"

Any field that is empty ("") means the soldier will not get an ability at this level, if all are empty he/she will not get any. If one or all soldiers should get randomly generated personal abilities as it is in the vanilla game then the name should be set to "".
