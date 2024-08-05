# CineAPI

Bienvenido a la API de CineAPI, una API RESTful para gestionar películas. A continuación, se presenta la documentación para los clientes que desean utilizar esta API.

## Tabla de Contenidos

1. [Autenticación](#autenticación)
2. [Endpoints](#endpoints)
   - [Registro](#registro)
   - [Inicio de Sesión](#inicio-de-sesión)
   - [Obtener Películas](#obtener-películas)
   - [Obtener Película por ID](#obtener-película-por-id)
   - [Añadir Película](#añadir-película)
   - [Actualizar Película](#actualizar-película)
   - [Eliminar Película](#eliminar-película)
3. [Ejemplos de Solicitudes](#ejemplos-de-solicitudes)

## Autenticación
Esta API utiliza tokens JWT para la autenticación. Asegúrese de incluir un token válido en la cabecera `Authorization` de cada solicitud protegida.

**Formato del Token JWT**:
`Authorization: Bearer <token>`

## Endpoints

### Registro
**Endpoint**: `POST /api/users/register`  
**Descripción**: Permite a nuevos usuarios registrarse en el sistema.

#### Solicitud
```json
{
  "email": "string",
  "password": "string"
}
```
#### Respuesta
```json
{
  "token": "string"
}
```

### Inicio de Sesión
**Endpoint**: `POST /api/users/login`  
**Descripción**: Permite a usuarios registrados iniciar sesión en el sistema.

#### Solicitud
```json
{
  "email": "string",
  "password": "string"
}
```
#### Respuesta
```json
{
  "token": "string"
}
```

### Obtener Películas
- **Endpoint**: `GET /api/movies`
- **Descripción**: Obtiene una lista de todas las películas disponibles en el sistema.
- **Autenticación**: Requiere token JWT.

#### Respuesta
```json
[
  {
    "id": 0,
    "title": "string",
    "director": "string",
    "genre": "string",
    "releaseDate": "2024-08-05T10:58:01.570Z",
    "duration": 0,
    "description": "string",
    "originalLanguage": "string"
  }
]
```

### Obtener Película por ID
- **Endpoint**: `GET /api/movies/{movieId}`
- **Descripción**: Obtiene los detalles de una película específica usando su ID.
- **Autenticación**: Requiere token JWT.

#### Parámetros de URL
- `movieId` (int): El ID de la película que deseas obtener.

#### Respuesta
```json
{
  "id": 0,
  "title": "string",
  "director": "string",
  "genre": "string",
  "releaseDate": "2024-08-05T11:01:59.369Z",
  "duration": 0,
  "description": "string",
  "originalLanguage": "string"
}
```

### Añadir Película
- **Endpoint**: `POST /api/movies`
- **Descripción**: Añade una nueva película a la base de datos.
- **Autenticación**: Requiere token JWT.

#### Solicitud
```json
{
  "title": "string",
  "director": "string",
  "genre": "string",
  "releaseDate": "dd/MM/aaaa",
  "duration": 0,
  "description": "string",
  "originalLanguage": "string"
}
```

#### Respuesta
```text
0
```
Donde `0` representa el ID de la película creada.

### Actualizar Película
- **Endpoint**: `PUT /api/movies/{movieId}`
- **Descripción**: Actualiza los detalles de una película existente utilizando su ID.
- **Autenticación**: Requiere token JWT.

#### Parámetros de URL
- `movieId` (int): El ID de la película que deseas actualizar.

#### Solicitud
```json
{
  "title": "string",
  "director": "string",
  "genre": "string",
  "releaseDate": "dd/MM/aaaa",
  "duration": 0,
  "description": "string",
  "originalLanguage": "string"
}
```

#### Respuesta
```text
true
```
Retorna `true` si los datos fueron actualizados correctamente.

### Eliminar Película
- **Endpoint**: `DELETE /api/movies/{movieId}`
- **Descripción**: Elimina una película existente utilizando su ID.
- **Autenticación**: Requiere token JWT.

#### Parámetros de URL
- `movieId` (int): El ID de la película que deseas eliminar.

#### Respuesta
```text
true
```
Retorna `true` si la película fue eliminada correctamente.

## Ejemplos de Solicitudes
### Solicitud para Registrar Usuario
```bash
curl -X POST "http://www.cineapi.somee.com/api/users/register" \
     -H "Content-Type: application/json" \
     -d '{"email": "ejemplo@registro.com", "password": "Abcd1234@@"}'
```

### Solicitud para Obtener Películas
```bash
curl -X GET "https://www.cineapi.somee.com/api/movies" \
     -H "Authorization: Bearer {tu_token_aqui}"
```
