# **ESTRUCTURA DE SOFTWARE**

# **SERVICIO PORTAL EMPLEO API**

|  |  |
| --- | --- |
| **CAPA** | BACKEND |
| **PLATAFORMA** | SERVER – WINDOWS |
| **ACCESO** | http://localhost:7083/ |
| **TIPO** | .NET |

## 1. DESCRIPCIÓN GENERAL

El servicio Portal Empleo API proporciona endpoints para gestionar operaciones relacionadas con ofertas de empleo, incluyendo la publicación, edición, listado y postulación a ofertas de trabajo.

## 2. MÉTODOS

### 2.1. Autenticación y Registro

#### 2.1.1. Login

Autentica un usuario en el sistema.

Acceso: `api/Auth/login`  
Formato: JSON  
Servicio: REST / POST

##### 2.1.1.1. Entrada

| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| nombreUsuario | Nombre de usuario | String | Sí |
| contraseña | Contraseña del usuario | String | Sí |
| ip | Dirección IP del cliente | String | No |

Ejemplo de entrada:
```json
{
  "nombreUsuario": "usuario123",
  "contraseña": "password123",
  "ip": "192.168.1.1"
}
```

##### 2.1.1.2. Salida

```json
{
  "exito": true,
  "mensaje": "Usuario autenticado",
  "detalle": "El usuario usuario123 se ha autenticado correctamente.",
  "resultado": {
    "idUsuario": "RECLU000001",
    "nombreUsuario": "usuario123",
    "nombre": "Juan",
    "apellido": "Pérez",
    "email": "juan@email.com",
    "rol": "Reclutador",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

#### 2.1.2. Registro

Registra un nuevo usuario en el sistema.

Acceso: `api/Auth/registro`  
Formato: JSON  
Servicio: REST / POST

##### 2.1.2.1. Entrada

| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| nombreUsuario | Nombre de usuario | String | Sí |
| contraseña | Contraseña del usuario | String | Sí |
| nombre | Nombre real | String | Sí |
| apellido | Apellido | String | Sí |
| email | Correo electrónico | String | Sí |
| rolId | ID del rol (2:Reclutador, 3:Candidato) | Integer | Sí |

Ejemplo de entrada:
```json
{
  "nombreUsuario": "usuario123",
  "contraseña": "password123",
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan@email.com",
  "rolId": 2
}
```

##### 2.1.2.2. Salida

```json
{
  "exito": true,
  "mensaje": "Usuario registrado",
  "detalle": "El usuario se ha registrado correctamente",
  "resultado": {
    "idUsuario": "RECLU000001",
    "nombreUsuario": "usuario123",
    "email": "juan@email.com",
    "rol": "Reclutador"
  }
}
```

### 2.2. Gestión de Ofertas de Empleo

#### 2.2.1. Crear Oferta

Crea una nueva oferta de empleo.

Acceso: `api/OfertaEmpleo`  
Formato: JSON  
Servicio: REST / POST  
Autenticación: JWT (Reclutador)

##### 2.2.1.1. Entrada

| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| titulo | Título de la oferta | String | Sí |
| descripcion | Descripción detallada | String | Sí |
| ubicacion | Ubicación del trabajo | String | Sí |
| salario | Salario ofrecido | Decimal | No |
| idTipoContrato | ID del tipo de contrato | Integer | Sí |

Ejemplo de entrada:
```json
{
  "titulo": "Desarrollador Full Stack",
  "descripcion": "Buscamos desarrollador con experiencia en .NET y React",
  "ubicacion": "Medellín",
  "salario": 5000000.00,
  "idTipoContrato": 1
}
```

##### 2.2.1.2. Salida

```json
{
  "exito": true,
  "mensaje": "Oferta creada",
  "detalle": "La oferta de empleo se ha creado correctamente",
  "resultado": {
    "idOferta": 1,
    "titulo": "Desarrollador Full Stack",
    "descripcion": "Buscamos desarrollador con experiencia en .NET y React",
    "ubicacion": "Medellín",
    "salario": 5000000.00,
    "tipoContrato": "Tiempo Completo",
    "fechaPublicacion": "2024-03-20T10:00:00",
    "nombreReclutador": "Juan Pérez",
    "estado": "Activa"
  }
}
```

#### 2.2.2. Actualizar Oferta

Actualiza una oferta existente.

Acceso: `api/OfertaEmpleo/{idOferta}`  
Formato: JSON  
Servicio: REST / PUT  
Autenticación: JWT (Reclutador)

##### 2.2.2.1. Entrada

Similar a la creación de oferta.

#### 2.2.3. Eliminar Oferta

Marca una oferta como inactiva.

Acceso: `api/OfertaEmpleo/{idOferta}`  
Servicio: REST / DELETE  
Autenticación: JWT (Reclutador)

#### 2.2.4. Obtener Oferta

Obtiene los detalles de una oferta específica.

Acceso: `api/OfertaEmpleo/{idOferta}`  
Servicio: REST / GET

#### 2.2.5. Listar Ofertas

Obtiene un listado paginado de ofertas activas.

Acceso: `api/OfertaEmpleo`  
Servicio: REST / GET

Parámetros de consulta:
- pagina (default: 1)
- tamanoPagina (default: 10)
- filtro (opcional)

### 2.3. Gestión de Postulaciones

#### 2.3.1. Crear Postulación

Registra una nueva postulación a una oferta.

Acceso: `api/Postulacion`  
Formato: JSON  
Servicio: REST / POST

##### 2.3.1.1. Entrada

| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| idOferta | ID de la oferta | Integer | Sí |
| nombre | Nombre del candidato | String | Sí |
| email | Email del candidato | String | Sí |

Ejemplo de entrada:
```json
{
  "idOferta": 1,
  "nombre": "Ana García",
  "email": "ana@email.com"
}
```

##### 2.3.1.2. Salida

```json
{
  "exito": true,
  "mensaje": "Postulación exitosa",
  "detalle": "Su postulación ha sido registrada correctamente",
  "resultado": {
    "idPostulacion": 1,
    "nombre": "Ana García",
    "email": "ana@email.com",
    "fechaPostulacion": "2024-03-20T10:30:00"
  }
}
```

#### 2.3.2. Listar Postulaciones por Oferta

Obtiene las postulaciones de una oferta específica.

Acceso: `api/Postulacion/oferta/{idOferta}`  
Servicio: REST / GET  
Autenticación: JWT (Reclutador)

Parámetros de consulta:
- pagina (default: 1)
- tamanoPagina (default: 10)

## 3. CONSIDERACIONES DE SEGURIDAD

### 3.1. Autenticación
- Se utiliza JWT (JSON Web Tokens) para la autenticación.
- Los tokens tienen una validez limitada y deben ser renovados periódicamente.
- Se requieren headers específicos en las peticiones autenticadas:
  - Authorization: Bearer {token}
  - IdUsuario: {idUsuario}

### 3.2. Control de Acceso
- Los endpoints de gestión de ofertas requieren autenticación como reclutador.
- Las postulaciones pueden ser creadas sin autenticación.
- La consulta de postulaciones solo está disponible para el reclutador propietario de la oferta.

### 3.3. Validación de Datos
- Se implementa validación de modelo en todas las entradas.
- Se aplican restricciones de longitud y formato en campos críticos.
- Se valida la existencia y estado de las ofertas antes de permitir postulaciones.

### 3.4. Registro de Actividad
- Todas las operaciones son registradas en el sistema de logs.
- Se registra información de IP, usuario y detalles de la operación.
- Los errores son capturados y registrados para su análisis.
