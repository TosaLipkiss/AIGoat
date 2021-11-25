# AIGoat

När man startar upp från en ny omstart på datorn så tappas en reference i scenen (unity bugg?).

Det fixar man genom att:

1. Det är på Farmer (1).
2. Öppna Farmer (1)
3. Markera Farmer001
4. Ta bort FarmerBlendShape scriptet
5. lägg på farmerBlendShape scriptet igen
6. Markera Farmer (1)
7. I scriptet "FarmerAgent" så ser man en public s.k Farmer Blend Shape.
8. Drag and drop farmer001 i den referensen.

Nu ska det funka igen :)

Andra buggar:

1. Ibland direkt i start så kan en svamp råka spawna förnära Goat. Vilket sabbar hans startdestination.
Restarta igen isåfall.

2. En svamp kan spawna i huset, men stör AI bara ibland. 
