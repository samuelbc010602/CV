module continuedFraction

let compareFloat (x : float) (y : float) : bool =
    System.Math.Abs(x - y) < 0.0000001

let rec cfrac2float (lst:int list) : float =
  if lst.Length = 0 then
    0.0
  else if lst.Length = 1 then
    float (int lst.Head)
  else
    let ret = float lst.[0]
    let rec recC (newList:int list) =
      if newList.Length > 1 then
        1.0/((float newList.[0])+(recC newList.Tail))
      else
        1.0/(float newList.[0])
    ret+(recC lst.Tail)
    
let rec float2cfrac (x:float) : int list =
  if x < 1e-10 then
    []
  else if (float(int(x)))-x = 0.0 then
    [int x]
  else if x > 150000.0 then
    []
  else
    let xFloor = int (floor (x+1e-10))
    let rest = x-(float xFloor)
    let xi1 = 1.0/rest
    if abs rest < 1e-10 then
      [xFloor]
    else
      let newList = xFloor::float2cfrac(xi1)
      newList
