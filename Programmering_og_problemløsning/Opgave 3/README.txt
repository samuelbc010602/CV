##Rapport Samuel Cadell
##10i pop

#Lav bibliotek
$  cd src 
$  fsharpc -a simulate.fs

#Kør Blackbox-test
$  cd src 
$  fsharpc -r simulate.dll testSimulate.fsx
$  mono testSimulate.exe
