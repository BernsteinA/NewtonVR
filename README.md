## Summary

Our system allows players to pick up, drop, throw, and use held objects. Items don't pass through other items (rigidbodies), or the environment (non-rigidbodies). Rather, held items interact with other rigidbodies naturally -- taking mass into account. For example, if you have two boxes of the same mass they can push each other equally, but a balloon, with considerably less mass, can't push a box.

Items can be configured to be picked up at any point, or when grabbed, can rotate and position themselves to match a predefined orientation. This lets you pick up a box from its corner as well as pick up a gun and have it orient to the grip.
<img class='gfyitem' data-id='ImpureTautBergerpicard' />

We've also created a few physical UI elements to help with basic configuration and "menu type scenarios. We also give you the option to dynamically let the controllers turn into physical objects on a button press.
<img class='gfyitem' data-id='PointlessImperturbableBorzoi' />

<br>
NewtonVR brought to you by [Tomorrow Today Labs](http://www.tomorrowtodaylabs.com):

**Development:** Keith Bradner, Nick Abel, Amanda End<br>**Interaction Design:** Adrienne Hunter<br>**Modeling:** Wesley Eldridge, Carli Wood<br>**Audio:** Joshua Du Chene


<br>
Windows Mixed Reality support by:
[@Jesseric](https://twitter.com/jerressic) and ([@meulta](https://twitter.com/meulta))
