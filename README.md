# biblioteca-backend
Proyecto de biblioteca back

# Seguir los siguientes pasos

# Cadena de conexión
-> Cambiar la cadena de conexión en appsettings.json
-> Compilar solución y con esto re construir las dependiencias.

# Migración DB
-> Correr los siguientes comandos: add-Migration y update-database

# Observaciones
-> Se empleo JWT pero al no contar con autenticación o un usuario que se autentique, se dejo un 
  un controlador expuesto para obtener el token. Se valida contra unas variables de ambiente "NombreUnico"
  y "Usuario" para simular la autenticación.
  
-> Se empleo SeriLog.Extension.Loggin.File como recurso para el manejo de logs.
