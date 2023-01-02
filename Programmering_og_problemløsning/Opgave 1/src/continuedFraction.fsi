module continuedFraction
///<summary> Calculates complex number when given a continued fraction </summary>
///<param name="lst"> An integer list </param>
///<returns> It returns a float number </returns>
val cfrac2float : int list -> float

///<summary> Takes a float number and calculates the continued fraction </summary>
///<param name="x"> A floating point number </param>
///<returns> It returns a integer list </returns>
val float2cfrac : float -> int list
