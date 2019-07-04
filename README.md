# Before
![before](https://github.com/DeStilleGast/dnSpy-open-attached-process/raw/master/img/before.gif)

# After
![after](https://github.com/DeStilleGast/dnSpy-open-attached-process/raw/master/img/after.gif)
(note: opened document gets closed after debugging)

# How to build
# To add in your dnSpy
- build the dll from the source or from the release tab here: https://github.com/DeStilleGast/dnSpy-open-attached-process/releases
- place the dll file in the directory where dnSpy.exe is located or in a subfolder called "Extensions"
- (re)start dnSpy
- enjoy

# How to build
- Follow instructions to add references from here https://github.com/0xd4d/dnSpy/wiki/Extensions
- Add `dnSpy.Contracts.Debugger.dll` as reference (because of this, it might break)
- build it
- Copy "dnSpy show attach process.x.dll" to your dnSpy folder location (or a subfolder named "Extensions" and place the dll in there)
- (re)start dnSpy
- enjoy
