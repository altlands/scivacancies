:menu
@set /p input=DropDatabase before applying migrations? [y/n] (default - n)

@if "%input%" EQU "y" (rh.exe --databasetype=postgresql -c "Server=localhost;Database=scivacancies;User Id=postgres;Password=ekobka" -vf version.txt -env=EKOBKA -silent /drop)

@rh.exe --databasetype=postgresql -c "Server=localhost;Database=scivacancies;User Id=postgres;Password=ekobka" -vf version.txt -env=EKOBKA -silent

@goto menu