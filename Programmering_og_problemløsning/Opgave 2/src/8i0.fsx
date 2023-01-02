type point = int * int
type color = ImgUtil.color
let gray : color = ImgUtil.fromRgb(128,128,128)
type figure =
  |Circle of point * int * color
  |Rectangle of point * point * color
  |Mix of figure * figure

///<Summary>Creates a figure consisting of a circle with radius of 45
///and a center in (50,50) and a Rectangle with a left corner in
///(40,40) and right corner in (90,110) </Summary>
///<Returns>Returns a figure </Returns>
let figTest : figure =
  Mix(Circle(point(50,50),45,ImgUtil.red),
      Rectangle(point(40,40),point(90,110),ImgUtil.blue))

let rec colorAt (x,y) figure =
  match figure with
    |Circle ((cx,cy), r, col) ->
      if (x-cx)*(x-cx)+(y-cy)*(y-cy) <= r*r then Some col else None
    |Rectangle ((x0,y0), (x1,y1), col) ->
      if x0 <= x && x <= x1 && y0 <= y && y <= y1 then
        Some col else None
    |Mix (f1, f2) ->
      match (colorAt (x,y) f1, colorAt (x,y) f2) with
        |(None, c) -> c // no overlap
        |(c, None) -> c
        |(Some c1, Some c2) ->
        let (a1,r1,g1,b1) = ImgUtil.fromColor c1
        let (a2,r2,g2,b2) = ImgUtil.fromColor c2
        in Some(ImgUtil.fromArgb((a1+a2)/2, (r1+r2)/2,  // calculate
                                (g1+g2)/2, (b1+b2)/2)) //average
                                
///<Summary> Makes a picture in png format of a given figure </Summary>
///<Param name='fileName'> A string to use as the filename </Param>
///<Param name='fig'> A figure </Param>
///<Param name='width'> The width of the canvas as integer </Param>
///<Param name='height'> The height of the canvas as integer </Param>
///<Returns> Returns unit </Returns>
let makePicture (fileName:string) (fig:figure)
 (width:int) (heigth:int) =
  let can = ImgUtil.mk width heigth
  let rec drawToCan (nextPoint:point) =
    if colorAt (nextPoint) fig = None then
      ImgUtil.setPixel (gray) (nextPoint) (can)
    else
      ImgUtil.setPixel (colorAt (nextPoint)
                         (fig)).Value (nextPoint) (can)
    if fst(nextPoint) < width-1 then
      drawToCan (fst(nextPoint)+1,snd(nextPoint))
    else
      if snd(nextPoint) < heigth-1 then
        drawToCan (0,snd(nextPoint)+1)
  drawToCan (0,0)
  ImgUtil.toPngFile (fileName+".png") (can)  

makePicture "figTest" figTest 100 150

///<Summary>Checks if the figure is valid </Summary>
///<Param name='fig'> A figure </Param>
///<Returns> Returns true if the figure is valid, otherwise it
///returns false </Returns>
let rec checkFigure (fig:figure) : bool =
  match fig with
    |Circle((x,y),rad,col) -> rad >= 0
    |Rectangle((x1,y1),(x2,y2),col) ->
      (x1 <= x2 && y1 <= y2) ||
       (x1 = 0 && x2 = 0 && y1 = 0 && y2 = 0) 
    |Mix(f1,f2) ->
      (checkFigure f1) && (checkFigure f2) 

///<Summary> Moves the figure along an vector </Summary>
///<Param name='fig'> A figure </Param>
///<Param name='movePoint'> The vector to
///move the figure along </Param>
///<Returns> Returns a new figure that
///has been moved accordingly </Returns>
let rec move (fig:figure) (movePoint:point) : figure =
    match fig with
      |Rectangle((x1,y1),(x2,y2),col) ->
        Rectangle((x1+fst(movePoint),y1+snd(movePoint)),
                  (x2+fst(movePoint),y2+snd(movePoint)),col)
      |Circle((x1,y1),rad,col) -> Circle((x1+fst(movePoint),
                                          y1+snd(movePoint)),rad,col)
      |Mix(f1,f2) -> Mix((move f1 movePoint),
                         (move f2 movePoint))

///<Summary> Makes the smallest rectangle that covers
///the whole figure </Summary>
///<Param name='fig'> A figure </Param>
///<Returns> Returns two points that corresponds to the left- and
//right corner of the smallest rectangle that
///covers the given figure</Returns>
let rec boundingBox (fig:figure) : point*point =
  match fig with
    |Circle(center,rad,color) -> ((fst(center)-rad,snd(center)-rad),
                                  (fst(center)+rad,snd(center)+rad))
    |Rectangle(first,second,col) -> (first,second)
    |Mix(f1,f2) -> let ((f1x1,f1y1),(f1x2,f1y2)) = boundingBox f1
                   let ((f2x1,f2y1),(f2x2,f2y2)) = boundingBox f2
                   ((min f1x1 f2x1,min f1y1 f2y1),
                    (max f1x2 f2x2,max f1y2 f2y2))


