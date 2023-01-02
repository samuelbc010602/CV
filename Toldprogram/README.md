# Toldprogram
Dette var et program jeg lavede i forlængelse af min fars arbejde. Jeg fik stillet til opgave at lave et program der kunne hente alle toldkoder
fra et bestemt land og sammenligne dem med andre landes toldkoder og derefter rangere dem i forhold til de varer, hvor der var den største difference, med andre ord
finde de vare hvor et bestemt land skulle betale mindst i told for at sælge det på det danske marked i forhold til andre lande.

Selve programmet bruger parallelisering til at læse toldkoder samtidig for mindske køretiden af selve programmet. Ved at parallelisere opgaven kunne man 
gøre det meget hurtigere end hvis man bare havde brugt en kerne til at læse samtlige tusinder af toldkoder sekventielt. Udover dette bruger jeg også webscraping til at læse fra data fra internettet, bemærk at dette program kræver at man har installeret en webdriver før man kan køre programmet. 
