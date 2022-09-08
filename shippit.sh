dotnet publish ./src/KordanorsCabal/KordanorsCabal.vbproj -o ./pub-linux -c Release --sc -r linux-x64
dotnet publish ./src/KordanorsCabal/KordanorsCabal.vbproj -o ./pub-windows -c Release --sc -r win-x64
butler push pub-windows thegrumpygamedev/kordanors-cabal:windows
butler push pub-linux thegrumpygamedev/kordanors-cabal:linux
git add -A
git commit -m "shipped it!"