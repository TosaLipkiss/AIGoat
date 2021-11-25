# AIGoat

Buggar:

1. Ibland direkt i start så kan en svamp råka spawna för nära Goat. Vilket sabbar hans startdestination och han "moon walkar".
Restarta igen isåfall för en bättre start.

2. En svamp kan spawna i huset, men stör AI bara ibland. 





OBS: (denna lösning är ej relevant längre)
Tagit/Markerat ut all script för blendshape för "Farmer" för att ta bort unitybuggen med missing referens. Farmern saknar nu sin blend shape.
Gamla lösningen var iaf:

När man startar upp från en ny omstart på datorn så tappas en reference i scenen (unity bugg?). Vilket gör att Farmer inte fungerar.
Det fixar man genom att:

1. markera Farmer (1).
2. Öppna Farmer (1)
3. Markera Farmer001 (finns under farmer (1))
4. Ta bort FarmerBlendShape scriptet
5. lägg på farmerBlendShape scriptet igen
6. Markera Farmer (1)
7. I scriptet "FarmerAgent" så ser man en public s.k Farmer Blend Shape.
8. Drag and drop farmer001 i den referensen.

Nu ska det funka igen :)
(denna lösning är ej relevant längre)
