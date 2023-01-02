# DIKUGames
Dette var et eksamensprojekt jeg havde i forbindelse med kurset SoftwareUdvikling på KU på første år. 
Vi fik til opgave at lave et spil der minder meget om det klassiske "Space Invaders" video-spil fra 80'erne, hvor vi skulle implementere mange af de teknikker og metoder vi har lært igennem hele kurset. Selve spillet er bygget oven på en game-engine som vi fik stillet til rådighed af DIKU, den har jeg ikke linket til her, denne mappe indeholder kun de filer jeg selv har lavet. 

Koden følger det objekt-orienterede paradigme, hvor "Solid Principles" er blevet fulgt meget nøje for at gøre koden modulær og adapterbar til nye ændringer, hvilket der kom med to ugers mellemrum over en seks ugers periode. Jeg har lagt meget vægt på at opfylde dependency inversion princippet, ved at lave en facade (facade.cs) hvorigennem høj-niveau kode kommunikerer igennem en facade der skjuler og simplificere mange af de forskellige lav-niveau funktionaliteter fra selve spilmotoren (game engine) der er nødvendige for at spillet kan fungere. Udover dette er der også blevet lagt meget vægt på testning for kunne verificere at funktionaliteten af programmet opfylder specifikationerne. Her hare jeg stræbet efter at opnå C-0 og C-1 test coverage.

Udover dette har jeg oså inkluderet en pdf-fil med rapporten, der dokumentere hele arbejdsprocessen der har ligget bag programmet, hvis det kunnne være af interesse


