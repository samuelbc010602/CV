open drones

///Blackbox-test

printfn "Blackbox-test Drone class"

printfn "Position property"
let middle = Drone((12,4),(1,2),2)
printfn "  %b: Drone((12,4),(1,2),2) = %i %i"
 (middle.Position = (12,4)) (fst(middle.Position))
 (snd(middle.Position))

printfn "Destination property"
printfn "  %b: Drone((12,4),(1,2),2) = %i %i"
 (middle.Destination = (1,2)) (fst(middle.Destination))
 (snd(middle.Destination))

printfn "Speed property"
printfn "  %b: Drone((12,4),(1,2),2) = %i" (middle.Speed = 2)
 (middle.Speed)

//Here I test if the method works if the corresponding
//position has changed in the right direction

printfn "Fly method"
middle.Fly()
printfn "  %b: Drone((12,4),(1,2),2) = %i %i"
 (middle.Position = (10,2)) (fst(middle.Position))
 (snd(middle.Position))

//Here I test by first checking if the method returns
//true if a Drone spawns at it's destination.
//Then I check if the method returns false if a Drone object
//that doesn't spawn at it's destination is instantiated
printfn "AtDestination method"
let atPos = Drone((1,1),(1,1),1)
printfn "  %b: Drone((1,1),(1,1),1) = %b"
 (atPos.AtDestination() = true) (atPos.AtDestination())

let notAtPos = Drone((2,4),(5,1),2)
printfn "  %b: Drone((2,4),(5,1),2) = %b"
 (notAtPos.AtDestination() = false) (notAtPos.AtDestination())

printfn "Blackbox-test for the Airspace class"

//Here I check if the returned Drone instance matches the Drone
//instance that was given as argument to the AddDrone method

printfn "Drones property"
let airspaceMiddle = Airspace()
airspaceMiddle.AddDrone(middle)
printfn "  %b: airspaceMiddle.AddDrone(middle) = %A"
 (airspaceMiddle.Drones.[0] = middle) (middle)

//Here I check if a drone instance has been
//appended to the list of instances
//after the AddDrone method has been called
printfn "AddDrone method"
printfn "  %b: airspaceMiddle.Drones = %A"
 (airspaceMiddle.Drones.[0] = middle) (middle)

printfn "DroneDist method"
let first = Drone((5,5),(2,2),1)
let second = Drone((2,2),(5,5),1)
printfn "  %b: airspaceMiddle.DroneDist (first) (second) = %A"
 (airspaceMiddle.DroneDist (first) (second) = (3,3))
 (airspaceMiddle.DroneDist (first) (second))

//To test this I make a new Airspace class and append
//two new drones, and check
//if the drones have moved in the right direction of
//their respective destination
//To test this I first make an if-statement that checks if
//both the drones have moved in the right direction
let flyDrones = Airspace()
let firstFly = Drone((5,5),(3,3),1)
let secondFly = Drone((10,10),(5,5),1)
flyDrones.AddDrone(firstFly)
flyDrones.AddDrone(secondFly)
flyDrones.FlyDrones()
printfn "FlyDrones method"

let mutable checker : bool = false

if (fst(firstFly.Position)) = 4 && (snd(firstFly.Position)) = 4 &&
 (fst(secondFly.Position)) = 9 && (snd(secondFly.Position)) = 9 then
   checker <- true
else
  checker <- false

printfn "  %b: airspaceMiddle.FlyDrones = (%i,%i) (%i,%i)" (checker)
 (fst(firstFly.Position)) (snd(firstFly.Position))
 (fst(secondFly.Position)) (snd(secondFly.Position))

//To test this method I make two new Airspace objects where I
//first add drones that will collide, and then two that wont't.
//To test this I simply check if the length of the list of collisions
//has increased in case of a collision, and check if it
//hasn't increased in case of no collision
printfn "WillCollide method"
let yesCol = Airspace()
let firstYesCol = Drone((5,5),(7,7),1)
let secondYesCol = Drone((4,4),(-1,-1),1)
yesCol.AddDrone(firstYesCol)
yesCol.AddDrone(secondYesCol)
printfn "  %b: yesCol.WillCollide 1 = %i"
 ((yesCol.WillCollide 1).Length = 1) ((yesCol.WillCollide 1).Length)

let noCol = Airspace()
let firstNoCol = Drone((5,5),(7,7),1)
let secondNoCol = Drone((-3,-3),(-1,-1),1)
yesCol.AddDrone(firstNoCol)
yesCol.AddDrone(secondNoCol)
printfn "  %b: noCol.WillCollide 1 = %i"
 ((noCol.WillCollide 1).Length = 0) ((noCol.WillCollide 1).Length)
