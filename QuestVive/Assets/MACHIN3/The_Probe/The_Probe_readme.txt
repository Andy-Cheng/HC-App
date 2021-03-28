///////// MACHIN3 /////////
---------------------------
/// http://machin3.io   ///
/// support@machin3.io  ///
---------------------------

Thanks for your purchase!

Please see this Unity Community thread for an illustrated and up-to-date overview:
http://forum.unity3d.com/threads/the-probe.426208/



/// ATTENTION!

If you import the package and Unity mentions:
"A material is using the texture as a normal map. The texture must be marked as a normal map in the import settings."
You have the option to ignore or to fix this now.

You should ignore, if you intend to use the procedural substance materials.
You should fix now, if you intend to use the static standard materials.

/// Introduction 

The Probe is a spherical, unmanned vehicle capable of floating and flying and comparable to a drone in its autonomous or remote controlled operation.

This asset is quite detailed and created with an eye towards current and next-generation needs in the gaming, VR and real-time film spaces.

/// Contents

This package contains 'The Probe' asset v1.0. 

While not specifically advertised as a modular asset, the probe is built from and provided as 4 basic mesh components:
    * main
    * inside 
    * front(sensor)
    * back(thrusters)

Note, that the inside part is actually 2 seperate meshes - front and back.

Each of the 4 main parts has its own texture-set and material.

Having the inside parts separated allows you to not render them, if you choose to have the front and back parts attached. This saves texture memory and avoids rendering unnecessary polygons.

At the same time it's giving you the flexibility to get more out of the asset as well as giving us the flexibility to potentially extend the asset in the future, although that completely depends on how The Probe performs on the asset store, so no promises.
In case you would be interested in extensions to this asset of any kind, we welcome your input and suggestions either here or via email at support@machin3.io.

Materials for The Probe are provided in form of substance .sbsar files, which contain procedural materials allowing for significant customization. 

Highlights:
    * 2 main, plain material choices - painted metal and plastics
    * a mix of both, where the plastic parts act as accents to the painted metal parts
    * a 4th camouflage material
    * emissive controls for each
    * parametric dirt
    * hero decal: use your own logo/icon on the model!

Standard materials are also included as a fallback solution, in case you don't need to customize or want to do it directly in the textures through image editing.

There's prefabs for The Probe in different assembly variations for substance materials as well es for standard(static) materials.

/// Statistics

Polygons/Triangles
    * 26K main + front + back
    * 35K main + insides + front + back
    * 18K main + insides
Textures sets​
    * 2048x2048 main
    * 2048x2048 insides
    * 2048x2048 front (sensor)
    * 2048x2048 back (thrusters)
Textures
    * AlbedoTransparency
    * MetallicSmoothness
    * Normal
    * Fluting (Normal)
    * Emissive
    * AO
    * Curvature
    * substance specific masks

All textures provided in .png format.

Materials
    * Substance .sbsar (procedural)
    * default materials* (static)

If you want to use default materials, you need to manually set all normal maps to "Normal" texture type, instead of the default "Texture" texture type, which is required for the substance materials to work. A good way to do this is to select the appropriate materials and use the "Fix now" button next to the normal input.

/// Requirements

Unity 5.3 is the "substance parity" release, meaning it's visually consistent with what artists create in Allegorithmic's Substance products. The Probe will work in earlier versions of Unity too, but we'd advice you to use a GGX shader nonetheless to achieve the highest visual quality.

Enjoy and good luck with your projects!
