module drones

///<summary> A class that represents the
///functionalities of the drone </summary>
///<param name='position'> The start position in
///coordinate format (x,y) </param>
///<param name='destination' The destination for the drone in
//coordinate format (x,y) </param>
///<param name='speed'> The speed in cm/second </param>
///<returns> A Drone object with a position,
///destination and a speed </returns>
type Drone(position:int*int, destination:int*int,speed:int) =
  let mutable (xMove,yMove) = position
  let mutable (xDes,yDes) = destination
  ///<summary> The position of the drone </summary>
  member this.Position = (xMove,yMove)
  ///<summary> The destination of the drone </summary>
  member this.Destination = destination
  ///<summary> The speed of the drone </summary>
  member this.Speed = speed

  ///<summary> Moves the drone's position after
  //one second of flight</summary>
  member this.Fly() =
    if this.AtDestination() = false then
      if xMove < xDes then
        xMove <- xMove+speed
        if xMove >= xDes then
          xMove <- xDes
      else
        xMove <- xMove-speed
        if xMove <= xDes then
          xMove <- xDes
          
      if yMove < yDes then
        yMove <- yMove+speed
        if yMove >= yDes then
          yMove <- yDes
      else
        yMove <- yMove-speed
        if yMove <= yDes then
          yMove <- yDes
          
  ///<summary> Sets the drone's coordinates
  ///to the destination </summary>
  member this.ignoreDrone =
    xMove <- xDes
    yMove <- yDes
  
  ///<summary> Checks if the drones has
  //reached it's destination </summary>
  ///<returns> A boolean </returns>
  member this.AtDestination() : bool =
   (xMove = xDes) && (yMove = yDes)

///<summary> A class representing the space in
///which Drones travel in </sumary>
///<returns> A Airspace object </returns>
type Airspace() =
  let mutable instanceList = []
  let mutable dronesCollide = []

  ///<summary> The list of Drone instances </summary>
  member this.Drones = instanceList

  ///<summary> Adds a new Drone instance
  ///to the list of instances </summary>
  ///<param name='droneToAppend'> A drone
  ///instance to add to the list of instances </param>
  ///<returns> unit </return> 
  member this.AddDrone (droneToAppend:Drone) =
    instanceList <- droneToAppend::instanceList

  ///<summary> Returns the distance between
  ///two Drones in coordinate format (x,y) </summary>
  ///<param name='firstDrone'> A Drone instance </param>
  ///<param name='secondDrone'> A Drone instance </param>
  ///<returns> A tuple </returns>
  member this.DroneDist (firstDrone:Drone) (secondDrone:Drone) =
    ((abs (fst(firstDrone.Position)-fst(secondDrone.Position))),
     (abs (snd(firstDrone.Position)-snd(secondDrone.Position))))

  ///<summary> Changes all the Drone's position
  ///after one second of fligth </summary>
  ///<returns> unit </returns>
  member this.FlyDrones() =
    for i in instanceList do
      if i.AtDestination() = false then
        i.Fly()

  ///<summary> Checks if any drones collide
  ///in the given interval in minutes </summary>
  ///<param name='numOfMin'> Time in minutes </param>
  ///<returns> unit </returns>
  member this.collisionCheck (numOfMin:int) =
    let secToAdd = numOfMin*60
    for outer in 1..secToAdd do
      this.FlyDrones()
      for firstDrone in 0..(instanceList.Length-1) do
        for secondDrone in 0..(instanceList.Length-1) do
          if firstDrone <> secondDrone then
            if instanceList.[firstDrone].AtDestination() = false
             && instanceList.[secondDrone].AtDestination() = false then
              let middle =
                this.DroneDist
                 (instanceList.[firstDrone])
                  (instanceList.[secondDrone])
              if fst(middle) <= 5 && snd(middle) <= 5 then
                dronesCollide <-
                 (firstDrone,secondDrone)::dronesCollide
                instanceList.[firstDrone].ignoreDrone
                instanceList.[secondDrone].ignoreDrone
                
  ///<summary> Checks if any drones collide
  ///in the given interval in minutes </summary>
  ///<param name='numOfMin'> Time in minutes </param>
  ///<returns> List with Drone collisions </returns>
  member this.WillCollide (numOfMin:int) =
    this.collisionCheck numOfMin
    dronesCollide
