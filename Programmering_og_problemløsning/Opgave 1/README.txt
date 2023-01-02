#POP 6i SamuelCadell

##XML-filen laves med følgende kommando
$ cd src
$ fsharpc --doc:documentation.xml continuedFraction.fsi continuedFraction.fs

##Biblioteket laves med følgende kommandoer
$ cd src
$ fsharpc -a continuedFraction.fsi continuedFraction.fs

##Whitebox og blackbox testen i continuedFractionTest.fsx compiles således:
$ cd src
$ fsharpc -r continuedFraction.dll continuedFractionTest.fsx
$ mono continuedFractionTest.exe


